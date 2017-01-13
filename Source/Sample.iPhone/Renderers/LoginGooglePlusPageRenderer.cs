using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Sample.Common;
using Sample.Common.Entities;
using Sample.Common.Helpers;
using Sample.Common.Pages;

[assembly: ExportRenderer(typeof(LoginFacebookPage), typeof(Sample.iOS.Renderers.LoginFacebookPageRenderer))]
namespace Sample.iOS.Renderers
{    
    public class LoginFacebookPageRenderer : PageRenderer
    {
        private bool _done;

        public override async void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (_done)
                return;

            var realControl = Element as LoginFacebookPage;
            if (realControl == null)
                return;

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
                    clientId: Constant.FACEBOOK_ID,
                    scope: Constant.FACEBOOK_ACCESS_BASE_SCOPE,
                    authorizeUrl: new Uri(Constant.FACEBOOK_OAUTH_URL),
                    redirectUrl: new Uri(Constant.FACEBOOK_LOGIN_SUCCESS_URL));

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
                    var accessInfo = await NetworkHelper.GetFacebookUserInfoByToken(accessToken);

                    if (accessInfo == null)
                    {
                        await DefaultApp.DisplayErrorDialog(GlobleMessages.AccessFacebookAccountFailed);
                        DefaultApp.NavigateToPage(new LoginPage());
                    }
                    else
                    {
                        eventArgs.Account.Username = accessInfo.Email;
                        eventArgs.Account.Properties.Add("id",accessInfo.Id);
                        eventArgs.Account.Properties.Add("full_name", accessInfo.Name);
                        eventArgs.Account.Properties.Add("avatar_url", accessInfo.AvatarUrl);

                        await CachingHelper.CleanAccountStorage(GlobleSetting.FacebookAccountDomain);
                        AccountStore.Create().Save(eventArgs.Account, GlobleSetting.FacebookAccountDomain);

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