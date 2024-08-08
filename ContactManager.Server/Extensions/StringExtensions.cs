namespace ContactManager.Server.Extensions
{
    public static class StringExtensions
    {
        public static String? ConvertToString(this Enum? eff)
        {
            return eff is not null ? Enum.GetName(eff.GetType(), eff) : null;
        }

        public static EnumType? ConvertToEnum<EnumType>(this String? enumValue) where EnumType : struct
        {
            EnumType? status = default;

            if (Enum.TryParse<EnumType>(enumValue, out var parsedStatus))
            {
                status = parsedStatus;
            }

            return status;
        }
    }
}
