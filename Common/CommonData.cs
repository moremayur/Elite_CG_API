namespace Elite_CG_API.Common
{
    public class CommonData
    {
        private static IConfiguration AppSetting = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public static string Base_URL = AppSetting["CommonFields:Base_URL"];
        public static string Repo_Owner = AppSetting["CommonFields:Repo_Owner"];
        public static string Repo_Name = AppSetting["CommonFields:Repo_Name"];
        public static string Token = AppSetting["CommonFields:Token"];

        public HttpClient Client { get; set; }

        public CommonData() {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + CommonData.Token);
            Client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            Client.DefaultRequestHeaders.Add("User-Agent", CommonData.Repo_Name);
        }

    }
}
