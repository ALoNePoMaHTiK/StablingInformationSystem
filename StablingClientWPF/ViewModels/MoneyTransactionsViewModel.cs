using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class MoneyTransactionsViewModel : BaseViewModel
    {
        private readonly Mediator _mediator;
        private readonly DialogManager _dialogManager;

        private readonly MoneyTransactionsHttpClient _moneyTransactionsHttpClient;
        private readonly MoneyAccountsHttpClient _moneyAccountsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                GetMoneyTransactions();
            }
        }

        public MoneyTransactionsViewModel(Mediator mediator,DialogManager dialogManager,
            MoneyTransactionsHttpClient moneyTrainsactionsHttpClient,
            MoneyAccountsHttpClient moneyAccountsHttpClient,
            TrainersHttpClient trainersHttpClient)
        {
            _mediator = mediator;
            _dialogManager = dialogManager;
            _mediator.OnDayOperationsDateUpdated += OnDateUpdate;
            _moneyTransactionsHttpClient = moneyTrainsactionsHttpClient;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;
            _trainersHttpClient = trainersHttpClient;

        }

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }

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

        public AsyncDelegateCommand CreateMoneyTransactionCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await CreateMoneyTransaction();
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task CreateMoneyTransaction()
        {
            MoneyTransaction? newTransaction = await _dialogManager.OpenMoneyTransactionCreateDialog(
                await _moneyAccountsHttpClient.GetAllAsync(),
                await _trainersHttpClient.GetAllAsync(),
                CurrentDate);
            if (newTransaction != null)
            {
                newTransaction = await _moneyTransactionsHttpClient.CreateAsync(newTransaction);
                MoneyTransactions.Add(
                    await _moneyTransactionsHttpClient.GetForShowAsync(
                        newTransaction.MoneyTransactionId));
            }
        }

        public AsyncDelegateCommand UpdateMoneyTransactionCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await UpdateMoneyTransaction((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task UpdateMoneyTransaction(int transactionId)
        {
            MoneyTransaction? updatedTransaction = await _dialogManager.OpenMoneyTransactionUpdateDialog(
                await _moneyAccountsHttpClient.GetAllAsync(),
                await _trainersHttpClient.GetAllAsync(),
                CurrentDate,
                await _moneyTransactionsHttpClient.GetAsync(transactionId));
            if (updatedTransaction != null)
            {
                await _moneyTransactionsHttpClient.UpdateAsync(updatedTransaction);
                MoneyTransactionForShow oldTransaction = MoneyTransactions.Where(
                    t => t.MoneyTransactionId == updatedTransaction.MoneyTransactionId).First();
                MoneyTransactions.Remove(oldTransaction);
                MoneyTransactions.Add(
                    await _moneyTransactionsHttpClient.GetForShowAsync(
                        updatedTransaction.MoneyTransactionId));
            }
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
                MoneyTransactionForShow oldTransaction = MoneyTransactions.Where(op => op.MoneyTransactionId == id).First();
                MoneyTransactions.Remove(oldTransaction);
            }
        }
    }
}
