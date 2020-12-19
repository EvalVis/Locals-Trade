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
        private SearchView searchView;

        private string ownersSurname;
        private int searchIn;
        private string businessInfo;
        private bool[] checks = new bool[7];
        private DateTime openFrom;
        private DateTime openTo;

        public PageView(int currentPage, int totalPages, SearchView searchView)
        {
            InitializeComponent();
            this.currentPage = currentPage;
            this.totalPages = totalPages;
            PageEntry.Text = $"{currentPage}";
            TotalPagesLabel.Text = $"out of {totalPages}";
            this.searchView = searchView;
        }

        private async void Back(object sender, EventArgs e)
        {
            Parse();
            if (currentPage - 1 < 1) currentPage = totalPages + 1;
            else if (currentPage - 1 > totalPages) currentPage = 2;
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(currentPage - 1, ownersSurname, businessInfo, searchIn, checks, openFrom, openTo)));
        }

        private async void Go(object sender, EventArgs e)
        {
            Parse();
            int.TryParse(PageEntry.Text, out int targetPage);
            if (targetPage == 0) targetPage = 1;
            if (targetPage < 1) targetPage = 1;
            else if (targetPage > totalPages) targetPage = totalPages;
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(targetPage, ownersSurname, businessInfo, searchIn, checks, openFrom, openTo)));
        }

        private async void Forward(object sender, EventArgs e)
        {
            Parse();
            if (currentPage + 1 < 1) currentPage = totalPages - 1;
            else if (currentPage + 1 > totalPages) currentPage = 0;
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(currentPage + 1, ownersSurname, businessInfo, searchIn, checks, openFrom, openTo)));
        }

        private void Parse()
        {
            ownersSurname = searchView.GetItems<string>(0) as string;
            businessInfo = searchView.GetItems<string>(1) as string;
            int.TryParse(searchView.GetItems<int>(2).ToString(), out searchIn);
            bool.TryParse(searchView.GetItems<bool>(3).ToString(), out checks[0]);
            bool.TryParse(searchView.GetItems<bool>(4).ToString(), out checks[1]);
            bool.TryParse(searchView.GetItems<bool>(5).ToString(), out checks[2]);
            bool.TryParse(searchView.GetItems<bool>(6).ToString(), out checks[3]);
            bool.TryParse(searchView.GetItems<bool>(7).ToString(), out checks[4]);
            bool.TryParse(searchView.GetItems<bool>(8).ToString(), out checks[5]);
            bool.TryParse(searchView.GetItems<bool>(9).ToString(), out checks[6]);
            DateTime.TryParse(searchView.GetItems<DateTime>(10).ToString(), out openFrom);
            DateTime.TryParse(searchView.GetItems<DateTime>(11).ToString(), out openTo);
            if (openFrom.Equals(openTo))
            {
                openFrom = new DateTime(2020, 10, 10, 7, 0, 0);
                openTo = new DateTime(2020, 10, 10, 18, 30, 0);
            }
        }

    }
}
