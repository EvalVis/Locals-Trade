using MSupportYourLocals.Models.Repositories;

namespace MSupportYourLocals.ViewModels
{
    class ItemViewModel : ViewModel
    {
        private readonly ILocalRepository repository;

        public ItemViewModel(ILocalRepository repo)
        {
            repository = repo;
        }

    }
}
