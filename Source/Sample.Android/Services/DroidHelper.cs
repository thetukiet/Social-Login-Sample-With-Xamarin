using System;
using System.Threading.Tasks;
using Sample.Common.Helpers;
using Xamarin.Forms;

namespace Sample.Droid.Services
{
    public class DroidHelper : ICommonHelper
    {
        public void OpenUrl(string url)
        {
            Task.Run(() =>
            {
                Device.OpenUri(new Uri(url));
            });
        }
    }
}