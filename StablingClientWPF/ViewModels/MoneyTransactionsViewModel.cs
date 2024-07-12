using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class MoneyTransactionsViewModel : BaseViewModel
    {
        public MoneyTransactionsHttpClient _moneyTransactionsHttpClient { get; }
        public MoneyAccountsHttpClient _moneyAccountsHttpClient { get; }
        public TrainersHttpClient _trainersHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                GetMoneyTransactions();
                ClearCurrentMoneyTransaction();
                CloseMoneyTransactionsDialog();
            }
        }

        public MoneyTransactionsViewModel(Mediator mediator, MoneyTransactionsHttpClient moneyTrainsactionsHttpClient,
            MoneyAccountsHttpClient moneyAccountsHttpClient, TrainersHttpClient trainersHttpClient)
        {
            mediator.GetDayOperationsDate += OnDateUpdate;
            _moneyTransactionsHttpClient = moneyTrainsactionsHttpClient;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;
            _trainersHttpClient = trainersHttpClient;

            GetMoneyAccounts();
            GetTrainers();
        }

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
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
        #endregion

        private ObservableCollection<MoneyTransactionForShow> _MoneyTransactions;
        public ObservableCollection<MoneyTransactionForShow> MoneyTransactions
        {
            get { return _MoneyTransactions; }
            set { _MoneyTransactions = value; OnPropertyChanged();}
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
            MoneyTransactions = new ObservableCollection<MoneyTransactionForShow>(
                await _moneyTransactionsHttpClient.GetForShowByDateAsync(CurrentDate));
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

        public DelegateCommand OpenEditMoneyTransactionDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditMoneyTransactionDialog((int)o);
                });
            }
        }
        private async void OpenEditMoneyTransactionDialog(int id)
        {
            CurrentTransaction = await _moneyTransactionsHttpClient.GetAsync(id);
            OpenMoneyTransactionsDialog();
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
                await GetMoneyTransactions();
            }
            else
            {
                await _moneyTransactionsHttpClient.UpdateAsync(CurrentTransaction);
                MoneyTransactionForShow oldTransaction = MoneyTransactions.Where(t =>
                t.MoneyTransactionId == CurrentTransaction.MoneyTransactionId).FirstOrDefault();
                MoneyTransactions.Remove(oldTransaction);
                MoneyTransactions.Add(await _moneyTransactionsHttpClient.GetForShowAsync(CurrentTransaction.MoneyTransactionId));
            }
            ClearCurrentMoneyTransaction();
            CloseMoneyTransactionsDialog();
        }

        private void ClearCurrentMoneyTransaction()
        {
            CurrentTransaction = new MoneyTransaction() { TransactionDate = CurrentDate };
        }

        public AsyncDelegateCommand DeleteMoneyTransactionCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await DeleteMoneyTransaction((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteMoneyTransaction(int id)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _moneyTransactionsHttpClient.DeleteAsync(id);
                MoneyTransactionForShow oldTransaction = MoneyTransactions.Where(op => op.MoneyTransactionId == id).FirstOrDefault();
                MoneyTransactions.Remove(oldTransaction);
            }
        }
    }
}
