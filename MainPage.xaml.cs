using Microsoft.Phone.Controls;
using EZTVMetro.Models;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Controls;
using System;
using Microsoft.Phone.Shell;
using System.Net;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.GamerServices;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel;

namespace EZTVMetro
{

    public partial class MainPage : PhoneApplicationPage
    {

        private DispatcherTimer dt;
        private StackPanel sp;
        private ProgressIndicator progress;
        private enum  PIVOT_ITEM { Listing, Downloads, Shows, Clients, Add};
        private enum APPBAR_ICON {Refresh, Search }
        private bool _useAlternate;


        public MainPage(){
            InitializeComponent();
            listings.ItemsSource = App.LatestShows;
            shows.ItemsSource = App.Shows;
            downloads.ItemsSource = App.Torrents;
            clientAddClientType.ItemsSource = EnumHelper<Enums.ClientTypes>.GetNames();            

            App.LatestShows.DataLoading += (s, e) => { showProgress("Loading data, please wait..."); } ;
            App.LatestShows.DataLoaded += (s, e) => { hideProgress(); };
            App.LatestShows.DataError += (s, e) => {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    MessageBox.Show("No result came from your query.");
                    hideProgress();
                });
            };
            App.LatestShows.DataSearching += (s, e) => {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    showProgress("Searching, please wait...");
                });
            };

            App.Shows.DataLoading += (s, e) => { showProgress("Loading data, please wait..."); };
            App.Shows.DataLoaded += (s, e) => { hideProgress(); };

            App.Torrents.DataLoading += (s, e) => { showProgress("Loading data, please wait..."); };
            App.Torrents.DataLoaded += (s, e) => { hideProgress(); };

            pivot.LoadingPivotItem += new EventHandler<PivotItemEventArgs>(pivot_DataLoading);
            pivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(pivot_DataLoaded);

            SystemTray.SetOpacity(this,0.0);
            progress = new ProgressIndicator {
                IsVisible = false,
                IsIndeterminate = true,
                Text = "Loading, please wait...",
            };
            SystemTray.SetProgressIndicator(this, progress);
        }



        void hideProgress() {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                SystemTray.SetOpacity(this, 0);
                progress.IsVisible = false;
            });
        }
        void showProgress() {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                SystemTray.SetOpacity(this, 1);
                progress.IsVisible = true;
            });
        }

        void showProgress(string text) {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                SystemTray.SetOpacity(this, 1);
                progress.IsVisible = true;
                progress.Text = text;
            });
        }


        /* event handlers */

        void pivot_DataLoading(object sender, EventArgs e) {
            showProgress("Working...");
        }
        void pivot_DataLoaded(object sender, EventArgs e) {
            hideProgress();
        }


        void latestShows_DataLoading(object sender, EventArgs e) {
            showProgress("Loading data, please wait...");
        }

        void latestShows_DataError(object sender, EventArgs e) {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                MessageBox.Show("No result came from your query.");
                hideProgress();
            });
        }

        void latestShows_DataSearching(object sender, EventArgs e) {
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                showProgress("Searching, please wait...");
            });
        }
        void latestShows_DataLoaded(object sender, EventArgs e) {
            hideProgress();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e){
            //loadListings();
            //loadShows();
            //loadDownloads();
        }

        private void listings_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // if an item is selected
            if (listings.SelectedIndex != -1) {
                Listing curList = (Listing)listings.SelectedItem;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs args) {
            // make sure no item is highlighted in the list of items in the listbox
            listings.SelectedIndex = -1;
            listings.SelectedItem = null;
        }

        private void AppBarRefresh_Click(object sender, EventArgs e) {
            showProgress("Working, please wait...");
            switch(pivot.SelectedIndex){
                case (int)PIVOT_ITEM.Listing:
                    App.LatestShows.LoadData(true);
                    break;
                case (int)PIVOT_ITEM.Shows:
                    App.Shows.LoadData(true);
                    break;
            }

        }
        
        /* event handlers */


        /* Context Menu Section */
        private void buildMenu() {
            switch (pivot.SelectedIndex)
            {
                case (int)PIVOT_ITEM.Listing:
                    if (sp == null) return;
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuTwitter = new MenuItem() { Header = "Share", Tag = "Share" };
                    menuTwitter.Click += twitter_Click;

                    contextMenu.Items.Add(menuTwitter);
                    var query = (
                        from c in App.db.Clients
                        select c
                    );

                    if (query.Count() > 0)
                    {

                        List<TorrentClient> list = query.ToList<TorrentClient>();
                        foreach (TorrentClient c in list)
                        {
                            MenuItem m = new MenuItem()
                            {
                                Header = "Send to " + c.Name,
                                Tag = c.Id
                            };
                            m.Click += new RoutedEventHandler(m_Click);
                            contextMenu.Items.Add(m);
                        }
                    }
                    ContextMenuService.SetContextMenu(sp, contextMenu);
                    break;
            }
 
        }

        void m_Click(object sender, RoutedEventArgs e) {
            Listing item = (sender as MenuItem).DataContext as Listing;
        }


        private string getClientType(object val) {
            return Enum.GetName(typeof(Enums.ClientTypes), val);
        }

        private void BuildAppTray() {
            String res = "";
            switch (pivot.SelectedIndex) {
                case (int)PIVOT_ITEM.Listing:
                    res = "appbarListings";
                    break;
                case (int)PIVOT_ITEM.Shows:
                    res = "appbarShows";
                    break;
                case (int)PIVOT_ITEM.Downloads:
                    res = "appbarDownloads";
                    break;
                case (int)PIVOT_ITEM.Add:
                    res = "appbarAdd";
                    break;
            }
            ApplicationBar = Resources[res] as ApplicationBar;
            if (ApplicationBar != null)
            {
                ApplicationBar.BackgroundColor = Color.FromArgb(255, 64, 195, 244);
                ApplicationBar.Opacity = 0.75;
            }
        }

        private void rebindClients() {
            List<TorrentClient> l = (from d in App.db.Clients select d).ToList();
            clients.ItemsSource = l;
        }


        private void loadListings()
        {
            BackgroundWorker listingWorker = new BackgroundWorker();
            listingWorker.WorkerReportsProgress = false;
            listingWorker.WorkerSupportsCancellation = false;
            listingWorker.DoWork += (src, evt) =>
            {
                if (!App.LatestShows.IsDataLoaded && !App.LatestShows.IsDataLoading) App.LatestShows.LoadData(false);
            };

            listingWorker.RunWorkerAsync();
        }

        private void loadDownloads()
        {
            BackgroundWorker downloadWorker = new BackgroundWorker();
            downloadWorker.WorkerReportsProgress = false;
            downloadWorker.WorkerSupportsCancellation = false;
            downloadWorker.DoWork += (src, ev) =>
            {
                App.Torrents.Refresh();
            };
            downloadWorker.RunWorkerAsync();
        }

        private void loadShows()
        {
            BackgroundWorker showWorker = new BackgroundWorker();
            showWorker.WorkerReportsProgress = false;
            showWorker.WorkerSupportsCancellation = false;
            showWorker.DoWork += (src, ev) =>
            {
                if (!App.Shows.IsDataLoaded && !App.Shows.IsDataLoading) App.Shows.LoadData(false);
            };
            showWorker.RunWorkerAsync();
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            switch(pivot.SelectedIndex){
                case (int)PIVOT_ITEM.Listing:
                    if (dt != null) dt.Stop();
                    loadListings();
                    break;
                case (int)PIVOT_ITEM.Downloads:
                    App.Torrents.LoadData();
                    if (dt == null) dt = new DispatcherTimer();
                    dt.Interval = new TimeSpan(0, 0, 0, 0, 15000);
                    dt.Tick += (dts, dte) => {
                        App.Torrents.Refresh();
                    };
                    //if(!dt.IsEnabled) dt.Start();
                    break;
                case (int)PIVOT_ITEM.Shows:
                    if (dt != null) dt.Stop();
                    loadShows();
                    break;
                case (int)PIVOT_ITEM.Add:
                    if (dt != null) dt.Stop();
                    break;
                case (int) PIVOT_ITEM.Clients:
                    if (dt != null) dt.Stop();
                    rebindClients();
                    break;
            }
            BuildAppTray();
            buildMenu();
        }


        private void AppBarSearch_Click(object sender, EventArgs e) {
            Guide.BeginShowKeyboardInput(Microsoft.Xna.Framework.PlayerIndex.One, "Search TV Shows", 
                "Type the name of the TV show you want to search.", "", new AsyncCallback(loadSearch), null);
        }

        private void loadSearch(IAsyncResult res) {
            string data = Guide.EndShowKeyboardInput(res);
            if (data == null) return;
            if (data.Length <= 1) return;
            App.LatestShows.queryListing(data);
        }

        private void AppBarSaveClient_Click(object sender, EventArgs e) {
            if(client_add_name.Text.Trim().Length <= 0){

            }else if(client_add_host.Text.Trim().Length <= 0){

            }else if(client_add_port.Text.Trim().Length <= 0){

            }else if(client_add_username.Text.Trim().Length <= 0){

            } else if (client_add_password.Text.Trim().Length <= 0) {

            }else{ //save
                //TorrentClientType tt = (clientAddClientType.SelectedItem) as TorrentClientType;
                TorrentClient tc = new TorrentClient {
                    Name = client_add_name.Text.Trim(),
                    ClientType = clientAddClientType.SelectedIndex,
                    Host = client_add_host.Text.Trim(),
                    Port = Convert.ToInt16(client_add_port.Text.Trim()),
                    Username = client_add_username.Text.Trim(),
                    Password = client_add_password.Text.Trim(),
                    Auth = (bool) client_add_auth.IsChecked,
                    Ctype = getClientType(clientAddClientType.SelectedIndex)
                };

                App.db.Clients.InsertOnSubmit(tc);
                App.db.SubmitChanges();

                MessageBox.Show("Client Saved.");
            }
        }


        private void twitter_Click(object sender, RoutedEventArgs e) {
            Listing item = (sender as MenuItem).DataContext as Listing;
            var wbt = new Microsoft.Phone.Tasks.WebBrowserTask();
            try{
                wbt.Uri = new Uri("http://twitter.com/share?text=" + HttpUtility.UrlEncode(item.Title) + " @eztvdroid&url=" + HttpUtility.UrlEncode(item.Showlink));
                wbt.Show();
            }catch(Exception){
                MessageBox.Show("Unable to share this item at this time.");
            }
        }

        private void client_DeleteClick(object sender, RoutedEventArgs e) {
            TorrentClient client = ((sender as MenuItem).DataContext) as TorrentClient;

            App.db.Clients.DeleteOnSubmit(client);
            App.db.SubmitChanges();
            rebindClients();
        }

        private void show_filter_Click(object sender, RoutedEventArgs e) {
            if (filter.Text.Length > 0)
            {
                App.Shows.setFilter(filter.Text);
                App.Shows.filterSet();
            }
            else
            {
                App.Shows.LoadData(true);
            }
        }

        private void stack_listing_Loaded(object sender, RoutedEventArgs e) {

            StackPanel ItemRef = sender as StackPanel;      // get the reference to the control
            SolidColorBrush brush1 = new SolidColorBrush(Color.FromArgb(255, 238, 241, 245));      //base colour
            SolidColorBrush brush2 = new SolidColorBrush(Color.FromArgb(255, 192, 232, 253));  //alternate colour

            if (_useAlternate)
                ItemRef.Background = brush1;
            else
                ItemRef.Background = brush2;

            _useAlternate = !_useAlternate;

            sp = (StackPanel)sender;
            buildMenu();
        }


    }
}