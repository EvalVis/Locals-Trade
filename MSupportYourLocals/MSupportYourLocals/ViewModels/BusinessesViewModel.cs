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
            Task.Run(async () => await GetBusinesses()).Wait();
        }

        public BusinessesViewModel(int currentPage, string ownersSurname, string businessInfo, int searchIn,
            bool[] weekdaySelected, DateTime openFrom, DateTime openTo)
        {
            CurrentPage = currentPage;
            Task.Run(async () => await GetFilteredBusinesses(ownersSurname, businessInfo, searchIn, weekdaySelected, openFrom, openTo)).Wait();
        }

        public async Task GetFilteredBusinesses(string ownersSurname, string businessInfo, int searchIn,
            bool[] weekdaySelected, DateTime openFrom, DateTime openTo)
        {
            PageBusiness pageBusiness = await businessService.GetFilteredBusinesses(ownersSurname, businessInfo, searchIn, weekdaySelected, openFrom, openTo, CurrentPage);
            businesses = pageBusiness?.Businesses;
            businesses = SortByWeekday.Sort(businesses);
            TotalPages = pageBusiness?.TotalPages ?? 1;
        }


        public async Task GetBusinesses()
        {
            PageBusiness pageBusiness = await businessService.GetBusinesses(CurrentPage);
            businesses = pageBusiness?.Businesses;
            businesses = SortByWeekday.Sort(businesses);
            TotalPages = pageBusiness?.TotalPages ?? 1;
        }

    }
}
