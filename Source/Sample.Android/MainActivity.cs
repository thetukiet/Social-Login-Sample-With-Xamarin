using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Sample.Common;
using Sample.Common.Helpers;
using Sample.Droid.Services;
using Xamarin.Forms;

namespace Sample.Droid
{
    [Activity(Label = "Sample", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
        public static DefaultApp RunningApp;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                ServicesRegister.Register();
                base.OnCreate(bundle);
                Forms.Init(this, bundle);

                CachingHelper.InitStorage();
                RunningApp = new DefaultApp();
                RunningApp.InitView();

                LoadApplication(RunningApp);

                ActionBar.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

