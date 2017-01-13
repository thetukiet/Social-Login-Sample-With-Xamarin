using System;
using Sample.Common.Enums;
using Xamarin.Forms;

namespace Sample.Common.Pages
{
    public delegate void PrombAnswer(PrombAnswerResult result);    

    public partial class PrombPage : ContentPage
	{
        public event PrombAnswer PrombAnswerEvent;

        public PrombPage(string content)
		{
			InitializeComponent ();
            if(!String.IsNullOrWhiteSpace(content))
		        QuestionContentLabel.Text = content;

            YesButton.Clicked += YesButtonClicked;
            NoButton.Clicked += NoButtonClicked;
            
		    //DoAnimations();
		}

        public PrombPage(string content, string iconResource) : this(content)
        {
            if (!String.IsNullOrWhiteSpace(iconResource))
                MainIconImage.Source = iconResource;
        }

        private void NoButtonClicked(object sender, System.EventArgs e)
        {
            PrombAnswerEvent?.Invoke(PrombAnswerResult.No);
        }

        private void YesButtonClicked(object sender, System.EventArgs e)
        {
            PrombAnswerEvent?.Invoke(PrombAnswerResult.Yes);
        }

        private async void DoAnimations()
	    {
            var btnNoBounds = NoButton.Bounds;
            var btnYesBounds = YesButton.Bounds;
            btnNoBounds.X = MainLayout.Width + 10;
            btnYesBounds.X = 0 - YesButton.Width;

            NoButton.LayoutTo(btnNoBounds, 0u, Easing.Linear);
            YesButton.LayoutTo(btnYesBounds, 0u, Easing.Linear);

            btnNoBounds.X = MainLayout.Width - NoButton.Width;
	        btnYesBounds.X = 0;
	        await NoButton.LayoutTo(btnNoBounds, 200u, Easing.Linear);
	        await YesButton.LayoutTo(btnYesBounds, 200u, Easing.Linear);
	    }

	    protected virtual void OnPrombAnswerEvent(PrombAnswerResult result)
	    {
	        PrombAnswerEvent?.Invoke(result);
	    }
	}
}
