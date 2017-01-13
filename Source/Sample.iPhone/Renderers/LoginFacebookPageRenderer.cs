using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Sample.Common;
using Sample.Common.Entities;
using Sample.Common.Helpers;
using Sample.Common.Pages;

[assembly: ExportRenderer(typeof(LoginGooglePlusPage), typeof(Sample.iOS.Renderers.LoginGooglePlusPageRenderer))]
namespace Sample.iOS.Renderers
{    
    public class LoginGooglePlusPageRenderer : PageRenderer
    {
        private bool _done;

        public override async void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (_done)
                return;

            var realControl = Element as LoginGooglePlusPage;
            if (realControl == null)
                DefaultApp.NavigateToPage(new LoginPage());

            try
            {
                GoValidateNewInput();
                _done = true;
            }
            catch (Exception)
            {
                await DefaultApp.DisplayErrorDialog(GlobleMessages.CheckNetworkMessage);
                DefaultApp.NavigateToPage(new LoginPage());
            }
        }


        private void GoValidateNewInput()
        {
            var auth = new OAuth2Authenticator(
                    clientId: Constant.GOOGLEPLUS_ID,
                    clientSecret: Constant.GOOGLEPLUS_CLIENT_SECRET,
                    scope: Constant.GOOGLEPLUS_ACCESS_SCOPE,
                    authorizeUrl: new Uri(Constant.GOOGLEPLUS_OAUTH_URL),
                    redirectUrl: new Uri(Constant.GOOGLEPLUS_LOGIN_SUCCESS_URL),
                    accessTokenUrl: new Uri(Constant.GOOGLEPLUS_LOGIN_ACCESS_TOKEN_URL));

            auth.Completed += OnCompleteValidateAccount;
            PresentViewController(auth.GetUI(), true, null);
        }


        private async void OnCompleteValidateAccount(object sender, AuthenticatorCompletedEventArgs eventArgs)
        {
            DismissViewController(true, null);
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
                        eventArgs.Account.Properties.Add("id",accessInfo.Id);
                        eventArgs.Account.Properties.Add("full_name", accessInfo.Name);
                        eventArgs.Account.Properties.Add("avatar_url", accessInfo.AvatarUrl);

                        await CachingHelper.CleanAccountStorage(GlobleSetting.GooglePlusAccountDomain);
                        AccountStore.Create().Save(eventArgs.Account, GlobleSetting.GooglePlusAccountDomain);

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