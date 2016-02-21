using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorToGrayScale
{
    public class ImageViewModel : BindableBase
    {

        public ImageViewModel()
        {
            OriginalPath = string.Empty;
            LoadImageCommand = new DelegateCommand(ExecuteLoadImage);
            GrayImageCommand = new DelegateCommand(ExecuteGrayImage);
        }

        private void ExecuteGrayImage()
        {
            var image = Image.FromFile(OriginalPath);
            Bitmap bitmap = new Bitmap(image);

            //var imageGrayScale = MakeGrayscaleWithNaiveAlgorithm(bitmap);

            var imageGrayScale = MakeGrayscaleWithColorMatrix(bitmap);

            //ImageSourceConverter conv = new ImageSourceConverter();
            //GrayScaleImageSource= conv.ConvertFrom(imageGrayScale) as ImageSource;

            GrayScaleImageSource = ColorHelper.ConvertBitmapToImageSource(imageGrayScale);

        }

        

        private Bitmap MakeGrayscaleWithNaiveAlgorithm(Bitmap orig)
        {
            for (int i = 0; i < orig.Width; i++)
            {
                for (int j = 0; j < orig.Height; j++)
                {
                    var pix = orig.GetPixel(i, j);
                    var avg = (pix.R + pix.B + pix.G) / 3;
                    orig.SetPixel(i, j, System.Drawing.Color.FromArgb(pix.A, avg, avg, avg));
                }
            }

            return orig;
        }


        private Bitmap MakeGrayscaleWithColorMatrix(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });
            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        private void ExecuteLoadImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                OriginalPath = ofd.FileName;
            }
        }

        private ICommand grayImageCommand;

        public ICommand GrayImageCommand
        {
            get { return grayImageCommand; }
            set { grayImageCommand = value; OnPropertyChanged(() => GrayImageCommand); }
        }


        private ImageSource grayScaleImageSource;

        public ImageSource GrayScaleImageSource
        {
            get { return grayScaleImageSource; }
            set
            {
                grayScaleImageSource = value;
                OnPropertyChanged(() => GrayScaleImageSource);
            }
        }

        private string origPath;

        public string OriginalPath
        {
            get { return origPath; }
            set
            {
                origPath = value;
                OnPropertyChanged(() => OriginalPath);
            }
        }


        private ICommand loadOrig;

        public ICommand LoadImageCommand
        {
            get { return loadOrig; }
            set
            {
                loadOrig = value;
                OnPropertyChanged(() => LoadImageCommand);
            }
        }





    }
}
