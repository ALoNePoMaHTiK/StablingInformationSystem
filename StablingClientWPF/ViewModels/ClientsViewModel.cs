using StablingApiClient;
using StablingClientWPF.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class ClientsViewModel : BaseViewModel
    {
        private IEnumerable<Client> _Clients;
        public IEnumerable<Client> Clients
        {
            get { return _Clients; }
            set { _Clients = value; OnPropertyChanged(); }
        }

        private ClientsHttpClient _httpClient;
        public ClientsViewModel(ClientsHttpClient httpClient)
        {
            _httpClient = httpClient;
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
            Client clientForEdit = Clients.ToList().Find(o => o.ClientId == id);
            OpenModalWindow(clientForEdit);
        }

        private void OpenModalWindow(Client clientForProcess)
        {
            EditClientViewModel viewModel = new EditClientViewModel(_httpClient);
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
            Clients = await _httpClient.GetAllAsync();
        }
    }
}
