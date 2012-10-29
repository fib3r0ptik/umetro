using System.Data.Linq;
namespace EZTVMetro.Models {
    public class DC : DataContext {
        public DC() : base("Data Source=isostore:/EZTVmetroData.sdf") {

        }

        public Table<TorrentClient> Clients;
        public Table<TShow> Shows;
        //public Table<TorrentClientType> ClientTypes;
    }//class

}
