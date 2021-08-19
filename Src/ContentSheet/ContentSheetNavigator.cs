using System.Threading.Tasks;
using ContentSheet.Control;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ContentSheet
{
    public static class ContentSheetNavigator
    {
        private static int StatusBarHeight => Device.RuntimePlatform == Device.Android ? 20 : 40;

        private static IPopupNavigation PopupNavigator => PopupNavigation.Instance;

        private static ContentSheetPopup CurrentSheet;

        public static ContentSheetView ContentSheetView => (ContentSheetView)CurrentSheet?.Content;

        private static bool _isInitialize;
        public static void Init()
        {
            if (!_isInitialize)
            {
                _isInitialize = true;
                PopupNavigator.Popping += PopupNavigator_Popping;
            }
        }

        public static async Task ShowSheet(ContentSheetView contentSheetView)
        {
            Init();

            ContentSheetPopup contentSheetPopup = new ContentSheetPopup();
            CurrentSheet = contentSheetPopup;

            MoveAnimationOptions direction = contentSheetView.Direction;

            contentSheetPopup.BackgroundColor = contentSheetView.LightboxBackgroundColor;
            contentSheetPopup.Animation = ApplyAnimation(direction);
            contentSheetPopup.HasSystemPadding = contentSheetView.HasSystemPadding;
            contentSheetPopup.CloseWhenBackgroundIsClicked = contentSheetView.CloseWhenBackgroundIsClicked;

            if (direction == MoveAnimationOptions.Top)
            {
                double statusBarSpacing = Device.RuntimePlatform == Device.Android ? -22 : -22;
                contentSheetView.Margin = new Thickness(contentSheetView.Margin.Left, contentSheetView.Margin.Top + statusBarSpacing, contentSheetView.Margin.Right, contentSheetView.Margin.Bottom);
                contentSheetView.Padding = new Thickness(contentSheetView.Padding.Left, contentSheetView.Padding.Top + StatusBarHeight, contentSheetView.Padding.Right, contentSheetView.Padding.Bottom);
            }

            contentSheetPopup.Content = contentSheetView;

            contentSheetView.OnAppearing();
            contentSheetView.PropertyChanged += ContentSheetView_PropertyChanged;
            await PopupNavigator.PushAsync(contentSheetPopup);
        }

        public static async Task HideSheet()
        {
            await PopupNavigator.PopAsync();
        }

        private static IPopupAnimation ApplyAnimation(MoveAnimationOptions direction)
        {
            IPopupAnimation animation;
            if (direction == MoveAnimationOptions.Center)
            {
                ScaleAnimation scaleAnimation = new ScaleAnimation(direction, direction);
                scaleAnimation.ScaleIn = 1.2;
                scaleAnimation.ScaleOut = 0.8;
                scaleAnimation.EasingIn = Easing.SinOut;
                scaleAnimation.EasingOut = Easing.SinIn;
                animation = scaleAnimation;
            }
            else
            {
                animation = new MoveAnimation(direction, direction);
            }

            return animation;
        }

        private static void ContentSheetView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ContentSheetView contentSheetView = (ContentSheetView)sender;
            if (e.PropertyName == ContentSheetView.LightboxBackgroundColorProperty.PropertyName)
            {
                if (CurrentSheet != null && contentSheetView != null)
                {
                    CurrentSheet.BackgroundColor = contentSheetView.LightboxBackgroundColor;
                }
            }
        }

        private static void PopupNavigator_Popping(object sender, Rg.Plugins.Popup.Events.PopupNavigationEventArgs e)
        {
            if (CurrentSheet != null)
            {
                ContentSheetView?.OnDisappearing();
                CurrentSheet.PropertyChanged -= ContentSheetView_PropertyChanged;
            }
        }
    }
}