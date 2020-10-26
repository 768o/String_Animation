using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace FFmpegTest
{
    class Program
    {
        private static readonly string InputPath = @"D:\VideoToStr\2.mp4";//视频路径
        private static readonly string OutputPath = @"D:\VideoToStr\Output\";//输出图片文件夹
        private static readonly string OutputTxtPath = @"D:\VideoToStr\Output\output.txt";//输出文本路径
        static StreamReader sr;
        static void Main(string[] args)
        {
            if (!Directory.Exists(OutputPath))
            {
                try
                {
                    Directory.CreateDirectory(OutputPath);
                }
                catch (Exception)
                {
                }
            }
            Console.WindowWidth = 150;
            Console.WindowHeight = 49;
            GetPicFromVideo(InputPath, 720, 576, 20);
            ImagePaint((720 / 150) + 1, (576 / (49-1)));
            PlayImageString(20 * 30);
            Console.WriteLine("132");
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VideoName">路径</param>
        /// <param name="vWidth">分辨率宽</param>
        /// <param name="vHeight">分辨率高</param>
        /// <param name="time">时长</param>
        /// <param name="CutTimeFrame"></param>
        /// <returns></returns>
        //将视频逐帧提取成图片
        static public bool GetPicFromVideo(string VideoName, int vWidth, int vHeight,int time, string CutTimeFrame = "1")
        {
            ProcessStartInfo startInfo = 
                new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + @"ffmpeg\ffmpeg.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = " -i " + VideoName
                + " -r 30"
                + " -y -f image2 -ss " + CutTimeFrame
                + " -t " + time + " -s " + vWidth + "x" + vHeight 
                + " " + OutputPath + "%d.jpg";

            try
            {
                System.Diagnostics.Process.Start(startInfo);
                Thread.Sleep(5000);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //将图片转为字符
        static public void ImagePaint(int w, int h)
        {
            int page = 0;
            if (File.Exists(OutputTxtPath))
            {
                File.Delete(OutputTxtPath);
            }
            while (true)
            {
                page++;
                Console.WriteLine(page);
                var jpgPath = OutputPath + page + ".jpg";
                if (!File.Exists(jpgPath))
                {
                    return;
                }
                #region
                Bitmap bitmap = new Bitmap(jpgPath);
                StringBuilder sb = new StringBuilder();
                string replaceChar = "@*#$%XB0H?OC7>+v=~^:_-'`. ";

                for (int i = 0; i < bitmap.Height; i += h)
                {
                    for (int j = 0; j < bitmap.Width; j += w)
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

                if (!File.Exists(OutputTxtPath))
                {
                    using (FileStream fs = new FileStream(OutputTxtPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(sb.ToString());
                        sw.Close();
                    }
                }
                else
                {
                    StreamWriter sw = File.AppendText(OutputTxtPath);
                    sw.Write(sb.ToString());
                    sw.Close();
                }
                #endregion
            }
        }

        static public void PlayImageString(int count)
        {
            FileStream fs = new FileStream(OutputTxtPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string m = "";
            for (int i = 1; i < count * 48; i++)
            {
                m += sr.ReadLine();
                m += "\n";

                if (i % 48 == 0)
                {
                    Console.Write(m);
                    Thread.Sleep(33);
                    m = "";
                }
            }
        }
    }
}
