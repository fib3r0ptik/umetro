using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace EZTVMetro.Models {
    public class Torrents : ObservableCollection<Torrent> {

        Handlers.UTorrentHandler ut;

        public event EventHandler DataLoaded;
        public event EventHandler DataLoading;
        public event EventHandler DataError;

        protected virtual void OnDataLoaded(EventArgs e) {
            if (DataLoaded != null)
                DataLoaded(this, e);
        }
        protected virtual void OnDataLoading(EventArgs e) {
            if (DataLoading != null)
                DataLoading(this, e);
        }

        protected virtual void OnDataError(EventArgs e) {
            if (DataError != null)
                DataError(this, e);
        }

        public bool IsDataLoaded {
            get { return App.Torrents.Count > 0; }
        }

        public Torrents() {
            ut = new Handlers.UTorrentHandler();
            ut.setAction(Enums.Actions.List)
              .setHost("127.0.0.1")
              .setPort(1000)
              .setUserName("admin")
              .setPassword("letmein")
              .setUseAuth(true);
        }

        public void Refresh() {
            ut.DataLoaded += (s, e) => {
                App.Torrents.ClearItems();
                List<Torrent> items = ut.torrents;
                for (int i = 0; i < items.Count; i++) {
                    App.Torrents.Add(items[i]);
                }
            };
            ut.getTorrents();
        }

        public void LoadData() {
            Debug.WriteLine("Calling loadData downloads");
            OnDataLoading(null);
            ut.getTorrents();
            ut.DataLoaded += (s,e) => {
                Debug.WriteLine("Calling dataloaded.");
                App.Torrents.ClearItems();
                List<Torrent> items = ut.torrents;
                for (int i = 0; i < items.Count; i++) {
                    App.Torrents.Add(items[i]);
                }
                OnDataLoaded(null);
            };
        }
        

    }
}
