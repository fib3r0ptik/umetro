using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace EZTVMetro.Models {
    [Table]
    public class TShow {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column(CanBeNull = false, DbType = "NVarChar(100) NOT NULL")]
        public string Title { get; set; }

        [Column(CanBeNull = false, DbType = "NVarChar(100) NOT NULL")]
        public string Status { get; set; }
    }



    public class Show : INotifyPropertyChanged {
        private string title;
        private string status;

        public string Title {
            get { return title; }
            set {
                if (value != title) {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Status {
            get { return status; }
            set {
                if (value != status) {
                    status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Constructor with full Listing information
        /// </summary>
        public Show(string title, string status) {
            Title = title;
            Status = status;
        }

        /// <summary>
        /// Raise the PropertyChanged event and pass along the property that changed
        /// </summary>
        private void NotifyPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
}
