using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System;
namespace EZTVMetro.Models
{
    public class Listings : ObservableCollection<Listing> {

        const string URL_SEARCH = "http://eztvdroid.org:80/scraper.php?method=search&mode=json&query=";
        const string URL_LIST = "http://eztvdroid.org/scraper.php?method=latest&mode=json";

        public event EventHandler DataLoaded;
        public event EventHandler DataLoading;
        public event EventHandler DataError;
        public event EventHandler DataSearching;
        private bool isDataLoading;

        protected virtual void OnDataLoaded(EventArgs e) {
            if (DataLoaded != null)
            {
                DataLoaded(this, e);
                isDataLoading = false;
            }
        }
        protected virtual void OnDataLoading(EventArgs e) {
            if (DataLoading != null)
            {
                DataLoading(this, e);
                isDataLoading = true;
            }
        }

        protected virtual void OnDataError(EventArgs e) {
            if (DataError != null)
                DataError(this, e);
        }
        protected virtual void OnDataSearching(EventArgs e) {
            if (DataSearching != null)
                DataSearching(this, e);
        }
        public bool IsDataLoaded {
            get { return App.LatestShows.Count > 0; }
        }

        public bool IsDataLoading
        {
            get { return isDataLoading; }
        }

        public Listings() { }

        public void LoadData(bool force) {
            OnDataLoading(null);
            if (PhoneApplicationService.Current.State.ContainsKey("jsontxt") && !force) {
                parseJSONString(PhoneApplicationService.Current.State["jsontxt"].ToString());
            } else {
                string rnddata = force?"&ref="+DateTime.Now.Ticks.ToString():""; //force retrieve new content
                string urLendpoint = URL_LIST+rnddata;
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += (s, e) => {
                    OnDataLoading(null);
                };
                webClient.DownloadStringCompleted += (s, e) => {
                    if (e.Error != null) {
                        Deployment.Current.Dispatcher.BeginInvoke(() => {
                            MessageBox.Show(e.Error.Message);
                        });
                    } else {
                        populateData(e.Result);
                    }
                };
                webClient.DownloadStringAsync(new System.Uri(urLendpoint));
            }
            
        }


        public void queryListing(string query) {
            OnDataSearching(null);
            string urLendpoint = URL_SEARCH + HttpUtility.UrlEncode(query);
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += (s, e) => {
                OnDataLoading(null);
            };
            webClient.DownloadStringCompleted += (s, e) => {
                if (e.Error != null) {
                    Deployment.Current.Dispatcher.BeginInvoke(() => {
                        MessageBox.Show(e.Error.Message);
                    });
                } else {
                    populateData(e.Result);
                }
            };
            webClient.DownloadStringAsync(new System.Uri(urLendpoint));
        }


        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            OnDataLoading(null);
        }
        private void parseJSONString(string json) {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                try {
                    JObject o = JObject.Parse(json);
                    JArray latest = (JArray)o["latest"];
                    App.LatestShows.ClearItems();
                    for (int i = 0; i < latest.Count; i++) {
                        Listing item = new Listing(latest[i]["title"].ToString(), Utility.bytesToFancyString((double)latest[i]["size"]),
                            latest[i]["link"].ToString(), Utility.stringToFancyDate(latest[i]["pubdate"].ToString()), latest[i]["showlink"].ToString());
                        App.LatestShows.Add(item);
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    OnDataError(null);
                    return;
                }
                OnDataLoaded(null);
            });
        }

        private void populateData(string data) {
            PhoneApplicationService.Current.State["jsontxt"] = data;
            parseJSONString(data);
        }

    }
}
