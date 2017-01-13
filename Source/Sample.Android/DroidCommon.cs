using System.Threading.Tasks;
using Android.Graphics;
using Sample.Common.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Sample.Droid
{    
    public class DroidCommon
    {
        public static Bitmap GetBitmapFromImageSource(SourceType sourceType, string keyValue)
        {
            ImageSource imageSource = null;
            switch (sourceType)
            {
                case SourceType.FromLocalFile:
                    {
                        imageSource = ImageSource.FromFile(keyValue);
                        break;
                    }
                case SourceType.FromResource:
                    {
                        imageSource = ImageSource.FromResource(keyValue);
                        break;
                    }
                case SourceType.FromUri:
                    {
                        imageSource = ImageSource.FromUri(new System.Uri(keyValue));
                        break;
                    }
            }
            return GetBitmapAsync(imageSource).Result;
        }


        public static Bitmap GetBitmapFromImageSource(ImageSource imageSource)
        {
            return GetBitmapAsync(imageSource).Result;
        }


        private static async Task<Bitmap> GetBitmapAsync(ImageSource source)
        {
            var handler = GetHandler(source);
            var returnValue = await handler.LoadImageAsync(source, null);

            return returnValue;
        }


        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            return returnValue;
        }
    }
}

