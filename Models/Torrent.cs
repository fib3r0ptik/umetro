using System;
using System.ComponentModel;
using System.Windows.Media;

namespace EZTVMetro.Models {
    public class Torrent : INotifyPropertyChanged, IComparable<Torrent> {

        const double KILOBYTE = 1024D;
        const double MEGABYTE = 1024D * KILOBYTE;
        const double GIGABYTE = 1024D * MEGABYTE;
        const double PERCENT_COMPLETED = 1000D;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name = null;
        private string _hash = null;
        private int _status;
        private long _size = 0;
        private int _percent = 0;
        private long _downloaded = 0;
        private long _uploaded = 0;
        private int _download_speed = 0;
        private int _upload_speed = 0;
        private int _seeders_con = 0;
        private int _seeders_all = 0;
        private int _peers_con = 0;
        private int _peers_all = 0;
        private int _order = 0;
        private long _remaining = 0;
        private int _eta = 0;

        public string Name {
            get { return _name; }
            set {
                if (value != _name) {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public string Hash {
            get { return _hash; }
            set {
                if (value != _hash) {
                    _hash = value;
                    NotifyPropertyChanged("Hash");
                }
            }
        }

        public int Status {
            get { return _status; }
            set {
                if (value != _status) {
                    _status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        public long Size {
            get { return _size; }
            set {
                if (value != _size) {
                    _size = value;
                    NotifyPropertyChanged("Size");
                }
            }
        }

        public int Percent {
            get { return _percent; }
            set {
                if (value != _percent) {
                    _percent = value;
                    NotifyPropertyChanged("Percent");
                }
            }
        }

        public long Downloaded {
            get { return _downloaded; }
            set {
                if (value != _downloaded) {
                    _downloaded = value;
                    NotifyPropertyChanged("Downloaded");
                }
            }
        }

        public long Uploaded {
            get { return _uploaded; }
            set {
                if (value != _uploaded) {
                    _uploaded = value;
                    NotifyPropertyChanged("Uploaded");
                }
            }
        }

        public int DownloadSpeed {
            get { return _download_speed; }
            set {
                if (value != _download_speed) {
                    _download_speed = value;
                    NotifyPropertyChanged("DownloadSpeed");
                }
            }
        }

        public int UploadSpeed {
            get { return _upload_speed; }
            set {
                if (value != _upload_speed) {
                    _upload_speed = value;
                    NotifyPropertyChanged("UploadSpeed");
                }
            }
        }

        public int SeedersConnected {
            get { return _seeders_con; }
            set {
                if (value != _seeders_con) {
                    _seeders_con = value;
                    NotifyPropertyChanged("SeedersConnected");
                }
            }
        }

        public int SeedersAll {
            get { return _seeders_all; }
            set {
                if (value != _seeders_all) {
                    _seeders_all = value;
                    NotifyPropertyChanged("SeedersAll");
                }
            }
        }

        public int PeersConnected {
            get { return _peers_con; }
            set {
                if (value != _peers_con) {
                    _peers_con = value;
                    NotifyPropertyChanged("PeersConnected");
                }
            }
        }

        public int PeersAll {
            get { return _peers_all; }
            set {
                if (value != _peers_all) {
                    _peers_all = value;
                    NotifyPropertyChanged("PeersAll");
                }
            }
        }

        public int Order {
            get { return _order; }
            set {
                if (value != _order) {
                    _order = value;
                    NotifyPropertyChanged("Order");
                }
            }
        }

        public long Remaining {
            get { return _remaining; }
            set {
                if (value != _remaining) {
                    _remaining = value;
                    NotifyPropertyChanged("Remaining");
                }
            }
        }

        public int ETA {
            get { return _eta; }
            set {
                if (value != _eta) {
                    _eta = value;
                    NotifyPropertyChanged("ETA");
                }
            }
        }

        public string SizeStr {
            get {
                if (Size / MEGABYTE > KILOBYTE) {
                    return String.Format("{0:###,###,###.##}", Size / GIGABYTE) + " gb";
                } else {
                    return String.Format("{0:###,###,###.##}", Size / MEGABYTE) + " mb";
                }
            }
        }

        public string DownloadedStr {
            get {
                if(Downloaded <= KILOBYTE){
                    return String.Format("{0:###,###,###.##}", Downloaded/KILOBYTE) + " kb";
                }else if (Downloaded / MEGABYTE > KILOBYTE) {
                    return String.Format("{0:###,###,###.##}", Downloaded / GIGABYTE) + " gb";
                } else {
                    return String.Format("{0:###,###,###.##}", Downloaded / MEGABYTE) + " mb";
                }
            }
        }

        public string PercentStr{
             get {
                 return "(" + Math.Round((double)Percent / 10.0, 2) + "%)";
                //return "("+ String.Format("{0:###.##}", Math.Round((double)Percent/10.0,2)) + "%)";
             }
        }


        public string ETAStr {
            get {
                if (Downloaded >= Size) {
                    return " - Completed.";
                } else {
                    return " ETA " + Utility.secToFancyString(ETA);
                }
               
            }
        }


        public string DownloadSpeedStr {
            get {
                if(DownloadSpeed / KILOBYTE > KILOBYTE){
                    return "D: " + Math.Round(DownloadSpeed / MEGABYTE, 2) + " MB/s";
                }else{
                    return "D: " + Math.Round(DownloadSpeed / KILOBYTE, 2) + " KB/s";
                }
            }
        }

        public string UploadSpeedStr {
            get {
                if (UploadSpeed / KILOBYTE > KILOBYTE) {
                    return "U: " + Math.Round(UploadSpeed / MEGABYTE, 2) + " MB/s";
                } else {
                    return "U: " + Math.Round(UploadSpeed / KILOBYTE, 2) + " KB/s";
                }
            }
        }


        public string Info1 {
            get {
                return DownloadedStr + " of " + SizeStr +" "+ PercentStr + ETAStr;
            }
        }

        public string Info2 {
            get {
                return DownloadSpeedStr + " " + UploadSpeedStr + " - " + SeedersConnected + "(" + SeedersAll + ") " +
                    "Seeds, " + PeersConnected + "(" + PeersAll + ") Peers";

            }
        }
        

        private void NotifyPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public int CompareTo(Torrent other) {
            return other._percent - _percent;
        }

        public int Bar
        {
            //440 is the width of the rectangle on the layout
            get { return (int)((Math.Round((double)Percent / 10.0, 2)/100) * 440); }
        }

    }
}
