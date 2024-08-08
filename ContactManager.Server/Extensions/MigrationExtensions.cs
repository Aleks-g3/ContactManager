using ContactManager.Server.Contexts;
using ContactManager.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Server.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationContext contactContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            contactContext.Database.Migrate();

            if(!contactContext.Users.Any())
            {
                var task = AddTechnicianUser(scope, contactContext);
                Task.WaitAll(task);
            }
        }

        private async static Task AddTechnicianUser(IServiceScope scope, ApplicationContext contactContext)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Contact>>();
            var user = new Contact
            {
                Name = "technican",
                Surname = "technican",
                PhoneNumber = "1234567891",
                Category = Category.Private,
                BirthDate = DateTime.Now,
                Email = "technican@test.com",
            };

            var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<Contact>>();
            var emailStore = (IUserEmailStore<Contact>)userStore;
            var email = user.Email;

            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, "zaq1@WSX");
        }
    }
}
