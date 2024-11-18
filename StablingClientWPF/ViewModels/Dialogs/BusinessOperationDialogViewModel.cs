using StablingApiClient;
using System.Collections.ObjectModel;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class BusinessOperationDialogViewModel : BaseViewModel
    {
        public BusinessOperationDialogViewModel(IEnumerable<MoneyAccount> moneyAccounts,
            IEnumerable<BusinessOperationType> incomeBusinessOperationTypes,
            IEnumerable<BusinessOperationType> consumptionBusinessOperationTypes,
            BusinessOperation businessOperation)
        {
            MoneyAccounts = new ObservableCollection<MoneyAccount>(moneyAccounts);
            IncomeBusinessOperationTypes = new ObservableCollection<BusinessOperationType>(incomeBusinessOperationTypes);
            ConsumptionBusinessOperationTypes = new ObservableCollection<BusinessOperationType>(consumptionBusinessOperationTypes);
            BusinessOperation = businessOperation;
        }

        private BusinessOperation _BusinessOperation;
        public BusinessOperation BusinessOperation
        {
            get { return _BusinessOperation; }
            set { _BusinessOperation = value; OnPropertyChanged(); }
        }
        
        #region Справочники

        private ObservableCollection<MoneyAccount> _MoneyAccounts;
        public ObservableCollection<MoneyAccount> MoneyAccounts
        {
            get { return _MoneyAccounts; }
            set { _MoneyAccounts = value; OnPropertyChanged(); }
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
        #endregion
    }
}
