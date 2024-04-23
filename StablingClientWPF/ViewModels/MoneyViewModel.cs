using StablingApiClient;
using StablingClientWPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class MoneyViewModel : BaseViewModel
    {
        public MoneyTransactionsViewModel MoneyTransactionsViewModel { get; }
        public BusinessOperationsViewModel BusinessOperationsViewModel { get; }
        public MoneyTransactionsHttpClient _moneyTransactionsHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged(); MoneyTransactionsViewModel.CurrentDate = value; }
        }

        public MoneyViewModel(MoneyTransactionsViewModel _MoneyTransactionsViewModel,
            BusinessOperationsViewModel businessOperationsViewModel,
            MoneyTransactionsHttpClient moneyTrainsactionsHttpClient)
        {
            MoneyTransactionsViewModel = _MoneyTransactionsViewModel;
            BusinessOperationsViewModel = businessOperationsViewModel;

            _moneyTransactionsHttpClient = moneyTrainsactionsHttpClient;

            CurrentDate = DateTime.Now;
            GetMoneyTransactions();
            CurrentTransaction = new MoneyTransaction() { TransactionDate = CurrentDate };
        }

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
