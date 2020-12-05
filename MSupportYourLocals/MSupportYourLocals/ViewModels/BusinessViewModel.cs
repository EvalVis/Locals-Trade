using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;

namespace MSupportYourLocals.ViewModels
{
    public class BusinessViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();

        private ObservableCollection<Business> business;

        public ObservableCollection<Business> Business
        {
            get { return business; }
            set
            {
                business = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Business"));
            }
        }

        public BusinessViewModel()
        {
            GetBusinesses();
        }

        public async Task GetBusinesses()
        {
            ObservableCollection<Business> businesses = await businessService.GetBusinesses();
            foreach (var b in businesses)
            {
                System.Diagnostics.Debug.WriteLine(b.Header);
            }
        }

    }
}
