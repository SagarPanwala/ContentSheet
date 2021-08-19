using System;
using System.Windows.Input;
using ContentSheet;
using ContentSheet.Control;
using Xamarin.Forms;

namespace ContentSheetSample.ViewModels
{
    public class CountrySelectionSheetViewModel : BaseViewModel
    {
        public CountrySelectionSheetViewModel()
        {
            Title = "Select Monkey";

            ReadCommand = new Command(async () =>
            {
                var navigation = App.Current.MainPage;
                await navigation.DisplayAlert(SelectedMonkey, $"You have {Age} year old Monkey", "Ok");
            });

            UpdateColorCommand = new Command(() =>
            {
                Random r = new Random();
                Color color = Color.FromRgba(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), r.Next(100, 150));
                ContentSheetNavigator.ContentSheetView.LightboxBackgroundColor = color;
            });

            DoneCommand = new Command(async () =>
            {
                await ContentSheetNavigator.HideSheet();
            });
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        private string _age;
        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                NotifyPropertyChanged();
            }
        }

        public string _selectedMonkey;
        public string SelectedMonkey
        {
            get { return _selectedMonkey; }
            set
            {
                _selectedMonkey = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ReadCommand { get; private set; }

        public ICommand UpdateColorCommand { get; private set; }

        public ICommand DoneCommand { get; private set; }
    }
}