using StablingApiClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace StablingClientMAUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ClientsHttpClient _client;

        private void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set
            {
                _Clients = value; OnPropertyChanged();
            }
        }

        private Client _newClient;
        public Client newClient
        {
            get { return _newClient; ; }
            set { _newClient = value; OnPropertyChanged(); }
        }
        public ICommand AddClientCommand { 
            get{
                return new Command(async () =>
                {
                    await AddClient();
                });
            }
        }

        public MainViewModel(ClientsHttpClient client)
        {
            _client = client;
            newClient = new Client();
            GetClientsAsync();
        }

        private async Task GetClientsAsync()
        {
            Clients = new ObservableCollection<Client>(await _client.GetAllAsync());
        }

        private async Task AddClient()
        {
            await _client.CreateClientAsync(newClient);
            await GetClientsAsync();
        }
    }
}