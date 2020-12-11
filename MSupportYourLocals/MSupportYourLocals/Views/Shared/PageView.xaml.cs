using System;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageView : ContentView
    {

        private int currentPage;
        private int totalPages;

        public PageView(int currentPage, int totalPages)
        {
            InitializeComponent();
            this.currentPage = currentPage;
            this.totalPages = totalPages;
            PageEntry.Text = $"{currentPage}";
            TotalPagesLabel.Text = $"out of {totalPages}";
        }

        private async void Back(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(currentPage - 1)));
        }

        private async void Go(object sender, EventArgs e)
        {
            int.TryParse(PageEntry.Text, out int page);
            if (page == 0) page = 1;
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(page)));
        }

        private async void Forward(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(currentPage + 1)));
        }

    }
}
