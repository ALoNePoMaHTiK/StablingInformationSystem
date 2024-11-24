using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class ClientsViewModel : BaseViewModel
    {
        private ObservableCollection<Client> _ActiveClients;
        public ObservableCollection<Client> ActiveClients
        {
            get { return _ActiveClients; }
            set { _ActiveClients = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Client> _InactiveClients;
        public ObservableCollection<Client> InactiveClients
        {
            get { return _InactiveClients; }
            set { _InactiveClients = value; OnPropertyChanged(); }
        }

        private readonly ClientsHttpClient _clientsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;
        private readonly DialogManager _dialogManager;
        public ClientsViewModel(Mediator mediator,
            DialogManager dialogManager,
            ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient)
        {
            _dialogManager = dialogManager;
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            GetClients();

        }

        private async Task GetClients()
        {
            ActiveClients = new ObservableCollection<Client>(
                await _clientsHttpClient.GetByAvailabilityAsync(true));
            InactiveClients = new ObservableCollection<Client>(
                await _clientsHttpClient.GetByAvailabilityAsync(false));
        }

        #region Основные операции

        public DelegateCommand CreateClientCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CreateClient();
                });
            }
        }
        private async void CreateClient()
        {
            Client? newClient = await _dialogManager.OpenClientCreateDialog(
                await _trainersHttpClient.GetAllAsync());
            if (newClient != null)
            {
                newClient = await _clientsHttpClient.CreateAsync(newClient);
                ActiveClients.Add(newClient);
            }
        }

        public DelegateCommand UpdateClientCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    UpdateClient((int)o);
                });
            }
        }
        private async void UpdateClient(int clientId)
        {
            Client? updatedClient = await _dialogManager.OpenClientUpdateDialog(
                await _trainersHttpClient.GetAllAsync(),
                await _clientsHttpClient.GetAsync(clientId));
            if (updatedClient != null)
            {
                await _clientsHttpClient.UpdateAsync(updatedClient);
                Client? oldClient = ActiveClients.Where(c =>
                    c.ClientId == updatedClient.ClientId).FirstOrDefault();
                if (oldClient != null)
                {
                    ActiveClients.Remove(oldClient);
                    ActiveClients.Add(updatedClient);
                }
                else
                {
                    oldClient = InactiveClients.Where(c =>
                        c.ClientId == updatedClient.ClientId).First();
                    InactiveClients.Remove(oldClient);
                    InactiveClients.Add(updatedClient);
                }
            }
        }

        public AsyncDelegateCommand ChangeClientAvailabilityCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ChangeClientAvailability((int)o);
                },
                    ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangeClientAvailability(int id)
        {
            Client? clientForWork = ActiveClients.FirstOrDefault(o => o.ClientId == id);
            if (clientForWork == null)
                clientForWork = InactiveClients.Where(o => o.ClientId == id).First();
            await _clientsHttpClient.ChangeAvailabilityAsync(id);
            if (clientForWork.IsAvailable)
            {
                ActiveClients.Remove(clientForWork);
                InactiveClients.Add(clientForWork);
            }
            else
            {
                InactiveClients.Remove(clientForWork);
                ActiveClients.Add(clientForWork);
            }
        }
        #endregion
    }
}
