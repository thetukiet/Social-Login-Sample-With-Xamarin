using System;
using System.Linq;
using Sample.Common.Entities;
using Sample.Common.Enums;
using Sample.Common.Helpers;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Sample.Common.Pages
{
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            Indicator.BackgroundColor = Color.FromRgba(30, 30, 30, 20);
            IndicatorPanel.BackgroundColor = Color.FromRgba(30, 30, 30, 180);
            
            IndicatorPanel.IsVisible = false;
            Indicator.IsVisible = false;
            Indicator.IsRunning = true;

            FacebookSubmitButton.Clicked += FacebookSubmitButtonClickedEvent;
            GooglePlusSubmitButton.Clicked += GooglePlusSubmitButtonClickedEvent;
        }
        
/*
        private async void SubmitButtonClickedEvent(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(EmailField.Text))
            {
                await DefaultApp.DisplayDialog(LabelResources.LoginFailTitleMessage, "Please input email address");
                return;
            }

            if (!ValidationHelper.IsValidEmailaddress(EmailField.Text))
            {
                await DefaultApp.DisplayDialog(LabelResources.LoginFailTitleMessage, "Your email address is not valid");
                return;
            }

            if (String.IsNullOrWhiteSpace(FirstNameField.Text))
            {
                await DefaultApp.DisplayDialog(LabelResources.LoginFailTitleMessage, "Please input First Name");
                return;
            }

            if (String.IsNullOrWhiteSpace(LastNameField.Text))
            {
                await DefaultApp.DisplayDialog(LabelResources.LoginFailTitleMessage, "Please input Last Name");
                return;
            }

            IndicatorPanel.IsVisible = true;
            Indicator.IsVisible = true;
            await SubmitLogIn();
        }*/



        private async void FacebookSubmitButtonClickedEvent(object sender, EventArgs e)
        {
            if (!NetworkHelper.IsNetworkAvailable())
            {
                await DisplayAlert(GlobleMessages.LoginFailTitle, GlobleMessages.NetworkNotAvailable, "OK");
                return;
            }

            IndicatorPanel.IsVisible = true;
            Indicator.IsVisible = true;

            var accounts = AccountStore.Create().FindAccountsForService(GlobleSetting.FacebookAccountDomain);
            var account = accounts.FirstOrDefault();
            if (account == null)
            {
                // Go input username & password
                await DefaultApp.AddPage(new LoginFacebookPage());
            }
            else
            {
                if (account.Properties.ContainsKey("id"))
                {
                    var askMessage = string.Format("There is an existing account with email [{0}]. " +
                                                   "Do you want to continue with it?", account.Username);
                    await DefaultApp.ShowPromb(askMessage, "Icon_Facebook_Large.png", async result =>
                    {
                        if(result == PrombAnswerResult.Yes)
                        {
                            var accessToken = account.Properties["access_token"];
                            var accountInfo = new SocialAccountInfo
                            {
                                Id = account.Properties["id"],
                                Email = account.Username,
                                Name = account.Properties["full_name"],
                                AvatarUrl = account.Properties["avatar_url"]
                            };
                            await DefaultApp.RegisterAndVerifyAccount(accessToken, accountInfo, SocialDomain.Facebook);
                        }
                        else
                        {
                            await DefaultApp.AddPage(new LoginFacebookPage());
                        }
                    });
                }
                else
                {
                    await DefaultApp.AddPage(new LoginFacebookPage());
                }
            }

            IndicatorPanel.IsVisible = false;
            Indicator.IsVisible = false;
        }

        private async void GooglePlusSubmitButtonClickedEvent(object sender, EventArgs e)
        {
            if (!NetworkHelper.IsNetworkAvailable())
            {
                await DisplayAlert(GlobleMessages.LoginFailTitle, GlobleMessages.NetworkNotAvailable, "OK");
                return;
            }

            IndicatorPanel.IsVisible = true;
            Indicator.IsVisible = true;

            var accounts = AccountStore.Create().FindAccountsForService(GlobleSetting.GooglePlusAccountDomain);
            var account = accounts.FirstOrDefault();
            if (account == null)
            {
                // Go input username & password
                await DefaultApp.AddPage(new LoginGooglePlusPage());
            }
            else
            {
                if (account.Properties.ContainsKey("id"))
                {
                    var askMessage = string.Format("There is an existing account with email [{0}]. " +
                                                   "Do you want to continue with it?", account.Username);                    
                    await DefaultApp.ShowPromb(askMessage, "Icon_GooglePlus_Large.png", async result =>
                    {
                        if (result == PrombAnswerResult.Yes)
                        {
                            var accessToken = account.Properties["access_token"];
                            var accountInfo = new SocialAccountInfo
                            {
                                Id = account.Properties["id"],
                                Email = account.Username,
                                Name = account.Properties["full_name"],
                                AvatarUrl = account.Properties["avatar_url"]
                            };
                            await DefaultApp.RegisterAndVerifyAccount(accessToken, accountInfo, SocialDomain.Google);
                        }
                        else
                        {
                            await DefaultApp.AddPage(new LoginGooglePlusPage());
                        }
                    });                    
                }
                else
                {
                    await DefaultApp.AddPage(new LoginFacebookPage());
                }
            }

            IndicatorPanel.IsVisible = false;
            Indicator.IsVisible = false;
        }
    }
}
