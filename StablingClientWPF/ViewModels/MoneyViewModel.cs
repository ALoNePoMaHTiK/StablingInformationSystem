using StablingApiClient;
using StablingClientWPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class MoneyViewModel : BaseViewModel
    {
        public MoneyTransactionsHttpClient _moneyTransactionsHttpClient { get; }
        public MoneyAccountsHttpClient _moneyAccountsHttpClient { get; }
        public BusinessOperationsHttpClient _businessOperationsHttpClient { get; }
        public BusinessOperationTypesHttpClient _businessOperationTypesHttpClient { get; }
        public TrainersHttpClient _trainersHttpClient { get; }
        public ClientsHttpClient _clientsHttpClient { get; }
        public BalanceReplenishmentsHttpClient _balanceReplenishmentsHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged();
                GetDataByDate();
                ClearCurrentData();
                CloseAllDialogs();
            }
        }

        public MoneyViewModel(MoneyTransactionsHttpClient moneyTrainsactionsHttpClient,
            MoneyAccountsHttpClient moneyAccountsHttpClient,
            TrainersHttpClient trainersHttpClient,
            BusinessOperationsHttpClient businessOperationsHttpClient,
            BusinessOperationTypesHttpClient businessOperationTypesHttpClient,
            ClientsHttpClient clientsHttpClient,
            BalanceReplenishmentsHttpClient balanceReplenishmentsHttpClient)
        {
            _moneyTransactionsHttpClient = moneyTrainsactionsHttpClient;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _businessOperationsHttpClient = businessOperationsHttpClient;
            _businessOperationTypesHttpClient = businessOperationTypesHttpClient;
            _clientsHttpClient = clientsHttpClient;
            _balanceReplenishmentsHttpClient = balanceReplenishmentsHttpClient;

            CurrentDate = DateTime.Now.Date;
            GetMoneyAccounts();
            GetTrainers();
            GetClients();
            GetBusinessOperationTypes();
            GetDataByDate();
            ClearCurrentData();
        }

        private void GetDataByDate()
        {
            GetMoneyTransactions();
            GetBusinessOperations();
            GetBalanceReplenishments();
        }

        private void ClearCurrentData()
        {
            ClearCurrentMoneyTransaction();
            ClearCurrentBusinessOperation();
            ClearCurrentReplenishment();
        }

        private void CloseAllDialogs()
        {
            CloseMoneyTransactionsDialog();
            CloseBusinessOperationsDialog();
            CloseBalanceReplenishmentDialog();
        }

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
                await _clientsHttpClient.GetAllAsync());
        }

        #region BalanceReplenishments

        private ObservableCollection<BalanceReplenishmentForShow> _BalanceReplenishments;
        public ObservableCollection<BalanceReplenishmentForShow> BalanceReplenishments
        {
            get { return _BalanceReplenishments; }
            set { _BalanceReplenishments = value; OnPropertyChanged(); }
        }
        private async Task GetBalanceReplenishments()
        {
            BalanceReplenishments = new ObservableCollection<BalanceReplenishmentForShow>(
                await _balanceReplenishmentsHttpClient.GetByDateForShowAsync(CurrentDate));
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

        #endregion

        #region BusinessOperations

        private ObservableCollection<BusinessOperationForShow> _IncomeBusinessOperations;
        public ObservableCollection<BusinessOperationForShow> IncomeBusinessOperations
        {
            get { return _IncomeBusinessOperations; }
            set { _IncomeBusinessOperations = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BusinessOperationForShow> _ConsumptionBusinessOperations;
        public ObservableCollection<BusinessOperationForShow> ConsumptionBusinessOperations
        {
            get { return _ConsumptionBusinessOperations; }
            set { _ConsumptionBusinessOperations = value; OnPropertyChanged(); }
        }

        private BusinessOperation _CurrentBusinessOperation;
        public BusinessOperation CurrentBusinessOperation
        {
            get { return _CurrentBusinessOperation; }
            set { _CurrentBusinessOperation = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BusinessOperationType> _IncomeBusinessOperationTypes;
        public ObservableCollection<BusinessOperationType> IncomeBusinessOperationTypes
        {
            get { return _IncomeBusinessOperationTypes; }
            set { _IncomeBusinessOperationTypes = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BusinessOperationType> _ConsumptionBusinessOperationTypes;
        public ObservableCollection<BusinessOperationType> ConsumptionBusinessOperationTypes
        {
            get { return _ConsumptionBusinessOperationTypes; }
            set { _ConsumptionBusinessOperationTypes = value; OnPropertyChanged(); }
        }

        private async void GetBusinessOperationTypes()
        {
            IncomeBusinessOperationTypes = new ObservableCollection<BusinessOperationType>(
                await _businessOperationTypesHttpClient.GetIncomeTypesAsync());
            ConsumptionBusinessOperationTypes = new ObservableCollection<BusinessOperationType>(
                await _businessOperationTypesHttpClient.GetConsumptionTypesAsync());
        }

        public AsyncDelegateCommand GetBusinessOperationsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await GetBusinessOperations();
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetBusinessOperations() 
        {
            IncomeBusinessOperations = new ObservableCollection<BusinessOperationForShow>(
                await _businessOperationsHttpClient.GetByIncomeAsync(CurrentDate));
            ConsumptionBusinessOperations = new ObservableCollection<BusinessOperationForShow>(
                await _businessOperationsHttpClient.GetByConsumptionAsync(CurrentDate));
        }

        private bool _IsBusinessOperationsDialogOpen = false;
        public bool IsBusinessOperationsDialogOpen
        {
            get { return _IsBusinessOperationsDialogOpen; }
            set { _IsBusinessOperationsDialogOpen = value; OnPropertyChanged(); }
        }

        public DelegateCommand OpenBusinessOperationsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenBusinessOperationsDialog();
                });
            }
        }
        private void OpenBusinessOperationsDialog()
        {
            IsBusinessOperationsDialogOpen = true;
        }

        public DelegateCommand OpenEditBusinessOperationsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditBusinessOperationsDialog((int)o);
                });
            }
        }
        private async void OpenEditBusinessOperationsDialog(int id)
        {
            CurrentBusinessOperation = await _businessOperationsHttpClient.GetAsync(id);
            OpenBusinessOperationsDialog();
        }

        public DelegateCommand CloseBusinessOperationsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseBusinessOperationsDialog();
                });
            }
        }
        private void CloseBusinessOperationsDialog()
        {
            IsBusinessOperationsDialogOpen = false;
        }

        public AsyncDelegateCommand ProcessBusinessOperationCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ProcessBusinessOperation();
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessBusinessOperation()
        {
            if (CurrentBusinessOperation.BusinessOperationId == 0)
            {
                await _businessOperationsHttpClient.CreateAsync(CurrentBusinessOperation);
                await GetBusinessOperations();
            }
            else
            {
                await _businessOperationsHttpClient.UpdateAsync(CurrentBusinessOperation);
                //ДОБАВИТЬ МЕТОД В API для получение BusinessOperationForShow по первичному ключу
                BusinessOperationForShow oldOperation = IncomeBusinessOperations.Where(op => op.BusinessOperationId ==  CurrentBusinessOperation.BusinessOperationId).FirstOrDefault();
                if (oldOperation != null)
                {
                    IncomeBusinessOperations.Remove(oldOperation);
                }
                else
                {
                    oldOperation = ConsumptionBusinessOperations.Where(op => op.BusinessOperationId == CurrentBusinessOperation.BusinessOperationId).FirstOrDefault();
                    ConsumptionBusinessOperations.Remove(oldOperation);
                }
                BusinessOperationForShow newOperation = await _businessOperationsHttpClient.GetForShowAsync(CurrentBusinessOperation.BusinessOperationId);
                if (newOperation.IsIncome)
                    IncomeBusinessOperations.Add(newOperation);
                else
                    ConsumptionBusinessOperations.Add(newOperation);
                await GetBusinessOperations();
            }
            CloseBusinessOperationsDialog();
            ClearCurrentBusinessOperation();
        }
        private void ClearCurrentBusinessOperation()
        {
            CurrentBusinessOperation = new BusinessOperation() { OperationDateTime = CurrentDate };
        }

        #endregion

        #region MoneyTransactions

        private ObservableCollection<MoneyTransaction> _MoneyTransactions;
        public ObservableCollection<MoneyTransaction> MoneyTransactions
        {
            get { return _MoneyTransactions; }
            set { _MoneyTransactions = value; OnPropertyChanged(); }
        }
        public AsyncDelegateCommand GetMoneyTransactionsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await GetMoneyTransactions();
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetMoneyTransactions()
        {
            MoneyTransactions = new ObservableCollection<MoneyTransaction>(
                await _moneyTransactionsHttpClient.GetByDateAsync(CurrentDate));
        }

        private MoneyTransaction _CurrentTransaction;
        public MoneyTransaction CurrentTransaction
        {
            get => _CurrentTransaction;
            set { _CurrentTransaction = value; OnPropertyChanged(); }
        }

        private bool _IsMoneyTransactionsDialogOpen;
        public bool IsMoneyTransactionsDialogOpen
        {
            get => _IsMoneyTransactionsDialogOpen;
            set { _IsMoneyTransactionsDialogOpen = value; OnPropertyChanged(); }
        }

        public DelegateCommand OpenMoneyTransactionsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenMoneyTransactionsDialog();
                });
            }
        }
        private void OpenMoneyTransactionsDialog()
        {
            IsMoneyTransactionsDialogOpen = true;
        }

        public DelegateCommand CloseMoneyTransactionsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseMoneyTransactionsDialog();
                });
            }
        }
        private void CloseMoneyTransactionsDialog()
        {
            IsMoneyTransactionsDialogOpen = false;
        }

        public AsyncDelegateCommand ProcessTransactionCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await ProcessTransaction(); },
                    ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessTransaction()
        {
            if (CurrentTransaction.MoneyTransactionId == 0)
            {
                await _moneyTransactionsHttpClient.CreateAsync(CurrentTransaction);
                MoneyTransactions.Add(CurrentTransaction);
            }
            else
            {
                await _moneyTransactionsHttpClient.UpdateAsync(CurrentTransaction);
                MoneyTransaction oldTransaction = MoneyTransactions.Where(t =>
                t.MoneyTransactionId == CurrentTransaction.MoneyTransactionId).FirstOrDefault();
                MoneyTransactions.Remove(oldTransaction);
                MoneyTransactions.Add(CurrentTransaction);
            }
            ClearCurrentMoneyTransaction();
            CloseMoneyTransactionsDialog();
        }

        private void ClearCurrentMoneyTransaction()
        {
            CurrentTransaction = new MoneyTransaction() { TransactionDate = CurrentDate };
        }
        #endregion

    }
}
