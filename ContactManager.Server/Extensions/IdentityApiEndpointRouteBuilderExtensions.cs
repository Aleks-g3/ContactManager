using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ContactManager.Server.DTOs;
using ContactManager.Server.Entities;
using ContactManager.Server.Services;
using ContactManager.Server.Profiles;
using ContactManager.Server.Exceptions;
using Microsoft.SqlServer.Server;

namespace ContactManager.Server.Extensions
{
    public static class IdentityApiEndpointRouteBuilderExtensions
    {
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();

        public static IEndpointConventionBuilder MapCustomIdentityApi<TUser>(this IEndpointRouteBuilder endpoints)
            where TUser : Contact, new()
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<TUser>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            var routeGroup = endpoints.MapGroup("");

            routeGroup.MapPost("/register", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] RegisterFormDTO registration, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                var userManager = sp.GetRequiredService<UserManager<TUser>>();

                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException($"{nameof(MapCustomIdentityApi)} requires a user store with email support.");
                }

                var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                var emailStore = (IUserEmailStore<TUser>)userStore;
                var email = registration.Email;

                if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
                {
                    return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
                }

                var user = new TUser
                {
                    Name = registration.Name,
                    Surname = registration.Surname,
                    PhoneNumber = registration.PhoneNumber,
                    Category = registration.Category.ConvertToEnum<Category>().GetValueOrDefault(),
                    SubCategory = registration.SubCategory.ConvertToEnum<SubCategory>(),
                    OtherSubCategory = registration.OtherSubCategory,
                    BirthDate = registration.BirthDate.ConvertToDateTime(),
                };
                await userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, email, CancellationToken.None);
                var result = await userManager.CreateAsync(user, registration.Password);

                if (!result.Succeeded)
                {
                    return CreateValidationProblem(result);
                }

                return TypedResults.Ok();
            }).RequireAuthorization();

            routeGroup.MapPost("/login", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
                ([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetRequiredService<SignInManager<TUser>>();

                var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
                var isPersistent = (useCookies == true) && (useSessionCookies != true);
                signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

                var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

                if (result.RequiresTwoFactor)
                {
                    if (!string.IsNullOrEmpty(login.TwoFactorCode))
                    {
                        result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                    }
                    else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                    {
                        result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                    }
                }

                if (!result.Succeeded)
                {
                    return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
                }

                // The signInManager already produced the needed response in the form of a cookie or bearer token.
                return TypedResults.Empty;
            });

            routeGroup.MapPost("/update", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] UpdateFormDTO formDTO, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                var userManager = sp.GetRequiredService<UserManager<TUser>>();

                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException($"{nameof(MapCustomIdentityApi)} requires a user store with email support.");
                }

                var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                var emailStore = (IUserEmailStore<TUser>)userStore;
                var email = formDTO.Email;

                var existingUser = await userStore.FindByIdAsync(formDTO.Id, CancellationToken.None);

                if(existingUser is null)
                    throw new NotFoundException($"user with id '{formDTO.Id}' does not exist");

                if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
                {
                    return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
                }
                
                existingUser.Name = formDTO.Name;
                existingUser.Surname = formDTO.Surname;
                existingUser.PhoneNumber = formDTO.PhoneNumber;
                existingUser.Category = formDTO.Category.ConvertToEnum<Category>().GetValueOrDefault();
                existingUser.SubCategory = formDTO.SubCategory.ConvertToEnum<SubCategory>();
                existingUser.OtherSubCategory = formDTO.OtherSubCategory;
                existingUser.BirthDate = formDTO.BirthDate.ConvertToDateTime();

                await userStore.SetUserNameAsync(existingUser, email, CancellationToken.None);
                await emailStore.SetEmailAsync(existingUser, email, CancellationToken.None);
                var result = await userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    return CreateValidationProblem(result);
                }
                
                return TypedResults.Ok();
            }).RequireAuthorization();

            routeGroup.MapPost("/updatePassword", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] UpdatePasswordFormDTO formDTO, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                var userManager = sp.GetRequiredService<UserManager<TUser>>();

                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException($"{nameof(MapCustomIdentityApi)} requires a user store with email support.");
                }

                var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                var emailStore = (IUserEmailStore<TUser>)userStore;

                var existingUser = await userStore.FindByIdAsync(formDTO.Id, CancellationToken.None);

                if (existingUser is null)
                    throw new NotFoundException($"user with id '{formDTO.Id}' does not exist");

                var result = await userManager.ChangePasswordAsync(existingUser, formDTO.Password, formDTO.NewPassword);

                if (!result.Succeeded)
                {
                    return CreateValidationProblem(result);
                }
                
                return TypedResults.Ok();
            }).RequireAuthorization();


            routeGroup.MapPost("/getall", async Task<Results<Ok<SimpleContactDTO[]>, ValidationProblem>>
                ([FromServices] IServiceProvider sp) =>
            {
                var userService = sp.GetRequiredService<IUserService>();
                var users = await userService.GetAll();
                var simpleContacts = users.Select(user => user.ToSimpleContactDTO()).ToArray();
                return TypedResults.Ok(simpleContacts);
            });

            routeGroup.MapPost("/getById", async Task<Results<Ok<ContactDTO>, ValidationProblem>>
                ([FromBody] UserFormDTO form,[FromServices] IServiceProvider sp) =>
            {
                var userService = sp.GetRequiredService<IUserService>();
                var user = await userService.GetById(form.Id);
                return TypedResults.Ok(user.ToContactDTO());
            }).RequireAuthorization();

            routeGroup.MapPost("/delete", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] UserFormDTO form, [FromServices] IServiceProvider sp) =>
            {
                var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                var user = await userStore.FindByIdAsync(form.Id, CancellationToken.None);
                if (user is null)
                    throw new NotFoundException($"user with id '{form.Id}' does not exist");

                await userStore.DeleteAsync(user, CancellationToken.None);
                return TypedResults.Ok();
            }).RequireAuthorization();

            return new IdentityEndpointsConventionBuilder(routeGroup);
        }

        private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
            TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
            });

        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            // We expect a single error code and description in the normal case.
            // This could be golfed with GroupBy and ToDictionary, but perf! :P
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }

        private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
            where TUser : class
        {
            return new()
            {
                Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
                IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
            };
        }

        // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
        private sealed class IdentityEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
        {
            private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

            public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
            public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromBodyAttribute : Attribute, IFromBodyMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromServicesAttribute : Attribute, IFromServiceMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromQueryAttribute : Attribute, IFromQueryMetadata
        {
            public string? Name => null;
        }
    }
}
