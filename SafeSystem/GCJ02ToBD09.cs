using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeSystem
{
    class GCJ02ToBD09
    {
        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        public static void bd_encrypt(double gg_lat, double gg_lon, out double bd_lat, out double bd_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            bd_lon = z * Math.Cos(theta) + 0.0065;
            bd_lat = z * Math.Sin(theta) + 0.006;
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        public static void bd_decrypt(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            gg_lon = z * Math.Cos(theta);
            gg_lat = z * Math.Sin(theta);
        }
    }
}
