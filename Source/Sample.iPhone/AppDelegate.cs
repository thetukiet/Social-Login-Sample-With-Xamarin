using Foundation;

using UIKit;
using Sample.Common;
using Sample.Common.Helpers;
using Sample.iOS.Services;

namespace Sample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public static DefaultApp RunningApp;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            ServicesRegister.Register();

            Xamarin.Forms.Forms.Init();
            RunningApp = new DefaultApp();

            CachingHelper.InitStorage();
            RunningApp.InitView();
            LoadApplication(RunningApp);
            return base.FinishedLaunching(application, launchOptions);
        }
    }
}


