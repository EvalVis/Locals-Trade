using System;
using System.Collections.Generic;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchView : StackLayout
    {

        private List<string> searchOptions = new List<string> {"Search in header", "Search in description", "Search in header and description"};

        public SearchView()
        {
            InitializeComponent();
            SearchOptionsPicker.ItemsSource = searchOptions;
            SearchOptionsPicker.SelectedItem = searchOptions[0];
        }

        private async void Search(object sender, EventArgs e)
        {
            string ownersSurname = OwnerEntry.Text;
            string businessInfo = SearchEntry.Text;
            int searchIn = SearchOptionsPicker.SelectedIndex;
            DateTime openFrom = new DateTime(2020, 01, 01, FromPicker.Time.Hours, FromPicker.Time.Minutes, 0);
            DateTime openTo = new DateTime(2020, 01, 01, ToPicker.Time.Hours, ToPicker.Time.Minutes, 0);
            bool[] checks =  {Monday.IsChecked, Tuesday.IsChecked, Wednesday.IsChecked, Thursday.IsChecked, Friday.IsChecked, Saturday.IsChecked, Sunday.IsChecked};
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(1, ownersSurname, businessInfo,
                searchIn, checks, openFrom, openTo)));
        }


    }
}
