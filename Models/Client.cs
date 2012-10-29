using System.Data.Linq;
using System.Data.Linq.Mapping;
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace EZTVMetro.Models {
    [Table]
    public class TorrentClient {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column(CanBeNull = false, DbType = "NVarChar(35) NOT NULL")]
        public string Name { get; set; }

        [Column()]
        public int ClientType { get; set; }

        [Column(CanBeNull = false)]
        public string Host { get; set; }

        [Column()]
        public int Port { get; set; }

        [Column(CanBeNull = false, DbType = "NVarChar(35) NOT NULL")]
        public string Username { get; set; }

        [Column(CanBeNull = false, DbType = "NVarChar(35) NOT NULL")]
        public string Password { get; set; }

        [Column(CanBeNull = false, DbType ="BIT NOT NULL")]
        public bool Auth { get; set; }
        [Column(CanBeNull = false, DbType = "NVarChar(35) NULL")]
        public string Ctype { get; set; }

    }

    public class TClients : INotifyPropertyChanged {
        private int id;
        private string name;
        private string ctype;
        private string host;
        private string port;
        private string username;
        private string password;


        public int Id {
            get { return id; }
            /*set {
                if (value != id) {
                    id = value;
                    NotifyPropertyChanged("Id");
                }
            }*/
        }

        public string Name {
            get { return name; }
            set {
                if (value != name) {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public string Type {
            get { return ctype; }
            set {
                if (value != ctype) {
                    ctype = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        public string Host {
            get { return host; }
            set {
                if (value != host) {
                    host = value;
                    NotifyPropertyChanged("Host");
                }
            }
        }

        public string Port {
            get { return port; }
            set {
                if (value != port) {
                    port = value;
                    NotifyPropertyChanged("Port");
                }
            }
        }


        public string Username {
            get { return username; }
            set {
                if (value != username) {
                    username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }


        public string Password {
            get { return password; }
            set {
                if (value != password) {
                    password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        public TClients(string name, string type, string host, string port, string username, string password) {
            Name = name;
            Type = type;
            Host = host;
            Port = port;
            Username = username;
            Password = password;
        }

        public void Add() {


        }

        public void Delete() {

        }

        /*
        public List<TorrentClient> GetAll() {

        }
        */
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

}
