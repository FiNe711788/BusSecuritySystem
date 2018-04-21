using Luxand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Net;
using System.Threading;

namespace SafeSystem
{
    public partial class FormMain : Form
    {
        // program states: whether we recognize faces, or user has clicked a face
        enum ProgramState { psRemember, psRecognize }
        bool needClose = false; //声明定义一个摄像头需不需要关闭
        String TrackerMemoryFile = "tracker.dat";//追踪
        private int bgImageIndex = 0;
        private List<Bitmap> bgList;
        private SerialPort GpsCom = new SerialPort();
        private StringBuilder GpsBuilder = new StringBuilder();

        //跟踪器参数
        public static bool RecognizeFaces = true;
        public static bool HandleArbitraryRotations = false;
        public static bool DetermineFaceRotationAngle = false;
        public static float FaceDetectionThreshold = 1;
        public static int InternalResizeWidth = 100;

        private static int CountingSize = 1000;
        private static int MiniAppear = 30;
        private static int SleepTime = 20;//帧间时间间隔（毫秒）

        private int cameraHandle1 = 0;//第一个摄像头的句柄
        private int cameraHandle2 = 0;//第二个摄像头的句柄

        private BackgroundWorker workerFront;
        private BackgroundWorker workerRear;

        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        //构造函数
        public FormMain()
        {
            InitializeComponent();
        }

        //初始化串口、摄像头、后台线程
        private void Form1_Load(object sender, EventArgs e)
        {
            bgList = new List<Bitmap>();
            // GpsCom.BaudRate = 9600;
            // GpsCom.PortName = WMIFunc.GetSerialPort("u-blox 6 GPS Receiver");
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            string[] files = Directory.GetFiles(Application.StartupPath + @"\Res\beijing\");
            foreach (string file in files)
            {
                Bitmap bmp = new Bitmap(file);
                bgList.Add(new Bitmap(bmp));
                bmp.Dispose();
            }
            this.BackgroundImage = bgList[bgImageIndex];
            timer1.Start();
            DateTimeLabel.BackColor = Color.Transparent;
            TimeLabel.BackColor = Color.Transparent;
            DateTimeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            TimeLabel.Text = DateTime.Now.ToString("yyyy年MM月dd日 dddd");
            try
            {
                GpsCom.Open();
                GpsTimer.Enabled = true;
            }
            catch
            {
                GpsCom = new SerialPort();
            }
            cameralInit();
            bgInit();
        }

        //初始化并开始运行三个后台线程
        void bgInit()
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerAsync();
            BackgroundWorker bg1 = new BackgroundWorker();
            bg1.DoWork += Bg1_DoWork;
            bg1.RunWorkerAsync();
            BackgroundWorker bg2 = new BackgroundWorker();
            //bg2.DoWork += Bg2_DoWork;
            bg2.RunWorkerAsync();

            //开启前门摄像头守护线程
            workerFront = new BackgroundWorker();
            workerFront.RunWorkerAsync();

            //开启后门摄像头守护线程
            workerRear = new BackgroundWorker();
            workerRear.RunWorkerAsync();
        }


        //处理三个摄像头图像
        private void Bg1_DoWork(object sender, DoWorkEventArgs e)
        {
            string ConnString = Tool.connstr;//连接字符串
            SqlConnection conn = new SqlConnection(ConnString);
            int[] countID1 = new int[CountingSize];
            byte[][] templates1 = new byte[CountingSize][];
            int[] countID2 = new int[CountingSize];
            byte[][] templates2 = new byte[CountingSize][];

            //存储所有已经出现的id
            List<long> allIDs1 = new List<long>();

            //新建1个Tracker
            int tracker1 = 0;
            FSDK.CreateTracker(ref tracker1);
            int tracker2 = 0;
            FSDK.CreateTracker(ref tracker2);

            //设置Tracker参数
            int err1 = 0;
            FSDK.SetTrackerMultipleParameters(tracker1,
                "RecognizeFaces=" + RecognizeFaces.ToString().ToLower() +
                "; HandleArbitraryRotations=" + HandleArbitraryRotations.ToString().ToLower() +
                "; DetermineFaceRotationAngle=" + HandleArbitraryRotations.ToString().ToLower() +
                "; InternalResizeWidth=" + InternalResizeWidth.ToString().ToLower() +
                "; FaceDetectionThreshold=" + FaceDetectionThreshold.ToString().ToLower() + ";",
                ref err1);
            int err2 = 0;
            FSDK.SetTrackerMultipleParameters(tracker2,
                "RecognizeFaces=" + RecognizeFaces.ToString().ToLower() +
                "; HandleArbitraryRotations=" + HandleArbitraryRotations.ToString().ToLower() +
                "; DetermineFaceRotationAngle=" + HandleArbitraryRotations.ToString().ToLower() +
                "; InternalResizeWidth=" + InternalResizeWidth.ToString().ToLower() +
                "; FaceDetectionThreshold=" + FaceDetectionThreshold.ToString().ToLower() + ";",
                ref err2);

            DateTime t1 = DateTime.Now;
            DateTime t2 = DateTime.Now;

            //记录总帧数
            int loops = 0;

            while (!needClose)
            {
                //声明定义三个图片句柄，来源于三个摄像头句柄
                Int32 imageHandle1 = 0;
                Int32 imageHandle2 = 0;
                if (FSDK.FSDKE_OK != FSDKCam.GrabFrame(cameraHandle1, ref imageHandle1))
                {
                    continue;
                }
                if (FSDK.FSDKE_OK != FSDKCam.GrabFrame(cameraHandle2, ref imageHandle2))
                {
                    continue;
                }

                FSDK.CImage image1 = new FSDK.CImage(imageHandle1);
                Image frameImage1 = image1.ToCLRImage();
                FSDK.CImage image2 = new FSDK.CImage(imageHandle2);
                Image frameImage2 = image2.ToCLRImage();

                long[] IDs1;
                long[] IDs2;
                long faceCount1 = 0;
                long faceCount2 = 0;
                FSDK.FeedFrame(tracker1, 0, image1.ImageHandle, ref faceCount1, out IDs1, sizeof(long) * 256); // maximum 256 faces detected
                FSDK.FeedFrame(tracker2, 0, image2.ImageHandle, ref faceCount2, out IDs2, sizeof(long) * 256); // maximum 256 faces detected
                Array.Resize(ref IDs1, (int)faceCount1);
                Array.Resize(ref IDs2, (int)faceCount2);
                Graphics g1 = Graphics.FromImage(frameImage1);
                Graphics g2 = Graphics.FromImage(frameImage2);

                conn.Open();

                Application.DoEvents(); //使控件可用

                //处理1号相机当前帧
                for (int i = 0; i < IDs1.Length; ++i)
                {
                    try
                    {
                        //计算人脸特征模板
                        FSDK.TFacePosition facePosition1 = new FSDK.TFacePosition();
                        FSDK.GetTrackerFacePosition(tracker1, 0, IDs1[i], ref facePosition1);

                        //绘制人脸矩形框
                        int left = facePosition1.xc - (int)(facePosition1.w * 0.5);
                        int top = facePosition1.yc - (int)(facePosition1.w * 0.5);
                        g1.DrawRectangle(Pens.LightGreen, left, top, (int)(facePosition1.w * 1.0), (int)(facePosition1.w * 1.0));

                        //计算当前ID对应人脸特征串
                        byte[] temp1 = image1.GetFaceTemplateInRegion(ref facePosition1);
                       
                        //记录当前ID
                        if (allIDs1.Count == 0)//如果是第一个ID则直接加入
                        {
                            //记录当前ID出现次数
                            countID1[IDs1[i]] = countID1[IDs1[i]] >= 100 ? 100 : countID1[IDs1[i]] + 1;

                            //更新当前ID的特征串
                            templates1[IDs1[i]] = temp1;

                            //无重复添加新的ID
                            AddIDIntoAllIDList(allIDs1, IDs1[i]);
                        }
                        else if (allIDs1.Exists(x => x == IDs1[i]))//如果已存在当前ID，则只更新特征串数据
                        {
                            //更新当前ID的特征串
                            templates1[IDs1[i]] = temp1;

                            //记录当前ID出现次数
                            countID1[IDs1[i]] = countID1[IDs1[i]] >= 100 ? 100 : countID1[IDs1[i]] + 1;
                        }
                        else//如果是新的ID，则与原来的每个ID对应的模板进行比对，看相似度
                        {
                            float simlarity = 0;
                            //foreach (long id in allIDs1)
                            //{
                            //    int r = FSDK.MatchFaces(ref templates1[id], ref temp1, ref simlarity);
                            //    if (simlarity > 0.8f)//发现相似
                            //    {
                            //        //FSDK.PurgeID(tracker1, IDs[i]);
                            //        countID1[id] = countID1[id] >= 100 ? 100 : countID1[id] + 1;
                            //        break;
                            //    }
                            //    else//没有相似
                            //    {
                            //        //记录当前ID出现次数
                            //        countID1[IDs1[i]] = countID1[IDs1[i]] >= 100 ? 100 : countID1[IDs1[i]] + 1;

                            //        //更新当前ID的特征串
                            //        templates1[IDs1[i]] = temp1;

                            //        //无重复添加新的ID
                            //        AddIDIntoAllIDList(allIDs1, IDs1[i]);
                            //    }
                            //}

                            int sim1_count = 0;
                            foreach (long id in allIDs1)
                            {
                                int r = FSDK.MatchFaces(ref templates1[id], ref temp1, ref simlarity);
                                if (simlarity > 0.8f)//发现相似
                                {
                                    countID1[id] = countID1[id] >= 100 ? 100 : countID1[id] + 1;
                                    sim1_count = sim1_count + 1;
                                    break;
                                }
                            }

                            if (sim1_count == 0)
                            {
                                //记录当前ID出现次数
                                countID1[IDs1[i]] = countID1[IDs1[i]] >= 100 ? 100 : countID1[IDs1[i]] + 1;

                                //更新当前ID的特征串
                                templates1[IDs1[i]] = temp1;

                                //无重复添加新的ID
                                AddIDIntoAllIDList(allIDs1, IDs1[i]);
                            }
                        }
                    }
                    catch { }
                }

                //处理2号相机当前帧
                for (int i = 0; i < IDs2.Length; ++i)
                {
                    try
                    {
                        //计算人脸特征模板
                        FSDK.TFacePosition facePosition2 = new FSDK.TFacePosition();
                        FSDK.GetTrackerFacePosition(tracker2, 0, IDs2[i], ref facePosition2);

                        //绘制人脸矩形框
                        int left = facePosition2.xc - (int)(facePosition2.w * 0.5);
                        int top = facePosition2.yc - (int)(facePosition2.w * 0.5);
                        g2.DrawRectangle(Pens.LightGreen, left, top, (int)(facePosition2.w * 1.0), (int)(facePosition2.w * 1.0));

                        //计算当前ID对应人脸特征串
                        byte[] temp2 = image2.GetFaceTemplateInRegion(ref facePosition2);

                        //如果是新的ID，则与原来的每个ID对应的模板进行比对，看相似度
                        float simlarity = 0;
                        foreach (long id in allIDs1)
                        {
                            int r = FSDK.MatchFaces(ref templates1[id], ref temp2, ref simlarity);
                            if (simlarity > 0.8f)//发现相似
                            {
                                countID1[id] = 0;
                                
                                int a = Convert.ToInt32(id);
                                DateTime XiaCheShiJian = DateTime.Now.ToLocalTime();
                                SqlCommand sqlCmd = new SqlCommand("UPDATE [SafeSystem].[dbo].[FaceRecognition] SET [XiaCheShiJian] = @XiaCheShiJian WHERE Template = " + a + "and XiaCheShiJian is null", conn);
                                sqlCmd.Parameters.Add("@XiaCheShiJian", System.Data.SqlDbType.DateTime);
                                sqlCmd.Parameters["@XiaCheShiJian"].Value = XiaCheShiJian;
                                sqlCmd.ExecuteNonQuery();
                                RemoveIDFromAllIDList(allIDs1, IDs1[i]);
                                break;
                            }
                        }
                    }
                    catch { }
                }

                //组织当前全部ID列表字符串
                string strAll1 = "";
                int index1 = 0;
                foreach (int id in allIDs1)
                {
                    if (index1 == 0)
                        strAll1 += "(" + id + ":" + countID1[id] + ")";
                    else
                        strAll1 += ",(" + id + ":" + countID1[id] + ")";
                    index1++;
                }

                //显示当前实时数据到当前帧
                t2 = DateTime.Now;
                TimeSpan ts = t2 - t1;
                t1 = t2;
                g1.DrawString("Frames Count：" + loops, new Font("微软雅黑", 20f), Brushes.Red, 10, 0);
                g1.DrawString("All Person：" + allIDs1.Count + "[" + strAll1 + "]", new Font("微软雅黑", 20f), Brushes.Red, 10, 50);
                
                g1.DrawString("Frame Time：" + ts.TotalMilliseconds.ToString("####.#") + "ms", new Font("微软雅黑", 20f), Brushes.Red, 10, 100);
                g1.DrawString("Frame Rate：" + (1000.0 / ts.TotalMilliseconds).ToString("####.#") + "fps", new Font("微软雅黑", 20f), Brushes.Red, 10, 150);
                g1.Dispose();
                g1 = null;
                Helper.SetImage(box1, frameImage1);
                Thread.Sleep(SleepTime);
                g2.Dispose();
                g2 = null;
                Helper.SetImage(box2, frameImage2);
                Thread.Sleep(SleepTime);

                //每处理100帧图像就清理一次出现次数很少的ID，减少计数错误
                if (loops == 100)
                {
                    List<long> bak1 = new List<long>();
                    foreach (long id in allIDs1)
                    {
                        if (countID1[id] > MiniAppear)
                        {
                            bak1.Add(id);
                            int a = Convert.ToInt32(id);
                            DateTime ShangCheShiJian = DateTime.Now.ToLocalTime();
                            SqlCommand sqlCmd = new SqlCommand("if not exists (SELECT [Template] FROM [SafeSystem].[dbo].[FaceRecognition] Where Template = '" + a + "' and XiaCheShiJian is null) INSERT INTO [SafeSystem].[dbo].[FaceRecognition] ([Template],[ShangCheShiJian]) " + " values(@Template, @ShangCheShiJian)", conn);
                            sqlCmd.Parameters.Add("@Template", System.Data.SqlDbType.Int);
                            sqlCmd.Parameters.Add("@ShangCheShiJian", System.Data.SqlDbType.DateTime);
                            sqlCmd.Parameters["@Template"].Value = a;
                            sqlCmd.Parameters["@ShangCheShiJian"].Value = ShangCheShiJian;
                            sqlCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            countID1[id] = 0;
                        }
                    }
                    allIDs1 = bak1;
                    loops = 0;
                }
                loops++;

                GC.Collect(); // collect the garbage after the deletion
                conn.Close();
            }
            FSDK.SaveTrackerMemoryToFile(tracker1, TrackerMemoryFile);
            FSDK.FreeTracker(tracker1);
            FSDK.FreeTracker(tracker2);

            FSDKCam.CloseVideoCamera(cameraHandle1);
            FSDKCam.CloseVideoCamera(cameraHandle2);
            FSDKCam.FinalizeCapturing();


        }

        //定期将线索集串行化成字符串通过网络传递给伺服
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            //while (true)
            //{
            //    XmlSerializer xs = new XmlSerializer(typeof(CluesCollect));
            //    int CentralPort = Int32.Parse(ConfigurationManager.AppSettings["centralPort"]);
            //    String CentralIp = ConfigurationManager.AppSettings["centralIp"];
            //    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(CentralIp), CentralPort);
            //    TcpClient client = null;
            //    try
            //    {
            //        client = new TcpClient(CentralIp, CentralPort);
            //    }
            //    catch
            //    {
            //        //clientPic1.Image = Image.FromFile(@".\Res\sucai\异常.png");
            //        Thread.Sleep(1000);
            //        continue;
            //    }
            //    CluesCollect cluesCollect = new CluesCollect();
            //    SqlConnection conn = new SqlConnection(Tool.connstr);
            //    conn.Open();
            //    if (num == -1)
            //    {
            //        num = (int)new SqlCommand("select count(*) from CriminalPicture", conn).ExecuteScalar();
            //    }
            //    SqlDataReader dr1 = new SqlCommand("select top(1) * from serial order by id desc", conn).ExecuteReader();
            //    if (dr1.Read())
            //    {
            //        cluesCollect.alcohol = (int)dr1["alcohol"];
            //        cluesCollect.smoke = (int)dr1["smoke"];
            //        cluesCollect.gasoline = (int)dr1["Petrol"];
            //        String s = dr1["temper"].ToString();
            //        cluesCollect.temperature = float.Parse(s);
            //    }
            //    dr1.Close();
            //    SqlDataReader dr2 = new SqlCommand("select top(1) * from gps order by id desc", conn).ExecuteReader();
            //    if (dr2.Read())
            //    {
            //        cluesCollect.gps_x = double.Parse(dr2["Longitude"].ToString());
            //        cluesCollect.gps_y = double.Parse(dr2["Latitude"].ToString());
            //    }
            //    dr2.Close();
            //    if (num != (int)new SqlCommand("select count(*) from CriminalPicture", conn).ExecuteScalar())
            //    {
            //        SqlDataReader dr3 = new SqlCommand("select top(1) * from CriminalPicture order by id desc", conn).ExecuteReader();
            //        if (dr3.Read())
            //        {
            //            num = (int)new SqlCommand("select count(*) from CriminalPicture", conn).ExecuteScalar();
            //            cluesCollect.bmp = (string)dr3["CriminalPicture"];
            //        }
            //        dr3.Close();
            //    }

            //    SqlDataReader dr4 = new SqlCommand("select top(1) * from NumberOfPeople order by id desc", conn).ExecuteReader();
            //    if (dr4.Read())
            //    {
            //        cluesCollect.pernum = (int)dr4["NumberOfPeople"];
            //    }
            //    else
            //    {
            //        cluesCollect.pernum = 0;
            //    }
            //    dr4.Close();
            //    conn.Close();
            //    cluesCollect.plateNum = ConfigurationManager.AppSettings["plateNum"];
            //    cluesCollect.busNum = ConfigurationManager.AppSettings["busNum"];
            //    cluesCollect.url_0 = ConfigurationManager.AppSettings["cameraIp"];
            //    if (cluesCollect.alcohol == 1 || cluesCollect.smoke == 1 || cluesCollect.gasoline == 1)
            //    {
            //        cluesCollect.levelnum = 1;
            //    }
            //    else
            //    {
            //        cluesCollect.levelnum = 0;
            //    }
            //    NetworkStream stream = client.GetStream();
            //    xs.Serialize(stream, cluesCollect);
            //    stream.Close();
            //    stream.Dispose();
            //    client.Close();
            //    //clientPic1.Image = Image.FromFile(@".\Res\sucai\正常.png");
            //    Thread.Sleep(3000);
            //}
        }

        //获取GPS数据
        void GetGpsData(out double x, out double y)
        {
            int n = GpsCom.BytesToRead;
            byte[] buf = new byte[n];
            GpsCom.Read(buf, 0, n);
            GpsBuilder.Remove(0, GpsBuilder.Length);
            GpsBuilder.Append(Encoding.ASCII.GetString(buf));
            String GpsTemp = GpsBuilder.ToString();
            string[] analyse = GpsTemp.Split(new char[] { '\n' });
            for (int i = 0; i < analyse.Length; i++)
            {
                if (analyse[i].Contains("GPRMC"))
                {
                    GpsTemp = analyse[i];
                    analyse = GpsTemp.Split(new char[] { ',' });
                    if (analyse[3] != "" && analyse[5] != "")
                    {
                        WGS84ToGCJ02.transform(Double.Parse(analyse[3]), Double.Parse(analyse[5]), out x, out y);
                        GCJ02ToBD09.bd_encrypt(x, y, out x, out y);
                        richTextBox1.Text = x + ";" + y;
                        return;
                    }
                }
            }
            x = 0;
            y = 0;
        }

        //初始化摄像头
        private void cameralInit()
        {
            //激活FaceSDK
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary(@"O5vT1Fmf2KYH3v4rxuEFhXL55hZPjcl/6Y412kbp1WBv9yy5S13/GtYZfTHKlXV+1a4c03NOYEeIzTsRWgVDvf6hHuyR/bD45k1kMH6dzlFsPilgR3UAcZRm01e6fbeQaomfqcVnWyMSKuXqQgCF07onN0xTKUKQKEd7cqSpuPs="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            FSDK.InitializeLibrary();

            FSDKCam.InitializeCapturing();

            string[] cameraList;//声明摄像头数组列表
            int count;//摄像头数量
            FSDKCam.GetCameraList(out cameraList, out count);//获取各个摄像头分布及其总数
            int countCam = 0;

            foreach (string cam in cameraList)
            {
                //if (cam == @"Logitech HD Webcam C270")
                if (cam == @"USB2.0 PC CAMERA1" || cam == @"USB2.0 PC CAMERA2")
                {
                    countCam++;
                }
            }

            if (count < 2)
            {
                MessageBox.Show("系统需要连接2个摄像头！", "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            string cameraName1 = cameraList[0];//读取摄像头，下同
            string cameraName2 = cameraList[1];
            //backgroundWorker1.RunWorkerAsync();//自动工作函数，一打开就自动开启摄像头进行检测

            
            FSDKCam.VideoFormatInfo[] formatList;//摄像头展现出来的视频格式数组
            FSDKCam.GetVideoFormatList(ref cameraName1, out formatList, out count);
            FSDKCam.GetVideoFormatList(ref cameraName2, out formatList, out count);
            FSDKCam.SetVideoFormat(ref cameraName1, formatList[0]);
            FSDKCam.SetVideoFormat(ref cameraName2, formatList[0]);

            int r1 = FSDKCam.OpenVideoCamera(ref cameraName1, ref cameraHandle1);
            int r2 = FSDKCam.OpenVideoCamera(ref cameraName2, ref cameraHandle2);

            if (r1 != FSDK.FSDKE_OK || r2 != FSDK.FSDKE_OK)
            {
                MessageBox.Show("开启摄像头失败！", "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        //更新背景的定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            //bgImageIndex = (bgImageIndex + 1) % bgList.Count;
            BackgroundImage = bgList[bgImageIndex];
        }

        //更新车内人数的定时器
        private void timer2_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString("yyyy年MM月dd日 dddd");
            DateTimeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            SqlConnection conn = new SqlConnection(Tool.connstr);
            conn.Open();
            peopleNum.Text = new SqlCommand("select count(*) from [SafeSystem].[dbo].[FaceRecognition] where XiaCheShiJian is null", conn).ExecuteScalar().ToString();
            conn.Close();
        }

        //获得GPS数据的定时器
        private void GpsTimer_Tick(object sender, EventArgs e)
        {
            double x, y;
            GetGpsData(out x, out y);
            SqlConnection conn = new SqlConnection(Tool.connstr);
            conn.Open();
            new SqlCommand("INSERT INTO [SafeSystem].[dbo].[GPS]([Longitude],[Latitude],[DateTime]) VALUES('" + x + "','" + y + "','" + DateTime.Now + "')", conn).ExecuteNonQuery();
            conn.Close();
            //if(GpsTemp!="")
            //richTextBox1.Text = GpsTemp;
        }


        private void AddIDIntoAllIDList(List<long> IDs, long id)
        {
            if (!IDs.Exists(x => x == id))
            {
                IDs.Add(id);
            }
        }
        private void RemoveIDFromAllIDList(List<long> IDs, long id)
        {
            if (IDs.Exists(x => x == id))
            {
                IDs.Remove(id);
            }
        }

        private void safesystem1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void peopleNum_Click(object sender, EventArgs e)
        {

        }

        private void DateTimeLabel_Click(object sender, EventArgs e)
        {

        }
    }

    public struct TFaceRecord
    {
        public byte[] Template; //Face Template;
        public FSDK.TFacePosition FacePosition;
        public FSDK.TPoint[] FacialFeatures; //Facial Features;

        public string ImageFileName;

        public FSDK.CImage image;
        public FSDK.CImage faceImage;
    }
}