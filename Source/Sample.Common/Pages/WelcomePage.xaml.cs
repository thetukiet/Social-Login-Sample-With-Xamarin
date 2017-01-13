using System;

using Sample.Common.Entities;
using Sample.Common.Enums;
using Sample.Common.Helpers;
using Xamarin.Forms;

namespace Sample.Common.Pages
{
    public partial class WelcomePage : ContentPage
    {
        public SocialAccountInfo AccountInfo { set; get; }
        public SocialDomain FromDomain { set; get; }

        public readonly string[] ProfileBackgroundNames =
        {
            "Image_ScreenWall.jpg",
            "Image_ScreenWall11.jpg",
            "Image_ScreenWall12.jpg",
            "Image_ScreenWall13.jpg",
            "Image_ScreenWall14.jpg",
            "Image_ScreenWall15.jpg",
            "Image_ScreenWall2.jpg",
            "Image_ScreenWall3.jpg",
            "Image_ScreenWall4.jpg",
            "Image_ScreenWall5.jpg",
            "Image_ScreenWall6.jpg",
            "Image_ScreenWall9.jpg",
            "Image_ScreenWall16.jpg",
            "Image_ScreenWall17.jpg",
            "Image_ScreenWall18.jpg",
            "Image_ScreenWall19.jpg",
            "Image_ScreenWall20.jpg",
            "Image_ScreenWall21.jpg"
        };
        
        public WelcomePage(SocialAccountInfo accountInfo, SocialDomain fromDomain)
        {
            InitializeComponent();
            
            var rnd = new Random();
            var bgIndex = rnd.Next(ProfileBackgroundNames.Length);
            ProfileBackgourndImage.Source = ProfileBackgroundNames[bgIndex];

            LogoutButton.Clicked += LogoutButtonClickedEvent;
            FromDomain = fromDomain;
            AccountInfo = accountInfo;
            if (!String.IsNullOrWhiteSpace(accountInfo.AvatarUrl))
                ProfileImage.Source = new Uri(accountInfo.AvatarUrl);

            if (!String.IsNullOrWhiteSpace(accountInfo.Name))
                ProfileNameLabel.Text = accountInfo.Name;

            if (!String.IsNullOrWhiteSpace(accountInfo.Id))
                SocialIdLabel.Text = accountInfo.Id;

            if (!String.IsNullOrWhiteSpace(accountInfo.Email))
                EmaiLabel.Text = accountInfo.Email;
            SociaLabel.Text = fromDomain.ToString();
            
            DoAnimations();
        }

        private async void DoAnimations()
        {
            await ProfileImage.ScaleTo(1.5, 250);
            await ProfileImage.ScaleTo(1, 250);
        }

        private async void LogoutButtonClickedEvent(object sender, System.EventArgs e)
        {
            await DefaultApp.ShowPromb("Do you want to sign out? 👽", result =>
            {
                if (result == PrombAnswerResult.Yes)
                {
                    CachingHelper.DeleteLoginInfoFromCache();
                    DefaultApp.NavigateToPage(new LoginPage());
                }
            });
        }
    }
}
