using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing.Drawing2D;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SafeSystem
{
    public partial class Safesystem1 : UserControl
    {
        string PortName = "USB-SERIAL CH340";////////这里需填U转串的设备名//////////
        string SerialData = "";
        private SerialPort comm = new SerialPort();
        private StringBuilder builder = new StringBuilder();
        int num = 0;
        public Safesystem1()
        {
            InitializeComponent();
            SetStyle(
                       ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.Selectable
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor,
                     true);
        }
        public void SerialPortOpen()
        {
            comm.BaudRate = 9600;
            comm.PortName = WMIFunc.GetSerialPort(PortName);
            try
            {
                comm.Open();
            }
            catch
            {
                //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
                comm = new SerialPort();
            }

        }
        public void GetSerialData()
        {
            int n = comm.BytesToRead;
            byte[] buf = new byte[n];
            comm.Read(buf, 0, n);//读取缓冲数据
            builder.Remove(0, builder.Length);
            foreach (byte b in buf)
            {
                builder.Append(b.ToString("X2"));
            }
            SerialData = builder.ToString();
        }
        public Result TransCompute(string Data)
        {
            Result res = new Result();
            //取温度整数部分高四位
            string TemperHighFront = Data.Substring(4, 1);
            //取温度整数部分低四位
            string TemperHighBack = Data.Substring(5, 1);
            //取温度小数部分
            string TemperLow = Data.Substring(6, 2);
            //将温度整数高四位、低四位、小数全部由十六进制转化为十进制
            int tlD = Convert.ToInt32(TemperLow, 16);
            int thfD = Convert.ToInt32(TemperHighFront, 16);
            int thbD = Convert.ToInt32(TemperHighBack, 16);
            //当温度整数二进制高四位的第一位等于1时，其十进制必定大于等于8，故有如下判断修改
            if (thfD >= 8)
            {
                thfD -= 8;
                thfD = -thfD;
                thbD = -thbD;
                tlD = -tlD;
            }
            //计算出温度值，传给全局变量
            res.Temper = thfD * 16 + thbD + tlD * 0.0625;
            //读取其他传感器数据位
            string DataBack;
            DataBack = Data.Substring(9, 1);
            int DataBackD = Convert.ToInt32(DataBack, 16);
            switch (DataBackD)
            {
                case 1:
                    res.SmokeSenser = 1;
                    break;
                case 2:
                    res.AlcoholSenser = 1;
                    break;
                case 3:
                    res.SmokeSenser = 1;
                    res.AlcoholSenser = 1;
                    break;
                case 4:
                    res.InfraredSenser = 1;
                    break;
                case 5:
                    res.SmokeSenser = 1;
                    res.InfraredSenser = 1;
                    break;
                case 6:
                    res.AlcoholSenser = 1;
                    res.InfraredSenser = 1;
                    break;
                case 7:
                    res.SmokeSenser = 1;
                    res.AlcoholSenser = 1;
                    res.InfraredSenser = 1;
                    break;
                case 8:
                    res.PetrolSenser = 1;
                    break;
                case 9:
                    res.SmokeSenser = 1;
                    res.PetrolSenser = 1;
                    break;
                case 10:
                    res.AlcoholSenser = 1;
                    res.PetrolSenser = 1;
                    break;
                case 11:
                    res.SmokeSenser = 1;
                    res.PetrolSenser = 1;
                    res.AlcoholSenser = 1;
                    break;
                case 12:
                    res.PetrolSenser = 1;
                    res.InfraredSenser = 1;
                    break;
                case 13:
                    res.PetrolSenser = 1;
                    res.InfraredSenser = 1;
                    res.SmokeSenser = 1;
                    break;
                case 14:
                    res.PetrolSenser = 1;
                    res.InfraredSenser = 1;
                    res.AlcoholSenser = 1;
                    break;
                case 15:
                    res.PetrolSenser = 1;
                    res.InfraredSenser = 1;
                    res.AlcoholSenser = 1;
                    res.SmokeSenser = 1;
                    break;
            }
            return res;
        }

        private void Safesystem1_Load(object sender, EventArgs e)
        {

            this.BackColor = Color.Transparent;
            if (WMIFunc.GetSerialPort(PortName) != "")
            {
                SerialPortOpen();
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetSerialData();
            if (SerialData.Length == 14)
            {
                string gotTemper = TransCompute(SerialData).Temper.ToString();
                DateTime gotTemperTime = DateTime.Now.ToLocalTime();
                SqlConnection conn = new SqlConnection(Tool.connstr);
                wendu.Text = Math.Round(TransCompute(SerialData).Temper, 1).ToString() + "℃";
                if (TransCompute(SerialData).AlcoholSenser == 1)
                    //TextAlcohol.Text = "检测到";
                    jiujing.Image = Image.FromFile(@".\Res\sucai\jiujingy.png");
                else
                    //TextAlcohol.Text = "未检测到";
                    jiujing.Image = Image.FromFile(@".\Res\sucai\jiujingn.png");
                if (TransCompute(SerialData).PetrolSenser == 1)
                    //TextPetrol.Text = "检测到";
                    qiyou.Image = Image.FromFile(@".\Res\sucai\qiyouy.png");
                else
                    //TextPetrol.Text = "未检测到";
                    qiyou.Image = Image.FromFile(@".\Res\sucai\qiyoun.png");
                if (TransCompute(SerialData).SmokeSenser == 1)
                    //TextSmoke.Text = "检测到";
                    yanwu.Image = Image.FromFile(@".\Res\sucai\yanwuy.png");
                else
                    //TextSmoke.Text = "未检测到";
                    yanwu.Image = Image.FromFile(@".\Res\sucai\yanwun.png");
                if (TransCompute(SerialData).InfraredSenser == 1)
                    num += 1;
                conn.Open();
                string sql = "INSERT INTO [dbo].[Serial]([Alcohol],[Smoke],[Petrol],[temper] ,[time])VALUES(" + TransCompute(SerialData).AlcoholSenser + "," + TransCompute(SerialData).SmokeSenser + "," + TransCompute(SerialData).PetrolSenser + "," + Math.Round(TransCompute(SerialData).Temper, 1) + ",'" + DateTime.Now + "')";
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void qiyou_Click(object sender, EventArgs e)
        {

        }
    }
    public class Result
    {
        public double Temper;
        public int AlcoholSenser, SmokeSenser, PetrolSenser, InfraredSenser;
        public Result()
        {
            Temper = 0;
            AlcoholSenser = 0;
            SmokeSenser = 0;
            PetrolSenser = 0;
            InfraredSenser = 0;
        }
    }
}
