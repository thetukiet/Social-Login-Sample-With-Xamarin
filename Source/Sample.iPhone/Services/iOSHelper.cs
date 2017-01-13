using Foundation;
using UIKit;
using Sample.Common.Helpers;

namespace Sample.iOS.Services
{
    public class iOsHelper : ICommonHelper
    {
        public void OpenUrl(string urlData)
        {
            if (urlData != string.Empty)
            {
                NSUrl url = new NSUrl(urlData);
                if (UIApplication.SharedApplication.CanOpenUrl(url))
                {
                    UIApplication.SharedApplication.OpenUrl(url);
                }
            }
        }
    }
}