using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Configuration;
using NHttp;

namespace spotify_controller_project
{
    public partial class TwitchApp : Form
    {
        private HttpServer WebServer;
        private string CachedOwnerOfChannelAccessToken = "needaccesstoken";
        private readonly string RedirectUrl = "http://localhost";
        private readonly string ClientId = Properties.Settings.Default.clientid;
        private readonly string ClientSecret = Properties.Settings.Default.clientsecret;
        private readonly List<string> Scopes = new List<string>
        {
            "user:edit", "chat:read", "chat:edit", "channel:moderate", "whispers:read", "whispers:edit", "bits:read",
            "channel:read:subscriptions", "user:read:email", "user:read:subscriptions"
        };

        public TwitchApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Connecting to twitch chat...");
            InitializeWebServer();

            var authUrl = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri={RedirectUrl}&scope={String.Join("+",Scopes)}";
            System.Diagnostics.Process.Start(authUrl);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        void InitializeWebServer()
        {
            // Creating server to optain OAuth key automatically
            // May add option to do this manually for people concerned of security
            Webserver = HttpServer();
            Webserver.EndPoint = new IPEndPoint(IPAddress.Loopback, 80);

            // Setup callback that we wanna run when req made
            WebServer.RequestRecieved += async (s, e) =>
            {
                using (var writer = new StreamWriter(e.Response.OutputStream))
                {
                    if (e.Request.QueryString.AllKeys.Any("code".Contains))
                    {
                        var code = e.Request.QueryString.AllKeys["code"];
                        var ownerOfChannelAccessAndRefresh = await getAccessAndRefreshTokens(code);
                        CachedOwnerOfChannelAccessToken = ownerOfChannelAccessAndRefresh.Item1;
                        // SetNameAndIdByOauthedUser(CachedOwnerOfChannelAccessToken).Wait();
                        // InitializeOwnerOfChannelConnection(TwitchChannelName, CachedOwnerOfChannelAccessToken);
                        // InitializeTwitchApi(CachedOwnerOfChannelAccessToken);
                    }
                }
            };

            //Start the server
            WebServer.Start();
            Console.WriteLine($"Web server started at {WebServer.EndPoint}");
        }


        async Task<Tuple<string, string>> getAccessAndRefreshTokens(string code)
        {
            HttpClient = new HttpClient();
            var values = new Dictionary<string, string>
            {
                {"client_id", ClientId},
                {"client_secret", ClientSecret},
                {"code", code},
                {"grant type", "authorization_code"},
                {"redirect_url", RedirectUrl}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://id.twitch.tv/oauth2/token", content);
            response.EnsureSuccessStatusCode();         //not to sure if this will affect the code or not !check
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JSObject.Parse(responseString);

            // return refresh and access token so the access is not shortlived
            return new Tuple<string, string>(json["access_token"].ToString(), json["refresh_token"].ToString());
        }


    }
}
