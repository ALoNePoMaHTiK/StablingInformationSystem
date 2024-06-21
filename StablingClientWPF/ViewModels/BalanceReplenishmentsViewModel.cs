using StablingApiClient;
using StablingClientWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class BalanceReplenishmentsViewModel : BaseViewModel
    {
        public MoneyAccountsHttpClient _moneyAccountsHttpClient { get; }
        public TrainersHttpClient _trainersHttpClient { get; }
        public ClientsHttpClient _clientsHttpClient { get; }
        public BalanceReplenishmentsHttpClient _balanceReplenishmentsHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                GetBalanceReplenishments();
                ClearCurrentReplenishment();
                CloseBalanceReplenishmentDialog();
            }
        }

        public BalanceReplenishmentsViewModel(Mediator mediator,
            MoneyAccountsHttpClient moneyAccountsHttpClient,
            TrainersHttpClient trainersHttpClient, ClientsHttpClient clientsHttpClient,
            BalanceReplenishmentsHttpClient balanceReplenishmentsHttpClient)
        {
            mediator.GetDayOperationsDate += OnDateUpdate;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _clientsHttpClient = clientsHttpClient;
            _balanceReplenishmentsHttpClient = balanceReplenishmentsHttpClient;

            GetMoneyAccounts();
            GetTrainers();
            GetClients();
        }

        #region Справочники

        private ObservableCollection<MoneyAccount> _MoneyAccounts;
        public ObservableCollection<MoneyAccount> MoneyAccounts
        {
            get { return _MoneyAccounts; }
            set { _MoneyAccounts = value; OnPropertyChanged(); }
        }
        private async void GetMoneyAccounts()
        {
            MoneyAccounts = new ObservableCollection<MoneyAccount>(
                await _moneyAccountsHttpClient.GetAllAsync());
        }

        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }
        private async void GetTrainers()
        {
            Trainers = new ObservableCollection<Trainer>(
                await _trainersHttpClient.GetAllAsync());
        }

        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set { _Clients = value; OnPropertyChanged(); }
        }
        private async void GetClients()
        {
            Clients = new ObservableCollection<Client>(
                await _clientsHttpClient.GetByAvailabilityAsync(true));
        }

        #endregion

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }

        private ObservableCollection<BalanceReplenishmentForShow> _BalanceReplenishments;
        public ObservableCollection<BalanceReplenishmentForShow> BalanceReplenishments
        {
            get { return _BalanceReplenishments; }
            set { _BalanceReplenishments = value; OnPropertyChanged();}
        }
        private async Task GetBalanceReplenishments()
        {
            BalanceReplenishments = new ObservableCollection<BalanceReplenishmentForShow>(
                await _balanceReplenishmentsHttpClient.GetForShowByDateAsync(CurrentDate));
        }

        private BalanceReplenishment _CurrentReplenishment;
        public BalanceReplenishment CurrentReplenishment
        {
            get => _CurrentReplenishment;
            set { _CurrentReplenishment = value; OnPropertyChanged(); }
        }
        private void ClearCurrentReplenishment()
        {
            CurrentReplenishment = new BalanceReplenishment() { ReplenishmentDate = CurrentDate };
        }

        private bool _IsBalanceReplenishmentDialogOpen;
        public bool IsBalanceReplenishmentDialogOpen
        {
            get => _IsBalanceReplenishmentDialogOpen;
            set { _IsBalanceReplenishmentDialogOpen = value; OnPropertyChanged(); }
        }

        public DelegateCommand OpenBalanceReplenishmentDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenBalanceReplenishmentDialog();
                });
            }
        }
        private void OpenBalanceReplenishmentDialog()
        {
            IsBalanceReplenishmentDialogOpen = true;
        }

        public DelegateCommand CloseBalanceReplenishmentDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseBalanceReplenishmentDialog();
                });
            }
        }
        private void CloseBalanceReplenishmentDialog()
        {
            IsBalanceReplenishmentDialogOpen = false;
        }

        public DelegateCommand OpenEditBalanceReplenishmentDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditBalanceReplenishmentDialog((int)o);
                });
            }
        }
        private async void OpenEditBalanceReplenishmentDialog(int id)
        {
            CurrentReplenishment = await _balanceReplenishmentsHttpClient.GetAsync(id);
            OpenBalanceReplenishmentDialog();
        }

        public AsyncDelegateCommand ProcessBalanceReplenishmentCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ProcessBalanceReplenishment();
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessBalanceReplenishment()
        {
            if (CurrentReplenishment.BalanceReplenishmentId == 0)
            {
                await _balanceReplenishmentsHttpClient.CreateAsync(CurrentReplenishment);
                await GetBalanceReplenishments();
            }
            else
            {
                await _balanceReplenishmentsHttpClient.UpdateAsync(CurrentReplenishment);
                BalanceReplenishmentForShow oldReplenishment = BalanceReplenishments.Where(
                    rep => rep.BalanceReplenishmentId != CurrentReplenishment.BalanceReplenishmentId).First();
                BalanceReplenishments.Remove(oldReplenishment);
                BalanceReplenishments.Add(
                    await _balanceReplenishmentsHttpClient.GetForShowAsync(
                        CurrentReplenishment.BalanceReplenishmentId));
            }
            CloseBalanceReplenishmentDialog();
            ClearCurrentReplenishment();
        }

        public AsyncDelegateCommand DeleteBalanceReplenishmentCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await DeleteBalanceReplenishment((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteBalanceReplenishment(int id)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _balanceReplenishmentsHttpClient.DeleteAsync(id);
                BalanceReplenishmentForShow oldReplenishment = BalanceReplenishments.Where(op => op.BalanceReplenishmentId == id).FirstOrDefault();
                BalanceReplenishments.Remove(oldReplenishment);
            }
        }
    }
}
