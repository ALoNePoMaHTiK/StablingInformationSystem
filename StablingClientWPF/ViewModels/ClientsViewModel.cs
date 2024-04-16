using StablingApiClient;
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

        private ObservableCollection<Client> _DisactiveClients;
        public ObservableCollection<Client> InactiveClients
        {
            get { return _DisactiveClients; }
            set { _DisactiveClients = value; OnPropertyChanged(); }
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
            Client clientForEdit = ActiveClients.ToList().Find(o => o.ClientId == id);
            if (clientForEdit == null)
                clientForEdit = InactiveClients.ToList().Find(o => o.ClientId == id);
            OpenModalWindow(clientForEdit);
        }

        private void OpenModalWindow(Client clientForProcess)
        {
            EditClientViewModel viewModel = new EditClientViewModel(_clientsHttpClient, _trainersHttpClient);
            viewModel.CurrentClient = clientForProcess;
            Window modalWindow = new ClientsModalWindow(viewModel);
            modalWindow.ShowDialog();
        }

        public DelegateCommand GetClientsCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    GetClients();
                });
            }
        }
        private async Task GetClients()
        {
            ActiveClients = new ObservableCollection<Client>(await _clientsHttpClient.GetByAvailabilityAsync(true));
            InactiveClients = new ObservableCollection<Client>(await _clientsHttpClient.GetByAvailabilityAsync(false));
        }

        public DelegateCommand DeactivateClientCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    DeactivateClient((int)o);
                });
            }
        }
        private async Task DeactivateClient(int id)
        {
            Client clientForEdit = ActiveClients.ToList().Find(o => o.ClientId == id);
            if (clientForEdit != null)
            {
                clientForEdit.IsAvailable = false;
                await _clientsHttpClient.UpdateAsync(clientForEdit);
                await GetClients();
            }
        }

        public DelegateCommand ActivateClientCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    ActivateClient((int)o);
                });
            }
        }
        private async Task ActivateClient(int id)
        {
            Client clientForEdit = InactiveClients.ToList().Find(o => o.ClientId == id);
            if (clientForEdit != null)
            {
                clientForEdit.IsAvailable = true;
                await _clientsHttpClient.UpdateAsync(clientForEdit);
                await GetClients();
            }
        }

    }
}
