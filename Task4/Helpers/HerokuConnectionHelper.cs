namespace Task4.Helpers
{
    public class HerokuConnectionHelper
    {
        public static string ConvertHerokuConnectionString(string databaseUrl)
        {
            if (string.IsNullOrEmpty(databaseUrl)) {
                throw new ArgumentNullException(nameof(databaseUrl), "URL cannot be empty.");
            }

            var uri = new Uri(databaseUrl);
            var host = uri.Host;
            var port = uri.Port;
            var database = uri.AbsolutePath.Trim('/');
            var userInfo = uri.UserInfo.Split(':');
            var username = userInfo[0];
            var password = userInfo[1];

            return $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode=Require;Trust Server Certificate=true";
        }
    }
}
