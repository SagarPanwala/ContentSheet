using ContentSheet.Control;
using ContentSheetSample.ViewModels;

namespace ContentSheetSample
{
    public partial class CountrySelectionSheet : ContentSheetView
    {
        public CountrySelectionSheet()
        {
            InitializeComponent();
            BindingContext = new CountrySelectionSheetViewModel();
        }
    }
}