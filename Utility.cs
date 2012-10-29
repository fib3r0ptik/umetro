using System;
namespace EZTVMetro {
    public static class Utility {
        public static string stringToFancyDate(string date){
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            DateTime dt =  DateTime.Parse(date);
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double elapse = Math.Abs(ts.TotalSeconds);


            if (elapse < 0) {
                return "not yet";
            }
            if (elapse < 1 * MINUTE) {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (elapse < 2 * MINUTE) {
                return "a minute ago";
            }
            if (elapse < 45 * MINUTE) {
                return ts.Minutes + " minutes ago";
            }
            if (elapse < 90 * MINUTE) {
                return "an hour ago";
            }
            if (elapse < 24 * HOUR) {
                return ts.Hours + " hours ago";
            }
            if (elapse < 48 * HOUR) {
                return "yesterday";
            }
            if (elapse < 30 * DAY) {
                return ts.Days + " days ago";
            }
            if (elapse < 12 * MONTH) {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            } else {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }


        public static string bytesToFancyString(double bytes) {
            const long KILOBYTE = 1024L;
            const long MEGABYTE = 1024L * KILOBYTE;
            const long GIGABYTE = 1024L * MEGABYTE;

            string ret="";

            if(bytes >= GIGABYTE){
                ret  += Convert.ToString(bytes / GIGABYTE)+" GB";
                //bytes = bytes % GIGABYTE;
            }

            if(bytes >=MEGABYTE && bytes < GIGABYTE){
                ret += Convert.ToString(bytes / MEGABYTE) + " MB";
                //bytes = bytes % MEGABYTE;
            }

            if (bytes >= KILOBYTE && bytes < MEGABYTE) {
                ret += Convert.ToString(bytes / KILOBYTE) + " KB";
                //bytes = bytes % MEGABYTE;
            }

            return ret;
        }

        public static string secToFancyString(int time) {
            string ret = "";
            int w = (time / 86400) / 7;
            int d = (time / 86400) % 7;
            int h = (time / 3600) % 24;
            int m = (time / 60) % 60;
            if (w > 0) {
                ret = w + "w ";
            }
            if (d > 0) {
                ret += d + "d ";
            }
            if (h > 0) {
                ret += h + "h ";
            }
            if (m > 0) {
                ret += m + "m";
            }

            return ret;
        }


    }//class
}
