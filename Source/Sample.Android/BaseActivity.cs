using System;
using Android.App;
using Xamarin.Forms.Platform.Android;

namespace Sample.Droid
{    
    public class BaseActivity : FormsApplicationActivity
    {
        protected void ShowInforDialog(string title, string message)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetCancelable(true);
            builder.SetPositiveButton("OK", delegate { });
            builder.Show();
        }

        protected void ShowDialogWithAction(string title, string message, string actionMessage, Action action)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetCancelable(true);
            builder.SetPositiveButton(actionMessage, delegate { action(); });
            builder.Show();
        }
    }
}

