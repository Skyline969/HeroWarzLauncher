using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace HeroWarzLauncher.Koggames
{
    internal class KoggamesAPI
    {
        internal enum ExceptionType
        {
            [Description("Unknown")]
            None,
            [Description("KoggamesAPI.Initialize()")]
            Initialization,
            [Description("KoggamesAPI.Login()")]
            Login,
            [Description("KoggamesAPI.LoginHeroWarz()")]
            LoginHeroWarz,
            [Description("KoggamesAPI.GetHeroWarzCmd()")]
            HeroWarzCmd,
        }
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";

        internal delegate void OnErrorHandler(Exception exception, ExceptionType type);
        internal event OnErrorHandler OnError;
        internal bool IsOnErrorSet
        {
            get
            {
                return OnError != null;
            }
        }

        internal delegate void OnInitializationHandler(bool success);
        internal event OnInitializationHandler OnInitialization;
        internal bool IsOnInitializationSet
        {
            get
            {
                return OnInitialization != null;
            }
        }

        internal delegate void OnLoginHandler(bool success);
        internal event OnLoginHandler OnLogin;
        internal bool IsOnLoginSet
        {
            get
            {
                return OnLogin != null;
            }
        }

        internal delegate void OnLoginHeroWarzHandler(bool success);
        internal event OnLoginHeroWarzHandler OnLoginHeroWarz;
        internal bool IsOnLoginHeroWarzSet
        {
            get
            {
                return OnLoginHeroWarz != null;
            }
        }

        internal delegate void OnHeroWarzCmdHandler(string commandLine);
        internal event OnHeroWarzCmdHandler OnHeroWarzCmd;
        internal bool IsOnHeroWarzCmdSet
        {
            get
            {
                return OnHeroWarzCmd != null;
            }
        }

        private CookieContainer _cookieContainer = null;
        private CookieContainer CookieContainer
        {
            get
            {
                return _cookieContainer;
            }

            set
            {
                _cookieContainer = value;
            }
        }

        private string _gameTokenRequestUrl = string.Empty;
        private string GameTokenRequestUrl
        {
            get
            {
                return _gameTokenRequestUrl;
            }

            set
            {
                _gameTokenRequestUrl = value;
            }
        }

        internal void Initialize()
        {
            Initialize(true);
        }
        private void Initialize(bool newThread)
        {
            if (newThread)
            {
                new Thread(() =>
                {
                    Initialize(false);
                }).Start();
            }
            else
            {
                try
                {
                    CookieContainer = new CookieContainer();
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://myaccount.koggames.com/");
                    webRequest.Method = "GET";
                    webRequest.UserAgent = UserAgent;
                    webRequest.CookieContainer = CookieContainer;
                    webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    webRequest.Referer = "https://myaccount.koggames.com/";
                    webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse()) { }
                    if (IsOnInitializationSet)
                        OnInitialization(true);
                }
                catch (Exception exception)
                {
                    if (IsOnErrorSet)
                        OnError(exception, ExceptionType.Initialization);
                    if (IsOnInitializationSet)
                        OnInitialization(false);
                }
            }
        }

        internal void Login(string username, string password)
        {
            Login(true, username, password);
        }
        private void Login(bool newThread, string username, string password)
        {
            if (newThread)
            {
                new Thread(() =>
                {
                    Login(false, username, password);
                }).Start();
            }
            else
            {
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://myaccount.koggames.com/Home/Login");
                    webRequest.Method = "POST";
                    webRequest.UserAgent = UserAgent;
                    webRequest.CookieContainer = CookieContainer;
                    webRequest.Accept = "*/*";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.Referer = "https://myaccount.koggames.com/";
                    webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                    webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    webRequest.KeepAlive = true;

                    string postString = string.Format("UserName={0}&Password={1}&Captcha=", Uri.EscapeDataString(username), Uri.EscapeDataString(password));
                    byte[] postBuffer = Encoding.ASCII.GetBytes(postString);
                    webRequest.ContentLength = postBuffer.Length;

                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        stream.Write(postBuffer, 0, postBuffer.Length);
                    }

                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            string response = streamReader.ReadToEnd();
                            if (!response.Contains("\"Success\":true"))
                                throw new Exception(string.Format("Login failed. Server response:\n{0}", response));
                            else if (IsOnLoginSet)
                                OnLogin(true);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (IsOnErrorSet)
                        OnError(exception, ExceptionType.Login);
                    if (IsOnLoginSet)
                        OnLogin(false);
                }
            }
        }

        internal void LoginHeroWarz()
        {
            LoginHeroWarz(true);
        }
        private void LoginHeroWarz(bool newThread)
        {
            if (newThread)
            {
                new Thread(() =>
                {
                    LoginHeroWarz(false);
                }).Start();
            }
            else
            {
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://api.koggames.com/auth/HWServerSelect.aspx/HWLogIN");
                    webRequest.Method = "POST";
                    webRequest.UserAgent = UserAgent;
                    webRequest.CookieContainer = CookieContainer;
                    webRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                    webRequest.ContentType = "application/json; charset=UTF-8";
                    webRequest.Referer = "https://api.koggames.com/auth/HWServerselect.aspx";
                    webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                    webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    webRequest.KeepAlive = true;

                    string postString = "{SKey:1}";
                    byte[] postBuffer = Encoding.ASCII.GetBytes(postString);
                    webRequest.ContentLength = postBuffer.Length;

                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        stream.Write(postBuffer, 0, postBuffer.Length);
                    }

                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            string response = streamReader.ReadToEnd();
                            if (!response.Contains("\"d\":\""))
                                throw new Exception(string.Format("Login failed. Server response:\n{0}", response));
                            GameTokenRequestUrl = response.Split(new string[] { "\"d\":\"" }, StringSplitOptions.None)[1];
                            GameTokenRequestUrl = GameTokenRequestUrl.Split(new string[] { "\"}" }, StringSplitOptions.None)[0];
                        }
                    }

                    if (string.IsNullOrEmpty(GameTokenRequestUrl))
                        throw new Exception("Login failed. Could not get game login pre-token.");
                    else if (IsOnLoginHeroWarzSet)
                        OnLoginHeroWarz(true);
                }
                catch (Exception exception)
                {
                    if (IsOnErrorSet)
                        OnError(exception, ExceptionType.LoginHeroWarz);
                    if (IsOnLoginHeroWarzSet)
                        OnLoginHeroWarz(false);
                }
            }
        }

        internal void GetHeroWarzCmd()
        {
            GetHeroWarzCmd(true);
        }
        private void GetHeroWarzCmd(bool newThread)
        {
            if (newThread)
            {
                new Thread(() =>
                {
                    GetHeroWarzCmd(false);
                }).Start();
            }
            else
            {
                try
                {
                    string token = string.Empty;
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(GameTokenRequestUrl);
                    webRequest.Method = "GET";
                    webRequest.UserAgent = UserAgent;
                    webRequest.CookieContainer = CookieContainer;
                    webRequest.Accept = "*/*";
                    webRequest.Referer = "https://api.koggames.com/auth/HWServerselect.aspx";
                    webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            string response = streamReader.ReadToEnd();
                            if (!response.Contains("\"succeed\":true") || !response.Contains("\"token\":\""))
                                throw new Exception(string.Format("Could not get the login token. Server response:\n{0}", response));
                            token = response.Split(new string[] { "\"token\":\"" }, StringSplitOptions.None)[1];
                            token = token.Split(new string[] { "\"}" }, StringSplitOptions.None)[0];
                        }
                    }

                    if (string.IsNullOrEmpty(token))
                        throw new Exception("Login failed. Could not get game login token.");

                    if (IsOnHeroWarzCmdSet)
                        OnHeroWarzCmd(string.Format("-autologin -t1 {0} -sk 1 -cc Herowarz", token));
                }
                catch (Exception exception)
                {
                    if (IsOnErrorSet)
                        OnError(exception, ExceptionType.HeroWarzCmd);
                    if (IsOnHeroWarzCmdSet)
                        OnHeroWarzCmd(string.Empty);
                }
            }
        }
    }
}
