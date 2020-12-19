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

        private List<string> searchOptions = new List<string> { "Search in header", "Search in description", "Search in header and description" };

        public SearchView(string ownersSurname, string businessInfo, int searchIn, bool[] checks, DateTime? openFrom, DateTime? openTo)
        {
            InitializeComponent();
            Setup(ownersSurname, businessInfo, searchIn, checks, openFrom, openTo);
        }

        private void Setup(string ownersSurname, string businessInfo, int searchIn, bool[] checks, DateTime? openFrom, DateTime? openTo)
        {
            OwnerEntry.Text = ownersSurname;
            SearchEntry.Text = businessInfo;
            SearchOptionsPicker.ItemsSource = searchOptions;
            SearchOptionsPicker.SelectedItem = searchOptions[searchIn];
            Monday.IsChecked = checks?[0] ?? true;
            Tuesday.IsChecked = checks?[1] ?? true;
            Wednesday.IsChecked = checks?[2] ?? true;
            Thursday.IsChecked = checks?[3] ?? true;
            Friday.IsChecked = checks?[4] ?? true;
            Saturday.IsChecked = checks?[5] ?? true;
            Sunday.IsChecked = checks?[6] ?? true;
            FromPicker.Time = openFrom?.TimeOfDay ?? new TimeSpan(7, 0, 0);
            ToPicker.Time = openTo?.TimeOfDay ?? new TimeSpan(18, 30, 0);
        }

        private async void Search(object sender, EventArgs e)
        {
            string ownersSurname = OwnerEntry.Text;
            string businessInfo = SearchEntry.Text;
            int searchIn = SearchOptionsPicker.SelectedIndex;
            DateTime openFrom = new DateTime(2020, 01, 01, FromPicker.Time.Hours, FromPicker.Time.Minutes, 0);
            DateTime openTo = new DateTime(2020, 01, 01, ToPicker.Time.Hours, ToPicker.Time.Minutes, 0);
            bool[] checks = { Monday.IsChecked, Tuesday.IsChecked, Wednesday.IsChecked, Thursday.IsChecked, Friday.IsChecked, Saturday.IsChecked, Sunday.IsChecked };
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(1, ownersSurname, businessInfo,
                searchIn, checks, openFrom, openTo)));
        }

        public object GetItems<T>(int index)
        {
            switch (index)
            {
                case 0:
                    return OwnerEntry.Text;
                case 1:
                    return SearchEntry.Text;
                case 2:
                    return SearchOptionsPicker.SelectedIndex;
                case 3:
                    return Monday.IsChecked;
                case 4:
                    return Tuesday.IsChecked;
                case 5:
                    return Wednesday.IsChecked;
                case 6:
                    return Thursday.IsChecked;
                case 7:
                    return Friday.IsChecked;
                case 8:
                    return Saturday.IsChecked;
                case 9:
                    return Sunday.IsChecked;
                case 10:
                    return FromPicker.Time;
                case 11:
                    return ToPicker.Time;
                default:
                    return null;
            }
        }


    }
}
