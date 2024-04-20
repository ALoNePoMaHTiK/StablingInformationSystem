using StablingApiClient;
using StablingClientWPF.Commands;
using StablingClientWPF.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        private ClientsHttpClient _clientsHttpClient;
        private TrainersHttpClient _trainersHttpClient;
        public ClientsViewModel(ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient)
        {
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            GetClients();
        }

        public DelegateCommand OpenCreateClientCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    OpenCreateClient();
                });
            }
        }
        private void OpenCreateClient()
        {
            OpenModalWindow(new Client());
        }

        public DelegateCommand OpenEditClientCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    OpenEditClient((int)o);
                });
            }
        }
        private void OpenEditClient(int id)
        {
            Client clientForEdit = ActiveClients.FirstOrDefault(o => o.ClientId == id);
            if (clientForEdit == null)
                clientForEdit = InactiveClients.Where(o => o.ClientId == id).First();
            OpenModalWindow(clientForEdit);
        }

        private void OpenModalWindow(Client clientForProcess)
        {
            EditClientViewModel viewModel = new EditClientViewModel(_clientsHttpClient, _trainersHttpClient);
            viewModel.CurrentClient = clientForProcess;
            Window modalWindow = new ClientsModalWindow(viewModel);
            modalWindow.ShowDialog();
        }

        public AsyncDelegateCommand GetClientsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await GetClients(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetClients()
        {
            ActiveClients = new ObservableCollection<Client>(await _clientsHttpClient.GetByAvailabilityAsync(true));
            InactiveClients = new ObservableCollection<Client>(await _clientsHttpClient.GetByAvailabilityAsync(false));
        }

        public AsyncDelegateCommand ChangeAvailabilityCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await ChangeAvailability((int)o); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangeAvailability(int id)
        {
            Client clientForWork = ActiveClients.FirstOrDefault(o => o.ClientId == id);
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
    }
}
