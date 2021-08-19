using System;
using ContentSheet;
using Rg.Plugins.Popup.Enums;
using Xamarin.Forms;

namespace ContentSheetSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void Popup_Clicked(object sender, EventArgs e)
        {
            // Open a Normal PopupPage
            await ContentSheetNavigator.ShowSheet(CountrySheet());
        }

        async void TopSheet_Clicked(object sender, EventArgs e)
        {
            var sheet = CountrySheet(MoveAnimationOptions.Top);
            sheet.CloseWhenBackgroundIsClicked = false;
            sheet.HasSystemPadding = false;
            await ContentSheetNavigator.ShowSheet(sheet);
        }

        async void LeftSheet_Clicked(object sender, EventArgs e)
        {
            await ContentSheetNavigator.ShowSheet(CountrySheet(MoveAnimationOptions.Left));
        }

        async void RightSheet_Clicked(object sender, EventArgs e)
        {
            var sheet = CountrySheet(MoveAnimationOptions.Right);
            sheet.LightboxBackgroundColor = Color.FromRgba(150, 150, 150, 100);
            sheet.HasSystemPadding = false;
            await ContentSheetNavigator.ShowSheet(sheet);
        }

        async void BottomSheet_Clicked(object sender, EventArgs e)
        {
            var sheet = CountrySheet(MoveAnimationOptions.Bottom);
            sheet.LightboxBackgroundColor = Color.Transparent;
            sheet.HasSystemPadding = true;
            await ContentSheetNavigator.ShowSheet(sheet);
        }

        private CountrySelectionSheet CountrySheet(MoveAnimationOptions direction = MoveAnimationOptions.Center)
        {
            var sheet = new CountrySelectionSheet();
            sheet.Direction = direction;
            return sheet;
        }
    }
}