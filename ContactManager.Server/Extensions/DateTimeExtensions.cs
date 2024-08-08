namespace ContactManager.Server.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertToDateTime(this string dateTime) => DateTime.Parse(dateTime);
        public static string ConvertToString(this DateTime dateTime) => dateTime.ToString("MM/dd/yyyy");
    }
}
