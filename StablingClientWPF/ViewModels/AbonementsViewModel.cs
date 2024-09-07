using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class AbonementsViewModel : BaseViewModel
    {
        private AbonementsHttpClient _abonementsHttpClient;
        private ClientsHttpClient _clientsHttpClient;
        public AbonementsViewModel(AbonementsHttpClient abonementsHttpClient,
            ClientsHttpClient clientsHttpClient)
        {
            _abonementsHttpClient = abonementsHttpClient;
            _clientsHttpClient = clientsHttpClient;
            GetAbonements();
        }

        private ObservableCollection<AbonementForShow> _ActiveAbonements;
        public ObservableCollection<AbonementForShow> ActiveAbonements
        {
            get { return _ActiveAbonements; }
            set { _ActiveAbonements = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AbonementForShow> _InactiveAbonements;
        public ObservableCollection<AbonementForShow> InactiveAbonements
        {
            get { return _InactiveAbonements; }
            set { _InactiveAbonements = value; OnPropertyChanged(); }
        }

        public AsyncDelegateCommand GetAbonementsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await GetAbonements(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetAbonements()
        {
            ActiveAbonements = new ObservableCollection<AbonementForShow>(
                await _abonementsHttpClient.GetForShowByAvailabilityAsync(true));
            InactiveAbonements = new ObservableCollection<AbonementForShow>(
                await _abonementsHttpClient.GetForShowByAvailabilityAsync(false));
        }

        public AsyncDelegateCommand CreateAbonementCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await CreateAbonement(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task CreateAbonement()
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementCreationForm(
                    new AbonementCreationFormViewModel(
                    new Abonement()
                    {
                        OpenDateTime = DateTime.Now,
                        IsAvailable = true,
                    },
                    await _clientsHttpClient.GetByAvailabilityAsync(true),
                    await _abonementsHttpClient.GetTypesAsync())), ROOT_IDENTIFIER);
            if (result != null)
            {
                await _abonementsHttpClient.CreateAsync((Abonement)result);
                await GetAbonements();
            }
        }


        public AsyncDelegateCommand ChangeAvailabilityCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await ChangeAvailability((int)o); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangeAvailability(int abonementId)
        {
            AbonementForShow abonementForWork = ActiveAbonements.FirstOrDefault(o => o.AbonementId == abonementId);
            if (abonementForWork == null)
                abonementForWork = InactiveAbonements.Where(o => o.AbonementId == abonementId).First();
            await _abonementsHttpClient.ChangeAvailabilityAsync(abonementId);
            if (abonementForWork.IsAvailable)
            {
                ActiveAbonements.Remove(abonementForWork);
                InactiveAbonements.Add(abonementForWork);
            }
            else
            {
                InactiveAbonements.Remove(abonementForWork);
                ActiveAbonements.Add(abonementForWork);
            }
        }
    }
}
