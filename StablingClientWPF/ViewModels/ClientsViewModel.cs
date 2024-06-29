using StablingApiClient;
using StablingClientWPF.Commands;
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

        private ClientsHttpClient _clientsHttpClient;
        private TrainersHttpClient _trainersHttpClient;
        public ClientsViewModel(Mediator mediator,ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient)
        {
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;

            mediator.GetClientInfo += ShowClient;

            GetClients();

            ClearCurrentClient();
            GetTrainers();
        }

        private void ShowClient(int clientId)
        {
            CurrentClient = ActiveClients.Where(c => c.ClientId == clientId).First();
            IsEditMode = false;
            OpenClientDialog();
        }

        private bool _IsEditMode = true;
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set { _IsEditMode = value; OnPropertyChanged(); }
        }

        private bool _IsClientsDialogOpen = false;
        public bool IsClientsDialogOpen
        {
            get { return _IsClientsDialogOpen; }
            set { _IsClientsDialogOpen = value; OnPropertyChanged(); }
        }

        private Client _CurrentClient;
        public Client CurrentClient
        {
            get { return _CurrentClient; }
            set { _CurrentClient = value; OnPropertyChanged(); }
        }
        private async Task GetClients()
        {
            ActiveClients = new ObservableCollection<Client>(
                await _clientsHttpClient.GetByAvailabilityAsync(true));
            InactiveClients = new ObservableCollection<Client>(
                await _clientsHttpClient.GetByAvailabilityAsync(false));
        }

        #region Справочники
        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }
        private async Task GetTrainers()
        {
            Trainers = new ObservableCollection<Trainer>(await _trainersHttpClient.GetAllAsync());
        }
        #endregion

        public DelegateCommand OpenClientDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenClientDialog();
                });
            }
        }
        private void OpenClientDialog()
        {
            IsClientsDialogOpen = true;
        }

        public DelegateCommand CloseClientDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseClientDialog();
                });
            }
        }
        private void CloseClientDialog()
        {
            IsClientsDialogOpen = false;
            IsEditMode = true;
        }

        public DelegateCommand OpenEditClientDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditClientDialog((int)o);
                });
            }
        }
        private async void OpenEditClientDialog(int id)
        {
            CurrentClient = await _clientsHttpClient.GetAsync(id);
            OpenClientDialog();
        }

        public AsyncDelegateCommand ChangeClientAvailabilityCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { 
                    await ChangeClientAvailability((int)o); },
                    ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangeClientAvailability(int id)
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

        public AsyncDelegateCommand ProcessClientCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ProcessClientAsync();
                },
                    ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessClientAsync()
        {
            if (CurrentClient.ClientId == 0)
            {
                await _clientsHttpClient.CreateAsync(CurrentClient);
                await GetClients();
            }
            else
            {
                await _clientsHttpClient.UpdateAsync(CurrentClient);
                Client oldClient = ActiveClients.Where(c =>
                    c.ClientId == CurrentClient.ClientId).FirstOrDefault();
                if (oldClient != null)
                {
                    ActiveClients.Remove(oldClient);
                    ActiveClients.Add(CurrentClient);
                }
                else
                {
                    oldClient = InactiveClients.Where(c =>
                        c.ClientId == CurrentClient.ClientId).FirstOrDefault();
                    InactiveClients.Remove(oldClient);
                    InactiveClients.Add(CurrentClient);
                }
                CloseClientDialog();
                ClearCurrentClient();
            }    
        }
        private void ClearCurrentClient()
        {
            CurrentClient = new Client();
        }
    }
}
