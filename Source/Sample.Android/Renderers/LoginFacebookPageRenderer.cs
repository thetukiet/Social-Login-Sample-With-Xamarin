using System;
using Android.App;
using Android.Content.PM;

using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Sample.Common;
using Sample.Common.Entities;
using Sample.Common.Helpers;
using Sample.Common.Pages;
using Sample.Droid.Renderers;

[assembly: ExportRenderer(typeof(LoginFacebookPage), typeof(LoginFacebookPageRenderer))]
namespace Sample.Droid.Renderers
{    
    public class LoginFacebookPageRenderer : PageRenderer
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var realControl = e.NewElement as LoginFacebookPage;
            DefaultApp.RemoveTitleBar(realControl);

            if (realControl == null)
                return;

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
                    clientId: Constant.FACEBOOK_ID,
                    scope: Constant.FACEBOOK_ACCESS_BASE_SCOPE,
                    authorizeUrl: new Uri(Constant.FACEBOOK_OAUTH_URL),
                    redirectUrl: new Uri(Constant.FACEBOOK_LOGIN_SUCCESS_URL));

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
                    var accessInfo = await NetworkHelper.GetFacebookUserInfoByToken(accessToken);

                    if (accessInfo == null)
                    {
                        await DefaultApp.DisplayErrorDialog(GlobleMessages.AccessFacebookAccountFailed);
                        DefaultApp.NavigateToPage(new LoginPage());
                    }
                    else
                    {
                        eventArgs.Account.Username = accessInfo.Email;
                        eventArgs.Account.Properties.Add("id", accessInfo.Id);
                        eventArgs.Account.Properties.Add("full_name", accessInfo.Name);
                        eventArgs.Account.Properties.Add("avatar_url", accessInfo.AvatarUrl);

                        await CachingHelper.CleanAccountStorage(GlobleSetting.FacebookAccountDomain);
                        var accountStorage = AccountStore.Create();
                        accountStorage.Save(eventArgs.Account, GlobleSetting.FacebookAccountDomain);

                        await DefaultApp.RegisterAndVerifyAccount(accessToken, accessInfo, SocialDomain.Facebook);
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