using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;

namespace MSupportYourLocals.ViewModels
{
    public class BusinessesViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();

        private ObservableCollection<Business> businesses;
        public int TotalPages;
        public int CurrentPage;
        public string OwnersSurname;
        public string BusinessInfo;
        public int SearchIn;
        public bool[] WeekdaySelected = new bool[7];
        public DateTime OpenFrom;
        public DateTime OpenTo;

        public ObservableCollection<Business> Businesses
        {
            get { return businesses; }
            set
            {
                businesses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Business"));
            }
        }

        public BusinessesViewModel(int currentPage)
        {
            CurrentPage = currentPage;
            OpenFrom = new DateTime(2020, 10, 10, 7, 0, 0);
            OpenTo = new DateTime(2020, 10, 10, 18, 30, 0);
            for (int i = 0; i < 7; i++) WeekdaySelected[i] = true;
            Task.Run(async () => await GetBusinesses()).Wait();
        }

        public BusinessesViewModel(int currentPage, string ownersSurname, string businessInfo, int searchIn,
            bool[] weekdaySelected, DateTime openFrom, DateTime openTo)
        {
            CurrentPage = currentPage;
            OwnersSurname = ownersSurname;
            businessInfo = BusinessInfo;
            SearchIn = searchIn;
            WeekdaySelected = weekdaySelected;
            OpenFrom = openFrom;
            OpenTo = openTo;
            Task.Run(async () => await GetFilteredBusinesses()).Wait();
        }

        public async Task GetFilteredBusinesses()
        {
            PageBusiness pageBusiness = await businessService.GetFilteredBusinesses(OwnersSurname, BusinessInfo, SearchIn, WeekdaySelected, OpenFrom, OpenTo, CurrentPage);
            businesses = pageBusiness?.Businesses ?? new ObservableCollection<Business>();
            businesses = SortByWeekday.Sort(businesses);
            TotalPages = pageBusiness?.TotalPages ?? 1;
        }


        public async Task GetBusinesses()
        {
            PageBusiness pageBusiness = await businessService.GetBusinesses(CurrentPage);
            businesses = pageBusiness?.Businesses ?? new ObservableCollection<Business>();
            businesses = SortByWeekday.Sort(businesses);
            TotalPages = pageBusiness?.TotalPages ?? 1;
        }

        public void ConcatAllProducts()
        {
            foreach (var b in Businesses)
            {
                b?.ConcatProducts();
            }
        }

    }
}
