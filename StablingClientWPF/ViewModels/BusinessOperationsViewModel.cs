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
    public class BusinessOperationsViewModel : BaseViewModel
    {
        public BusinessOperationsHttpClient _businessOperationsHttpClient { get; }
        public BusinessOperationTypesHttpClient _businessOperationTypesHttpClient { get; }
        public MoneyAccountsHttpClient _moneyAccountsHttpClient { get; }

        public BusinessOperationsViewModel(Mediator mediator, BusinessOperationsHttpClient businessOperationsHttpClient,
            BusinessOperationTypesHttpClient businessOperationTypesHttpClient,
            MoneyAccountsHttpClient moneyAccountsHttpClient)
        {
            mediator.GetDayOperationsDate += OnDateUpdate;
            _businessOperationsHttpClient = businessOperationsHttpClient;
            _businessOperationTypesHttpClient = businessOperationTypesHttpClient;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;

            GetMoneyAccounts();
            GetBusinessOperationTypes();
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                GetBusinessOperations();
                ClearCurrentBusinessOperation();
                CloseBusinessOperationsDialog();
            }
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

        #endregion

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }

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

        private BusinessOperation _CurrentBusinessOperation;
        public BusinessOperation CurrentBusinessOperation
        {
            get { return _CurrentBusinessOperation; }
            set { _CurrentBusinessOperation = value; OnPropertyChanged(); }
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

        public DelegateCommand OpenEditBusinessOperationDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditBusinessOperationDialog((int)o);
                });
            }
        }
        private async void OpenEditBusinessOperationDialog(int id)
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
                BusinessOperationForShow oldOperation = IncomeBusinessOperations.Where(op => op.BusinessOperationId == CurrentBusinessOperation.BusinessOperationId).FirstOrDefault();
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

        public AsyncDelegateCommand DeleteBusinessOperationCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await DeleteBusinessOperation((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteBusinessOperation(int id)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _businessOperationsHttpClient.DeleteAsync(id);
                BusinessOperationForShow oldOperation = IncomeBusinessOperations.Where(op => op.BusinessOperationId == id).FirstOrDefault();
                if (oldOperation != null)
                {
                    IncomeBusinessOperations.Remove(oldOperation);
                }
                else
                {
                    oldOperation = ConsumptionBusinessOperations.Where(op => op.BusinessOperationId == id).FirstOrDefault();
                    ConsumptionBusinessOperations.Remove(oldOperation);
                }
            }
        }

        private void ClearCurrentBusinessOperation()
        {
            CurrentBusinessOperation = new BusinessOperation() { OperationDateTime = CurrentDate };
        }
    }
}
