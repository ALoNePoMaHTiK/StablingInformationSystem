using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class BalanceWithdrawingsViewModel : BaseViewModel
    {
        public MoneyAccountsHttpClient _moneyAccountsHttpClient { get; }
        public TrainersHttpClient _trainersHttpClient { get; }
        public ClientsHttpClient _clientsHttpClient { get; }
        public TrainingsHttpClient _trainingsHttpClient { get; }

        public BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                GetBalanceWithdrawings();
                ClearCurrentWithdrawing();
                CloseBalanceWithdrawingDialog();
            }
        }

        public BalanceWithdrawingsViewModel(Mediator mediator,
            MoneyAccountsHttpClient moneyAccountsHttpClient,
            TrainersHttpClient trainersHttpClient, ClientsHttpClient clientsHttpClient, TrainingsHttpClient trainingsHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient)
        {
            mediator.GetDayOperationsDate += OnDateUpdate;
            mediator.NeedToCreateTrainingWithdrawing += CreateTrainingWithdrawing;


            _moneyAccountsHttpClient = moneyAccountsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _clientsHttpClient = clientsHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;

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

        private ObservableCollection<TrainingForShow> _Trainings;
        public ObservableCollection<TrainingForShow> Trainings
        {
            get { return _Trainings; }
            set { _Trainings = value; OnPropertyChanged(); }
        }
        private async void GetTrainings()
        {
            Trainings = new ObservableCollection<TrainingForShow>(
                await _trainingsHttpClient.GetAllNotPaidForShowAsync());
        }

        #endregion

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }

        private ObservableCollection<BalanceWithdrawingForShow> _BalanceWithdrawings;
        public ObservableCollection<BalanceWithdrawingForShow> BalanceWithdrawings
        {
            get { return _BalanceWithdrawings; }
            set { _BalanceWithdrawings = value; OnPropertyChanged(); }
        }
        private async Task GetBalanceWithdrawings()
        {
            BalanceWithdrawings = new ObservableCollection<BalanceWithdrawingForShow>(
                await _balanceWithdrawingsHttpClient.GetForShowByDateAsync(CurrentDate));
        }

        private BalanceWithdrawing _CurrentWithdrawing;
        public BalanceWithdrawing CurrentWithdrawing
        {
            get => _CurrentWithdrawing;
            set { _CurrentWithdrawing = value; OnPropertyChanged(); }
        }
        private void ClearCurrentWithdrawing()
        {
            CurrentWithdrawing = new BalanceWithdrawing() { WithdrawingDate = CurrentDate };
        }

        private bool _IsBalanceWithdrawingDialogOpen;
        public bool IsBalanceWithdrawingDialogOpen
        {
            get => _IsBalanceWithdrawingDialogOpen;
            set { _IsBalanceWithdrawingDialogOpen = value; OnPropertyChanged(); }
        }

        public DelegateCommand OpenBalanceWithdrawingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenBalanceWithdrawingDialog();
                });
            }
        }
        private void OpenBalanceWithdrawingDialog()
        {
            IsBalanceWithdrawingDialogOpen = true;
            GetTrainings();
        }

        public DelegateCommand CloseBalanceWithdrawingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseBalanceWithdrawingDialog();
                });
            }
        }
        private void CloseBalanceWithdrawingDialog()
        {
            IsBalanceWithdrawingDialogOpen = false;
            IsEditMode = true;
        }

        public DelegateCommand OpenEditBalanceWithdrawingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditBalanceWithdrawingDialog((Guid)o);
                });
            }
        }
        private async void OpenEditBalanceWithdrawingDialog(Guid id)
        {
            CurrentWithdrawing = await _balanceWithdrawingsHttpClient.GetAsync(id);
            OpenBalanceWithdrawingDialog();
        }

        private void CreateTrainingWithdrawing(Training training)
        {
            CurrentWithdrawing = new BalanceWithdrawing() 
            { 
                WithdrawingDate = CurrentDate,
                TrainerId = training.TrainerId,
                ClientId = training.ClientId
            };
            OpenBalanceWithdrawingDialog();
            TrainingId = training.TrainingId;
            IsEditMode = false;
        }

        private bool _IsEditMode = true;
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set { _IsEditMode = value; OnPropertyChanged(); }
        }


        private int _TrainingId;
        public int TrainingId
        {
            get { return _TrainingId; }
            set { _TrainingId = value; OnPropertyChanged(); }
        }

        public AsyncDelegateCommand ProcessBalanceWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ProcessBalanceWithdrawing((string)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessBalanceWithdrawing(string flag)
        {
            if (CurrentWithdrawing.BalanceWithdrawingId == Guid.Empty)
            {
                CurrentWithdrawing.BalanceWithdrawingId = Guid.NewGuid();
                if (flag == "Training")
                {
                    CurrentWithdrawing.WithdrawingCause = "Training";
                    await _balanceWithdrawingsHttpClient.CreateByTrainingAsync(CurrentWithdrawing, TrainingId);
                }  
                await GetBalanceWithdrawings();
            }
            else
            {
                await _balanceWithdrawingsHttpClient.UpdateAsync(CurrentWithdrawing);
                BalanceWithdrawingForShow oldReplenishment = BalanceWithdrawings.Where(
                    rep => rep.BalanceWithdrawingId != CurrentWithdrawing.BalanceWithdrawingId).First();
                BalanceWithdrawings.Remove(oldReplenishment);
                BalanceWithdrawings.Add(
                    await _balanceWithdrawingsHttpClient.GetForShowAsync(
                        CurrentWithdrawing.BalanceWithdrawingId));
            }
            CloseBalanceWithdrawingDialog();
            ClearCurrentWithdrawing();
        }

        public AsyncDelegateCommand DeleteBalanceWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await DeleteBalanceWithdrawing((Guid)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteBalanceWithdrawing(Guid id)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _balanceWithdrawingsHttpClient.DeleteAsync(id);
                BalanceWithdrawingForShow oldReplenishment = BalanceWithdrawings.Where(op => op.BalanceWithdrawingId == id).First();
                BalanceWithdrawings.Remove(oldReplenishment);
            }
        }
    }
}
