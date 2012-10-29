using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;
namespace EZTVMetro.Models {
    public class Shows : ObservableCollection<Show> {

        IQueryable<TShow> st;
        private string filter = "";
        public event EventHandler DataLoaded;
        public event EventHandler DataLoading;
        public event EventHandler DataInitialize;
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

        protected virtual void OnDataInitializing(EventArgs e) {
            if (DataInitialize != null)
                DataInitialize(this, e);
        }

        public bool IsDataLoaded {
            get { return App.Shows.Count > 0; }
        }

        public bool IsDataLoading
        {
            get { return isDataLoading; }
        }


        public Shows() {

        }

        public void LoadData(bool force) {
            if (!haveRecords() || force) {
                
                string urLendpoint = "http://eztvdroid.org/scraper.php?method=shows&mode=json" + "&ref=" + DateTime.Now.Ticks.ToString();
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                webClient.DownloadStringAsync(new System.Uri(urLendpoint));
                OnDataInitializing(null);
            } else {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.RunWorkerAsync();
            }
        }

        private bool haveRecords() {
            try {
                IQueryable<TShow> chk = (from d in App.db.Shows select d).Take(1);
                return (chk.Count() > 0);
            }catch(NullReferenceException){
                return false;
            }
        }

        public void filterSet() {
            OnDataLoading(null);
            if (this.filter == "") {
                st = (from d in App.db.Shows select d);
            } else {
                st = (from d in App.db.Shows where d.Title.Contains(filter) || d.Status.Contains(filter) select d);
            }
            App.Shows.ClearItems();
            foreach (TShow s in st) {
                App.Shows.Add(new Show(s.Title.Trim(), s.Status.Trim()));
            }

            OnDataLoaded(null);
        }



        private void worker_RunWorkerCompleted(object sender,
                               RunWorkerCompletedEventArgs e) {
                                   OnDataLoaded(null);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e) {
            //this.force = force;
            if (this.filter == "") {
                st = (from d in App.db.Shows select d);
            } else {
                st = (from d in App.db.Shows where d.Title.StartsWith(filter) || d.Status.StartsWith(filter) select d);
            }
            if (st.Count() > 0) {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    App.Shows.ClearItems();
                    foreach (TShow s in st) {
                        App.Shows.Add(new Show(s.Title.Trim(), s.Status.Trim()));
                    }
                });

            }
        }


        public void setFilter(String filter) {
            this.filter = filter;
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            OnDataLoading(null);
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
            if (e.Error != null) {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    MessageBox.Show(e.Error.Message);
                });
            } else {
                
                JObject o = JObject.Parse(e.Result);
                JArray shows = (JArray)o["shows"];
                PhoneApplicationService.Current.State["show_json"] = e.Result;
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    App.Shows.ClearItems();
                    App.db.Shows.DeleteAllOnSubmit(from d in App.db.Shows select d);
                    for (int i = 0; i < shows.Count; i++) {
                        Show item = new Show(shows[i]["title"].ToString(), shows[i]["status"].ToString());
                        App.Shows.Add(item);
                        TShow s = new TShow {
                            Title = shows[i]["title"].ToString(),
                            Status = shows[i]["status"].ToString()
                        };
                        App.db.Shows.InsertOnSubmit(s);
                    }

                    App.db.SubmitChanges();
                    OnDataLoaded(null);
                });
            }

        }



    }
}
