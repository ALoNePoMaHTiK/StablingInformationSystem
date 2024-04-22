using StablingApiClient;
using StablingClientWPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class MoneyTransactionsViewModel : BaseViewModel
    {
        private MoneyTransactionsHttpClient _moneyTrainsactionsHttpClient;
        public MoneyTransactionsViewModel(MoneyTransactionsHttpClient moneyTrainsactionsHttpClient)
        {
            _moneyTrainsactionsHttpClient = moneyTrainsactionsHttpClient;
            GetMoneyTransactions();
        }

        private ObservableCollection<MoneyTransaction> _MoneyTransactions;
        public ObservableCollection<MoneyTransaction> MoneyTransactions
        {
            get { return _MoneyTransactions; }
            set { _MoneyTransactions = value; OnPropertyChanged(); }
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged(); GetMoneyTransactions(); }
        }

        public AsyncDelegateCommand GetMoneyTransactionsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { 
                    await GetMoneyTransactions(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetMoneyTransactions()
        {
            MoneyTransactions = new ObservableCollection<MoneyTransaction>(
                await _moneyTrainsactionsHttpClient.GetByDateAsync(CurrentDate));
        }
    }
}
