using ImageFastLoader.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageFastLoader
{
    public class ImageProvider : IItemsProvider<DocumentData>
    {
        private readonly int _count;
        private readonly int _fetchDelay;

        string[] jpgFiles = null;

        public ImageProvider(int fetchDelay)
        {
            //_count = numItems;
            _fetchDelay = fetchDelay;
            string path = ConfigurationManager.AppSettings["ImagePath"];
            jpgFiles = Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly);
            _count = jpgFiles.Length;
        }

        public int FetchCount()
        {
            return _count;
        }

        public IList<DocumentData> FetchRange(int startIndex, int count)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IList<DocumentData> picList = new List<DocumentData>();

            if (startIndex + count <= _count)
            {
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    //BitmapImage image = new BitmapImage();
                    //image.CacheOption = BitmapCacheOption.OnLoad;
                    //image.BeginInit();
                    //image.UriSource = new Uri(jpgFiles[i], UriKind.Relative);
                    //image.EndInit();
                    //image.Freeze();
                    picList.Add(new DocumentData { FilePath = jpgFiles[i], FileName = Path.GetFileName(jpgFiles[i]) });
                }
            }
            sw.Stop();

            Trace.WriteLine(string.Format("ATTN: {0} bitmapimages in {1} msec", count, sw.ElapsedMilliseconds));
            return picList;
        }
    }
}
