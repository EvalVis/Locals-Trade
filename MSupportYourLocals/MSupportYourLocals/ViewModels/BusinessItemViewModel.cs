using System;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.ViewModels
{
    public class BusinessItemViewModel : ViewModel
    {
        public BusinessItem Item { get; private set; }
        public event EventHandler ItemStatusChanged;

        public BusinessItemViewModel(BusinessItem item) => Item = item;

    }
}
