using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FFmpegTest
{
    class Program
    {
        static StreamReader sr;
        static void Main(string[] args)
        {
            GetPicFromVideo(@"C:\Users\L-KAMI\Desktop\BedApple\BedApple.flv", "705x576", "1");
            ImagePaint();
            Console.WindowHeight = 48;
            Console.WindowWidth = 150;
            PlayImageString();
        }

        //将视频逐帧提取成图片
        static public string GetPicFromVideo(string VideoName, string WidthAndHeight, String CutTimeFrame)
        {
            string ffmpeg = @"H:\ffmpeg\bin\ffmpeg.exe";
            string PicName = VideoName + ".jpg";
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.Arguments = " -i " + VideoName
                + " -r 18"
                + " -y -f image2 -ss " + CutTimeFrame
                + " -t 513 -s " + WidthAndHeight
                + @" C:\Users\L-KAMI\Desktop\BedApple\%d.jpg";

            try
            {
                System.Diagnostics.Process.Start(startInfo);
                Thread.Sleep(5000);
            }
            catch (Exception)
            {
                return "";
            }

            if (System.IO.File.Exists(PicName))
            {
                return PicName;
            }

            return "";
        }

        //将图片转为字符
        static public void ImagePaint()
        {
            int page = 0;
            while (true)
            {
                page++;
                Console.WriteLine(page);
                if (!File.Exists(@"C:\Users\L-KAMI\Desktop\BedApple\"+page+".jpg"))
                {
                    return;
                }
                
                #region
                Bitmap bitmap = new Bitmap(@"C:\Users\L-KAMI\Desktop\BedApple\" + page + ".jpg");

                StringBuilder sb = new StringBuilder();
                string replaceChar = "@*#$%XB0H?OC7>+v=~^:_-'`. ";

                
                for (int i = 0; i < bitmap.Height; i += 12)
                {
                    for (int j = 0; j < bitmap.Width; j += 6)
                    {
                        //获取当前点的color对象
                        Color c = bitmap.GetPixel(j, i);

                        //计算灰度化后的rgb值
                        int rgb = (int)(c.R * .3 + c.G * .59 + c.B * .11);

                        //计算出replaceChar中要替换的字符index
                        //所以根据当前灰度所占总rgb的比例(rgb值最大为255，为了防止超出索引界限所以/256.0)
                        //（肯定是小于1的小数）乘以总共要替换字符的字符数，获取当前灰度程度在字符串中的复杂程度
                        int index = (int)(rgb / 256.0 * replaceChar.Length);

                        //追加进入sb
                        sb.Append(replaceChar[index]);
                    }

                    sb.Append("\r\n");
                }

                if (!File.Exists(@"C:\Users\L-KAMI\Desktop\BedApple\test.txt"))
                {
                    using (FileStream fs = new FileStream(@"C:\Users\L-KAMI\Desktop\BedApple\test.txt", FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        //byte[] bs = Encoding.Default.GetBytes();

                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(sb.ToString());
                    }
                }
                else
                {
                    StreamWriter sw = File.AppendText(@"C:\Users\L-KAMI\Desktop\BedApple\test.txt");
                    sw.Write("\n");
                    sw.Write(sb.ToString());
                    sw.Close();
                }
                #endregion


            }
        }

        static public void PlayImageString()
        {
            FileStream fs = new FileStream(@"C:\Users\L-KAMI\Desktop\BedApple\test.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string m = "";

            for (int i = 7; i < 256563; i++)
            {
                m += sr.ReadLine();
                m += "\n";
                

                if (i % 49 == 0)
                {
                    
                    Console.WriteLine(m);
                    
                    Thread.Sleep(25);
                    m = "";
                }
                
            }
            
        }
    }
}
