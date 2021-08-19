using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Enums;
using Xamarin.Forms;

namespace ContentSheet.Control
{
    public partial class ContentSheetView : Frame
    {
        public static readonly BindableProperty DirectionProperty = BindableProperty.Create(nameof(Direction), typeof(MoveAnimationOptions), typeof(ContentSheetView), MoveAnimationOptions.Center);

        public MoveAnimationOptions Direction
        {
            get => (MoveAnimationOptions)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public static readonly BindableProperty LightboxBackgroundColorProperty = BindableProperty.Create(nameof(LightboxBackgroundColor), typeof(Color), typeof(ContentSheetView), Color.FromHex("#80000000"));

        public Color LightboxBackgroundColor
        {
            get => (Color)GetValue(LightboxBackgroundColorProperty);
            set => SetValue(LightboxBackgroundColorProperty, value);
        }

        public static readonly BindableProperty HasSystemPaddingProperty = BindableProperty.Create(nameof(HasSystemPadding), typeof(bool), typeof(ContentSheetView), true);

        public bool HasSystemPadding
        {
            get => (bool)GetValue(HasSystemPaddingProperty);
            set => SetValue(HasSystemPaddingProperty, value);
        }

        public static readonly BindableProperty CloseWhenBackgroundIsClickedpProperty = BindableProperty.Create(nameof(CloseWhenBackgroundIsClicked), typeof(bool), typeof(ContentSheetView), true);

        public bool CloseWhenBackgroundIsClicked
        {
            get => (bool)GetValue(CloseWhenBackgroundIsClickedpProperty);
            set => SetValue(CloseWhenBackgroundIsClickedpProperty, value);
        }

        private bool CanPanHorizontally = false;
        private bool CanPanVertically = false;
        private PanGestureRecognizer PanGesture;


        private double translateX;
        private double translateY;

        public ContentSheetView()
        {
            InitializeComponent();
            PanGesture = new PanGestureRecognizer();
        }

        internal void OnAppearing()
        {
            PanGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(PanGesture);
        }

        internal void OnDisappearing()
        {
            PanGesture.PanUpdated -= OnPanUpdated;
            GestureRecognizers.Remove(PanGesture);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Direction):
                    SetDirection(Direction);
                    break;
                default:
                    break;
            }
        }

        private void SetDirection(MoveAnimationOptions direction)
        {
            if (direction == MoveAnimationOptions.Bottom)
            {
                VerticalOptions = LayoutOptions.End;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (direction == MoveAnimationOptions.Top)
            {
                VerticalOptions = LayoutOptions.Start;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (direction == MoveAnimationOptions.Left)
            {
                VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.Start;
            }
            else if (direction == MoveAnimationOptions.Right)
            {
                VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.End;
            }
            else if (direction == MoveAnimationOptions.Center)
            {
                VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.Center;
            }

            if (direction == MoveAnimationOptions.Left || direction == MoveAnimationOptions.Right)
            {
                CanPanHorizontally = true;
            }
            else if (direction == MoveAnimationOptions.Top || direction == MoveAnimationOptions.Bottom)
            {
                CanPanVertically = true;
            }
        }

        private async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (CanPan(e.TotalX, e.TotalY))
                    {
                        // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                        if (CanPanHorizontally)
                        {
                            translateX = e.TotalX;
                            TranslationX = translateX;
                            //Content.TranslationX = Math.Max(Math.Min(0, e.TotalX), -Math.Abs(Content.Width - App.ScreenWidth));
                        }
                        else if (CanPanVertically)
                        {
                            translateY = e.TotalY;
                            TranslationY = translateY;
                            //Content.TranslationY =
                            //  Math.Max(Math.Min(0, e.TotalY), -Math.Abs(Content.Height - App.ScreenHeight));
                        }
                    }
                    break;
                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    if (PannedEnoughToClose())
                    {
                        await Dismiss();
                    }
                    else
                    {
                        await ResetPosition();
                    }
                    break;
            }
        }

        private bool CanPan(double x, double y)
        {
            if (Direction == MoveAnimationOptions.Bottom)
            {
                return y > 0;
            }
            else if (Direction == MoveAnimationOptions.Top)
            {
                return y < 0;
            }
            else if (Direction == MoveAnimationOptions.Left)
            {
                return x < 0;
            }
            else if (Direction == MoveAnimationOptions.Right)
            {
                return x > 0;
            }
            else
            {
                return false;
            }
        }

        private async Task ResetPosition()
        {
            await this.TranslateTo(0, 0);
        }

        private bool PannedEnoughToClose()
        {
            if (CanPanHorizontally)
            {
                return Math.Abs(translateX) > this.Width / 4;
            }
            else if (CanPanVertically)
            {
                return Math.Abs(translateY) > this.Height / 4;
            }
            return false;
        }

        private async Task Dismiss()
        {
            await ContentSheetNavigator.HideSheet();
        }
    }
}