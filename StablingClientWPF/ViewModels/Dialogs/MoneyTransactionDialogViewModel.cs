using StablingApiClient;
using System.Collections.ObjectModel;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class MoneyTransactionDialogViewModel : BaseViewModel
    {
        /// <summary>
        /// Конструктор для добавления новой транзакции
        /// </summary>
        public MoneyTransactionDialogViewModel(IEnumerable<MoneyAccount> moneyAccounts,
            IEnumerable<Trainer> trainers,
            DateTime CurrentDate)
        {
            MoneyAccounts = new ObservableCollection<MoneyAccount>(moneyAccounts);
            Trainers = new ObservableCollection<Trainer>(trainers);
            Transaction = new MoneyTransaction() { TransactionDate = CurrentDate };
        }

        /// <summary>
        /// Конструктор для изменения транзакции
        /// </summary>
        public MoneyTransactionDialogViewModel(IEnumerable<MoneyAccount> moneyAccounts,
            IEnumerable<Trainer> trainers, MoneyTransaction transaction) : this(moneyAccounts, trainers,DateTime.Now)
        {
            Transaction = transaction;
        }

        private MoneyTransaction _Transaction;
        public MoneyTransaction Transaction
        {
            get => _Transaction;
            set { _Transaction = value; OnPropertyChanged(); }
        }

        #region Коллекции
        private ObservableCollection<MoneyAccount> _MoneyAccounts;
        public ObservableCollection<MoneyAccount> MoneyAccounts
        {
            get { return _MoneyAccounts; }
            set { _MoneyAccounts = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
