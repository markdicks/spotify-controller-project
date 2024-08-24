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
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    var request = context.Request;
                    var response = context.Response;

                    var url = request.Url.ToString();
                    Console.WriteLine($"Received URL: {url}"); // Debug log

                    // Serve the HTML page with JavaScript to extract the access token
                    if (url.EndsWith("/"))
                    {
                        string htmlResponse = @"
                    <html>
                    <head><title>Twitch Login</title></head>
                    <body>
                        <script type='text/javascript'>
                            // Extract the access token from the URL fragment
                            var fragment = window.location.hash.substring(1);
                            var params = new URLSearchParams(fragment);
                            var accessToken = params.get('access_token');

                            if (accessToken) {
                                // Send the access token to the server
                                var xhr = new XMLHttpRequest();
                                xhr.open('POST', '/', true);
                                xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                xhr.send('access_token=' + accessToken);

                                // Inform the user
                                document.write('Login successful! You can close this window.');
                            } else {
                                document.write('Error: Access token not found.');
                            }
                        </script>
                    </body>
                    </html>";

                        byte[] buffer = Encoding.UTF8.GetBytes(htmlResponse);
                        response.ContentType = "text/html";
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.ContentLength64 = buffer.Length;
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        response.OutputStream.Close();
                    }
                    else if (request.HttpMethod == "POST")
                    {
                        // Handle the POST request containing the access token
                        using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            string postData = await reader.ReadToEndAsync();
                            Console.WriteLine($"POST data received: {postData}"); // Debug log

                            var accessToken = WebUtility.UrlDecode(postData.Replace("access_token=", ""));
                            Console.WriteLine($"Access token extracted: {accessToken}"); // Debug log

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                _onAccessTokenReceived?.Invoke(accessToken);

                                // Respond to the client
                                byte[] buffer = Encoding.UTF8.GetBytes("Access token received. You can close this window.");
                                response.StatusCode = (int)HttpStatusCode.OK;
                                response.ContentLength64 = buffer.Length;
                                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                byte[] buffer = Encoding.UTF8.GetBytes("Error: Access token not found.");
                                response.StatusCode = (int)HttpStatusCode.BadRequest;
                                response.ContentLength64 = buffer.Length;
                                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                            }
                        }

                        response.OutputStream.Close();
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        byte[] buffer = Encoding.UTF8.GetBytes("Error: Invalid request.");
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                        response.OutputStream.Close();
                    }
                }
                catch (HttpListenerException ex)
                {
                    Console.WriteLine($"HttpListenerException: {ex.Message}");
                    break; // Gracefully exit the loop if the listener is stopped.
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        public void Stop()
        {
            _listener?.Stop();
        }
    }
}
