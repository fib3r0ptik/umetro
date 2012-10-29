using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using EZTVMetro.Models;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace EZTVMetro.Handlers {
    public class UTorrentHandler {
        private string _host;
        private string _username;
        private string _password;
        private int _port;
        private bool _useAuth;
        private string _endPoint;
        private string _token;
        private Enums.Actions _action;
        public List<Torrent> torrents;

        private CookieCollection cc;


        const int FIELD_HASH = 0;
        const int FIELD_STATUS = 1;
        const int FIELD_NAME = 2;
        const int FIELD_SIZE = 3;
        const int FIELD_PERCENT = 4;
        const int FIELD_DOWNLOADED = 5;
        const int FIELD_UPLOADED = 6;
        const int FIELD_UPLOAD_SPEED = 8;
        const int FIELD_DOWNLOAD_SPEED = 9;
        const int FIELD_ETA = 10;
        const int FIELD_PEERS_CON = 12;
        const int FIELD_PEERS_ALL = 13;
        const int FIELD_SEEDERS_CON = 14;
        const int FIELD_SEEDERS_ALL = 15;
        const int FIELD_ORDER = 17;
        const int FIELD_REMAINING = 18;

        public event EventHandler DataSending;
        public event EventHandler DataSent;
        public event EventHandler DataReceiving;
        public event EventHandler DataReceived;
        public event EventHandler DataError;
        private event EventHandler TokenRetrieved;
        public event EventHandler DataLoaded;

        protected virtual void OnDataSending(EventArgs e) {
            if (DataSending != null)
                DataSending(this, e);
        }
        protected virtual void OnDataSent(EventArgs e) {
            if (DataSent != null)
                DataSent(this, e);
        }
        protected virtual void OnDataReceiving(EventArgs e) {
            if (DataReceiving != null)
                DataReceiving(this, e);
        }
        protected virtual void OnDataReceived(EventArgs e) {
            if (DataError != null)
                DataError(this, e);
        }

        protected virtual void OnDataError(EventArgs e) {
            if (DataReceived != null)
                DataReceived(this, e);
        }

        protected virtual void OnTokenRetrieved(EventArgs e) {
            if (TokenRetrieved != null)
                TokenRetrieved(this, e);
        }

        protected virtual void OnDataLoaded(EventArgs e) {
            if (DataLoaded != null)
                DataLoaded(this, e);
        }


        public UTorrentHandler() {
            torrents = new List<Torrent>();
 
        }

        public UTorrentHandler setHost(string host) {
            _host = host;
            return this;
        }
        public UTorrentHandler setUserName(string username) {
            _username = username;
            return this;
        }

        public UTorrentHandler setPassword(string password) {
            _password = password;
            return this;
        }

        public UTorrentHandler setPort(int port) {
            _port = port;
            return this;
        }

        public UTorrentHandler setUseAuth(bool auth) {
            _useAuth = auth;
            return this;
        }
        public UTorrentHandler setEndPoint(string endpoint) {
            _endPoint = endpoint;
            return this;
        }

        public UTorrentHandler setAction(Enums.Actions action) {
            _action = action;
            return this;
        }

        public void retrieveToken() {
            if (_token != null || _token == "")
            {
                OnTokenRetrieved(null);
                return;

            }

            var request = (HttpWebRequest)WebRequest.Create(
                            new Uri("http://" + _host + ":" + _port + "/gui/token.html"));
            if (_useAuth) 
                request.Credentials = new NetworkCredential(_username, _password);



            request.BeginGetResponse(r =>
            {
                var httpReq = (HttpWebRequest)r.AsyncState;
                var httpResp = (HttpWebResponse)httpReq.EndGetResponse(r);
                cc = httpResp.Cookies;
                if(cc == null){
                    cc = new CookieCollection();
                    string cookieHeader = httpResp.Headers["Set-Cookie"];
                    if (cookieHeader != null)
                    {
                        string[] items = cookieHeader.Split(';');
                        string[] items2 = items.GetValue(0).ToString().Split('=');

                        cc.Add(new Cookie(items2.GetValue(0).ToString(), items2.GetValue(1).ToString()));
                    }
                }
                if (httpResp.StatusCode == HttpStatusCode.OK)
                {
                    
                    using (var reader = new StreamReader(httpResp.GetResponseStream()))
                    {
                        var response = reader.ReadToEnd();
                        _token = response.Replace("<html><div id='token' style='display:none;'>", "")
                            .Replace("</div></html>","").Trim();

                        

                        OnTokenRetrieved(null);
                        Debug.WriteLine(response);
                    }
                }
                else
                {
                    OnDataError(null);
                }
            }, request);



        }

        private void sendRequest(string hash) {
            string uri = "http://" + _host + ":" + _port + "/gui/";
            uri += "?token=" + _token + "&action=" + _action.ToString().ToLower() + hash;
            //retrieveToken();
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += (s, e) => {
                OnDataSending(null);
            };
            webClient.DownloadStringCompleted += (s, e) => {
                Console.WriteLine(e.Result);
                OnDataReceived(null);
            };
            webClient.DownloadStringAsync(new Uri(uri));
        }


        private void fillTorrents(String data)
        {
            JObject o = JObject.Parse(data);
            JArray jtorrents = (JArray)o["torrents"];
            torrents.Clear();
            for (int i = 0; i < jtorrents.Count; i++)
            {
                JArray sub = (JArray)jtorrents[i];
                Torrent t = new Torrent();
                t.Hash = sub[FIELD_HASH].ToString();
                t.Status = (int)sub[FIELD_STATUS];
                t.Name = sub[FIELD_NAME].ToString();
                t.Size = (long)sub[FIELD_SIZE];
                t.Percent = (int)sub[FIELD_PERCENT];
                t.Downloaded = (long)sub[FIELD_DOWNLOADED];
                t.Uploaded = (long)sub[FIELD_UPLOADED];
                t.UploadSpeed = (int)sub[FIELD_UPLOAD_SPEED];
                t.DownloadSpeed = (int)sub[FIELD_DOWNLOAD_SPEED];
                t.ETA = (int)sub[FIELD_ETA];
                t.PeersConnected = (int)sub[FIELD_PEERS_CON];
                t.PeersAll = (int)sub[FIELD_PEERS_ALL];
                t.SeedersConnected = (int)sub[FIELD_SEEDERS_CON];
                t.SeedersAll = (int)sub[FIELD_SEEDERS_ALL];
                t.Order = (int)sub[FIELD_ORDER];
                t.Remaining = (long)sub[FIELD_REMAINING];
                torrents.Add(t);
            }

            OnDataLoaded(null);
        }

        public void getTorrents() {
            retrieveToken();
            TokenRetrieved += (s , e) => {
                if(_action == Enums.Actions.List){

                    var request = (HttpWebRequest)WebRequest.Create(
                                    new Uri("http://" + _host + ":" + _port + "/gui/?token=" + _token + "&list=1"));
                    request.CookieContainer = new CookieContainer();
                     request.CookieContainer.Add(request.RequestUri, cc);


                    if (_useAuth){
                        request.Credentials = new NetworkCredential(_username, _password);
                    }

                    request.BeginGetResponse(r =>
                    {
                        try
                        {
                            
                            var httpReq = (HttpWebRequest)r.AsyncState;
                            Debug.WriteLine(httpReq.RequestUri.OriginalString);
                            var httpResp = (HttpWebResponse)httpReq.EndGetResponse(r);

                            if (httpResp.StatusCode == HttpStatusCode.OK)
                            {

                                using (var reader = new StreamReader(httpResp.GetResponseStream()))
                                {
                                    var response = reader.ReadToEnd();
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        fillTorrents(response);
                                    });
                                    
                                }
                            }
                            else
                            {
                                Debug.WriteLine(httpResp.StatusCode);
                                OnDataError(null);
                            }

                        }catch(Exception exx){
                            Debug.WriteLine(exx.Message);
                            OnDataError(null);
                        }
                    }, request);




                    /*
                    string uri = "http://" + _host + ":" + _port + "/gui/?token=" + _token + "&list=1";
                    WebClient wc = new WebClient();
                    if(_useAuth){
                        wc.Credentials = new NetworkCredential(_username, _password);
                    }

                    Debug.WriteLine("URI is: " + uri);
                    wc.DownloadStringCompleted += (wcs, wce) => {
                        try {
                            JObject o = JObject.Parse(wce.Result);
                            JArray jtorrents = (JArray)o["torrents"];
                            torrents.Clear();
                            for (int i = 0; i < jtorrents.Count; i++) {
                                JArray sub = (JArray)jtorrents[i];
                                Torrent t = new Torrent();
                                t.Hash = sub[FIELD_HASH].ToString();
                                t.Status = (int)sub[FIELD_STATUS];
                                t.Name = sub[FIELD_NAME].ToString();
                                t.Size = (long)sub[FIELD_SIZE];
                                t.Percent = (int)sub[FIELD_PERCENT];
                                t.Downloaded = (long)sub[FIELD_DOWNLOADED];
                                t.Uploaded = (long)sub[FIELD_UPLOADED];
                                t.UploadSpeed = (int)sub[FIELD_UPLOAD_SPEED];
                                t.DownloadSpeed = (int)sub[FIELD_DOWNLOAD_SPEED];
                                t.ETA = (int)sub[FIELD_ETA];
                                t.PeersConnected = (int)sub[FIELD_PEERS_CON];
                                t.PeersAll = (int)sub[FIELD_PEERS_ALL];
                                t.SeedersConnected = (int)sub[FIELD_SEEDERS_CON];
                                t.SeedersAll = (int)sub[FIELD_SEEDERS_ALL];
                                t.Order = (int)sub[FIELD_ORDER];
                                t.Remaining = (long)sub[FIELD_REMAINING];
                                torrents.Add(t);
                            }

                            OnDataLoaded(null);
                        } catch (Exception ex) {
                            Debug.WriteLine("Exc: " + ex.Message);
                            OnDataError(null);
                            return;
                        }
                    };

                    wc.DownloadStringAsync(new Uri(uri));
                    OnDataSent(null);
                     */
                }
            };

        }

    }
}
