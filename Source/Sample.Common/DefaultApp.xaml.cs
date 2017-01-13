using System;
using System.Net;
using System.Threading.Tasks;

using Android.Util;
using Xamarin.Forms;
using XLabs.Ioc;

using Sample.Common.Entities;
using Sample.Common.Helpers;
using Sample.Common.Pages;

namespace Sample.Common
{
    public partial class DefaultApp : Application
    {
        public DefaultApp()
        {
            InitializeComponent();
        }

        public void InitView()
        {
            ContentPage firstPage;

            var loginInfo = CachingHelper.GetLoginInfoFromCache();            
            if (loginInfo != null)
            {
                var accountInfo = new SocialAccountInfo
                {
                    AvatarUrl = loginInfo.AvatarUrl,
                    Email = loginInfo.Email,
                    Name = loginInfo.Name,
                    Id = loginInfo.SocialId
                };

                firstPage = new WelcomePage(accountInfo, loginInfo.SocialDomain);

                NavigationPage.SetHasNavigationBar(firstPage, false);
                NavigationPage navigatePage = new NavigationPage(firstPage);
                Current.MainPage = navigatePage;
            }
            else
            {
                firstPage = new LoginPage();
                NavigationPage.SetHasNavigationBar(firstPage, false);
                NavigationPage navigatePage = new NavigationPage(firstPage);
                Current.MainPage = navigatePage;
            }
        }


        public static async Task OpenUrl(string url)
        {
            try
            {
                var commonHelper = Resolver.Resolve<ICommonHelper>();
                commonHelper.OpenUrl(url);
            }
            catch (Exception ex)
            {
                await DisplayDialog("Error", "Temporary cannot open your profile address", "OK");
                Console.WriteLine(ex.Message);
            }
        }


        public static async Task DisplayDialog(string title, string message, string closeButtonCaption)
        {
            await Current.MainPage.DisplayAlert(title, message, closeButtonCaption);
        }

        public static async Task DisplayDialog(string title, string message)
        {
            await DisplayDialog(title, message, "OK");
        }

        public static async Task DisplayErrorDialog(string message)
        {
            await DisplayDialog("Error", message, "OK");
        }


        public static async Task<bool> DisplayConfirmationDialog(string title, string askMessage)
        {
            return await Current.MainPage.DisplayAlert(title, askMessage, "Yes", "No");
        }


        public static string GetDeviceType()
        {
            var platform = Device.OS;
            switch (platform)
            {
                case TargetPlatform.iOS:
                    return "ios";
                case TargetPlatform.Android:
                    return "android";
            }

            return string.Empty;
        }


        public static async void CollectCurrentPage()
        {
            try
            {
                await Current.MainPage.Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Log.Error("CollectCurrentPage", ex.Message);
            }
        }

        public static void NavigateToPage(ContentPage viewPage)
        {
            try
            {
                if (viewPage == null)
                    return;
                NavigationPage.SetHasNavigationBar(viewPage, false);
                NavigationPage navigatePage = new NavigationPage(viewPage);
                Current.MainPage = navigatePage;
            }
            catch (Exception ex)
            {
                Log.Error("NavigateToPage", ex.Message);
            }
        }

        public static async Task AddPage(ContentPage viewPage)
        {
            try
            {
                if (viewPage == null)
                    return;

                NavigationPage.SetHasNavigationBar(viewPage, false);
                await Current.MainPage.Navigation.PushModalAsync(viewPage, true);
            }
            catch (Exception ex)
            {
                Log.Error("AddPage", ex.Message);
            }
        }

        public static async Task ShowPromb(string content, PrombAnswer answerFunc)
        {
            await ShowPromb(content, string.Empty, answerFunc);
        }

        public static async Task ShowPromb(string content, string iconResource, PrombAnswer answerFunc)
        {
            try
            {
                var promb = new PrombPage(content, iconResource);
                promb.PrombAnswerEvent += (result) =>
                {
                    Current.MainPage.Navigation.PopModalAsync(true);
                };
                promb.PrombAnswerEvent += answerFunc;
                NavigationPage.SetHasNavigationBar(promb, false);
                await Current.MainPage.Navigation.PushModalAsync(promb, true);
            }
            catch (Exception ex)
            {
                Log.Error("Show Promb", ex.Message);
            }
        }


        public static void RemoveTitleBar(ContentPage withView)
        {
            NavigationPage.SetHasNavigationBar(withView, false);
        }


        public static async Task RegisterAndVerifyAccount(string accountToken, SocialAccountInfo accountInfo, SocialDomain fromDomain)
        {
            var webClient = new WebClient();
            try
            {
                // Save login info to cache
                var loginInfo = new LogInInfo
                {
                    Email = accountInfo.Email,
                    Name = accountInfo.Name,
                    SocialDomain = fromDomain,
                    SocialId = accountInfo.Id,
                    AvatarUrl = accountInfo.AvatarUrl
                };

                CachingHelper.SaveLoginInfoToCache(loginInfo);
                Current.MainPage = new WelcomePage(accountInfo, fromDomain);
            }
            catch (Exception ex)
            {
                //CollectCurrentPage();
                await DisplayErrorDialog(GlobleMessages.CheckNetworkMessage);
            }
            finally
            {
                if (webClient.IsBusy)
                {
                    webClient.CancelAsync();
                    webClient.Dispose();
                }
            }
        }


        public static string GetResourceStringValue(string resourceKey)
        {
            try
            {
                object keyValue;
                Current.Resources.TryGetValue(resourceKey, out keyValue);
                return (string) keyValue;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
