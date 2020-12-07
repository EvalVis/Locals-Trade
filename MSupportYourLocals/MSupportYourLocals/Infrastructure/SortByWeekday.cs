using System.Collections.ObjectModel;
using System.Linq;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Infrastructure
{
    public static class SortByWeekday
    {

        public static ObservableCollection<Business> Sort(ObservableCollection<Business> businesses)
        {
            foreach (var b in businesses)
            {
                if (b.Workdays != null)
                {
                    b.Workdays = new ObservableCollection<Workday>(b.Workdays.OrderBy(w => w.Weekday));
                }
            }
            return businesses;
        }


    }
}
