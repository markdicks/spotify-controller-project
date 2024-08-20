using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ControllerApplication
{
    public class TwitchCallbackListener
    {
        private readonly HttpListener _listener;
        private readonly string _redirectUri;
        private readonly Action<string> _onAccessTokenReceived;

        public TwitchCallbackListener(string redirectUri, Action<string> onAccessTokenReceived)
        {
            if (string.IsNullOrEmpty(redirectUri))
                throw new ArgumentException("Redirect URI cannot be null or empty.", nameof(redirectUri));

            // Ensure the redirectUri ends with exactly one '/'
            _redirectUri = redirectUri.EndsWith("/") ? redirectUri : redirectUri + "/";
            _onAccessTokenReceived = onAccessTokenReceived;

            _listener = new HttpListener();
            _listener.Prefixes.Add(_redirectUri);
        }

        public void Start()
        {
            _listener.Start();
            Task.Run(() => Listen());
        }

        private async Task Listen()
        {
            while (true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    var request = context.Request;
                    var response = context.Response;

                    var url = request.Url.ToString();
                    Console.WriteLine($"Received URL: {url}"); // Debug log

                    if (url.Contains("access_token"))
                    {
                        // Extract the access token from the URL fragment
                        var fragment = url.Split('#')[1];
                        var parameters = fragment.Split('&');
                        var accessTokenParam = parameters.FirstOrDefault(p => p.StartsWith("access_token="));
                        if (accessTokenParam != null)
                        {
                            var accessToken = accessTokenParam.Split('=')[1];
                            _onAccessTokenReceived?.Invoke(accessToken);

                            response.StatusCode = (int)HttpStatusCode.OK;
                            byte[] buffer = Encoding.UTF8.GetBytes("Login successful! You can close this window.");
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            byte[] buffer = Encoding.UTF8.GetBytes("Error: Access token not found.");
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        byte[] buffer = Encoding.UTF8.GetBytes("Error: Invalid request.");
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }

                    response.OutputStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}"); // Debug log
                }
            }
        }

        public void Stop()
        {
            _listener?.Stop();
        }
    }
}
