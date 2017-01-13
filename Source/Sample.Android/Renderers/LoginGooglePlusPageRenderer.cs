using System;
using Android.App;
using Android.Content.PM;
using Sample.Common;
using Sample.Common.Entities;
using Sample.Common.Helpers;
using Sample.Common.Pages;
using Sample.Droid.Renderers;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginGooglePlusPage), typeof(LoginGooglePlusPageRenderer))]
namespace Sample.Droid.Renderers
{    
    public class LoginGooglePlusPageRenderer : PageRenderer
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var realControl = e.NewElement as LoginGooglePlusPage;
            if (realControl == null)
                DefaultApp.NavigateToPage(new LoginPage());

            var activity = Context as Activity;
            activity.RequestedOrientation = ScreenOrientation.Portrait;

            try
            {
                GoValidateNewInput(activity);
            }
            catch (Exception)
            {
                await DefaultApp.DisplayErrorDialog(GlobleMessages.CheckNetworkMessage);
                DefaultApp.NavigateToPage(new LoginPage());
            }
        }


        private void GoValidateNewInput(Activity activity)
        {
            var auth = new OAuth2Authenticator(
                    clientId: Constant.GOOGLEPLUS_ID,
                    clientSecret: Constant.GOOGLEPLUS_CLIENT_SECRET,
                    scope: Constant.GOOGLEPLUS_ACCESS_SCOPE,
                    authorizeUrl: new Uri(Constant.GOOGLEPLUS_OAUTH_URL),
                    redirectUrl: new Uri(Constant.GOOGLEPLUS_LOGIN_SUCCESS_URL),
                    accessTokenUrl: new Uri(Constant.GOOGLEPLUS_LOGIN_ACCESS_TOKEN_URL));

            auth.Completed += OnCompleteValidateAccount;

            activity.StartActivity(auth.GetUI(activity));
        }


        private async void OnCompleteValidateAccount(object sender, AuthenticatorCompletedEventArgs eventArgs)
        {
            try
            {
                var isLoginSuccess = eventArgs.IsAuthenticated;
                if (isLoginSuccess)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"];
                    var accessInfo = await NetworkHelper.GetGooglePlusUserInfoByToken(accessToken);

                    if (accessInfo == null)
                    {
                        await DefaultApp.DisplayErrorDialog(GlobleMessages.AccessGoogleAccountFailed);
                        DefaultApp.NavigateToPage(new LoginPage());
                    }
                    else
                    {
                        eventArgs.Account.Username = accessInfo.Email;
                        eventArgs.Account.Properties.Add("id", accessInfo.Id);
                        eventArgs.Account.Properties.Add("full_name", accessInfo.Name);
                        eventArgs.Account.Properties.Add("avatar_url", accessInfo.AvatarUrl);

                        await CachingHelper.CleanAccountStorage(GlobleSetting.GooglePlusAccountDomain);
                        var accountStorage = AccountStore.Create();
                        accountStorage.Save(eventArgs.Account, GlobleSetting.GooglePlusAccountDomain);

                        await DefaultApp.RegisterAndVerifyAccount(accessToken, accessInfo, SocialDomain.Google);
                    }
                }
                else
                {
                    DefaultApp.CollectCurrentPage();
                }
            }
            catch (Exception)
            {
                await DefaultApp.DisplayErrorDialog(GlobleMessages.CheckNetworkMessage);
                DefaultApp.NavigateToPage(new LoginPage());
            }
        }
    }
}