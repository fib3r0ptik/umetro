using System;
using System.ComponentModel;

namespace EZTVMetro.Models
{
    public class Listing : INotifyPropertyChanged
    {
        private string title;
        private string size;
        private string link;
        private string pubdate;
        private string showlink;

        public string Title {
            get { return title; }
            set {
                if (value != title) {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Size {
            get { return size; }
            set {
                if (value != size) {
                    size = value;
                    NotifyPropertyChanged("Size");
                }
            }
        }

        public string Link {
            get { return link; }
            set {
                if (value != link) {
                    link = value;
                    NotifyPropertyChanged("Link");
                }
            }
        }

        public string Showlink {
            get { return showlink; }
            set {
                if (value != showlink) {
                    showlink = value;
                    NotifyPropertyChanged("Showlink");
                }
            }
        }


        public string PubDate {
            get { return pubdate; }
            set {
                if (value != pubdate) {
                    pubdate = value;
                    NotifyPropertyChanged("PubDate");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Constructor with full Listing information
        /// </summary>
        public Listing(string title, string size, string link, string pubdate, string showlink)
        {
            Title = title;
            Size = size;
            Link = link;
            PubDate = pubdate;
            Showlink = showlink;
        }

        /// <summary>
        /// Raise the PropertyChanged event and pass along the property that changed
        /// </summary>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
}
