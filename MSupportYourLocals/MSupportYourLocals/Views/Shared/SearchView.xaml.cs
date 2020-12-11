using System;
using System.Collections.Generic;
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

        }


    }
}
