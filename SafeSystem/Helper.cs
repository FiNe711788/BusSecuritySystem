using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SafeSystem
{
    class Helper
    {
        private delegate void SetImageHandler(PictureBox box, Image img);
        public static void SetImage(PictureBox box, Image img)
        {
            if (box.InvokeRequired)
            {
                SetImageHandler handler = new SetImageHandler(SetImage);
                box.Invoke(handler, new object[] { box, img });
            }
            else
            {
                box.Image = img;
            }
        }
    }
}
