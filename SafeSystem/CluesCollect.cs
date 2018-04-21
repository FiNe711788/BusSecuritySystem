using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SafeSystem
{
    [Serializable]
    public class CluesCollect
    {
        public int id;
        public String busNum;
        public String plateNum;
        public float temperature;
        public int pernum;
        public int alcohol;
        public int gasoline;
        public int smoke;
        public String bmp;
        public double gps_x;
        public double gps_y;
        public double gps_z;
        public double levelnum;
        public String url_0;
        public String url_1;
        public String url_2;
        public String url_3;
        public String url_4;
        public String url_5;
        public String url_6;
        public String url_7;
        public String url_8;
        public String url_9;
        public DateTime time;
        public CluesCollect()
        {

        }

        [XmlIgnore]
        public Bitmap Bmp
        {
            get
            {
                byte[] arr = Convert.FromBase64String(this.bmp);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }

            set
            {
                MemoryStream ms = new MemoryStream();
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                bmp = Convert.ToBase64String(arr);
            }
        }
    }
}
