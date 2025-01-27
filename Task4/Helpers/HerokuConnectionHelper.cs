namespace Task4.Helpers
{
    public class HerokuConnectionHelper
    {
        public static string ConvertHerokuConnectionString(string databaseUrl)
        {
            if (string.IsNullOrEmpty(databaseUrl))
            {
                databaseUrl = "postgres://u9hqp8p5l4bo2g:p65f86c1c8f44ed264c373f936b06d44ad6ef517d2e560bd5cf270212bffd6148@c7u1tn6bvvsodf.cluster-czz5s0kz4scl.eu-west-1.rds.amazonaws.com:5432/d4crvi0sbtue6s";
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
