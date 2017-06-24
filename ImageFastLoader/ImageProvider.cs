using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageFastLoader
{
    public class ImageProvider : IItemsProvider<Picture>
    {
        private readonly int _count;
        private readonly int _fetchDelay;

        string[] jpgFiles = null;

        public ImageProvider(int numItems, int fetchDelay)
        {
            _count = numItems;
            _fetchDelay = fetchDelay;
            string path = @"C:\Bhabesh\Personal\Pics";
            jpgFiles = Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly);
            _count = jpgFiles.Length;
        }

        public int FetchCount()
        {
            return _count;
        }

        public IList<Picture> FetchRange(int startIndex, int count)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IList<Picture> picList = new List<Picture>();

            if (startIndex + count <= _count)
            {
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    BitmapImage image = new BitmapImage();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.BeginInit();
                    image.UriSource = new Uri(jpgFiles[i], UriKind.Relative);
                    image.EndInit();
                    image.Freeze();
                    picList.Add(new Picture { Pic = image });
                }
            }
            sw.Stop();

            Trace.WriteLine( string.Format("ATTN: 10 bitmapimages in {0} msec", sw.ElapsedMilliseconds));
            return picList;
        }
    }
}
