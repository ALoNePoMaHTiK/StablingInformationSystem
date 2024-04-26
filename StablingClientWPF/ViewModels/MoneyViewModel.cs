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

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged();
                GetMoneyTransactions();
                GetBusinessOperations();
            }
        }

        public MoneyViewModel(MoneyTransactionsHttpClient moneyTrainsactionsHttpClient,
            MoneyAccountsHttpClient moneyAccountsHttpClient,
            TrainersHttpClient trainersHttpClient,
            BusinessOperationsHttpClient businessOperationsHttpClient,
            BusinessOperationTypesHttpClient businessOperationTypesHttpClient)
        {
            _moneyTransactionsHttpClient = moneyTrainsactionsHttpClient;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _businessOperationsHttpClient = businessOperationsHttpClient;
            _businessOperationTypesHttpClient = businessOperationTypesHttpClient;

            CurrentDate = DateTime.Now;
            GetMoneyAccounts();
            GetTrainers();
            GetBusinessOperationTypes();
            CurrentTransaction = new MoneyTransaction() { TransactionDate = CurrentDate };
            ClearCurrentBusinessOperation();
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

        public DelegateCommand OpenEditOperationsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditOperationsDialog((int)o);
                });
            }
        }
        private async void OpenEditOperationsDialog(int id)
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

        public DelegateCommand ShowDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    ShowDialog();
                });
            }
        }
        private void ShowDialog()
        {
            IsMoneyTransactionsDialogOpen = true;
        }

        public DelegateCommand CloseDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseDialog();
                });
            }
        }
        private void CloseDialog()
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
            CurrentTransaction = new MoneyTransaction() { TransactionDate = CurrentDate };
            CloseDialog();
        }
        #endregion

    }
}
