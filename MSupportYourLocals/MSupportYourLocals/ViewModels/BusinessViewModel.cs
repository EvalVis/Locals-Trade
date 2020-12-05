using System.Collections.ObjectModel;
using System.ComponentModel;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.ViewModels
{
    public class BusinessViewModel : INotifyPropertyChanged
    {

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
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
