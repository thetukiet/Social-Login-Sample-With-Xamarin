using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Xamarin.Forms;

namespace Sample.Droid
{
    [Activity(Label = "Sample", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                Forms.Init(this, bundle);
                StartActivity(typeof(MainActivity));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

