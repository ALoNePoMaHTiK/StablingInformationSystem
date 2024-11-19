using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
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
        private readonly BusinessOperationsHttpClient _businessOperationsHttpClient;
        private readonly MoneyAccountsHttpClient _moneyAccountsHttpClient;
        private readonly DialogManager _dialogManager;

        public BusinessOperationsViewModel(Mediator mediator,
            DialogManager dialogManager,
            BusinessOperationsHttpClient businessOperationsHttpClient,
            MoneyAccountsHttpClient moneyAccountsHttpClient)
        {
            _dialogManager = dialogManager;
            mediator.OnDayOperationsDateUpdated += OnDateUpdate;
            _businessOperationsHttpClient = businessOperationsHttpClient;
            _moneyAccountsHttpClient = moneyAccountsHttpClient;
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                GetBusinessOperations();
            }
        }

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

        #region Основные операции

        public DelegateCommand UpdateOperationCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    UpdateOperation((int)o);
                });
            }
        }
        private async void UpdateOperation(int id)
        {
            BusinessOperation? updatedOperation = await _dialogManager.OpenBusinessOperationUpdateDialog(
                await _businessOperationsHttpClient.GetAsync(id));
            if (updatedOperation != null)
            {
                await _businessOperationsHttpClient.UpdateAsync(updatedOperation);
                BusinessOperationForShow updatedOperationForShow = await _businessOperationsHttpClient.GetForShowAsync(updatedOperation.BusinessOperationId);
                BusinessOperationForShow? oldOperation = IncomeBusinessOperations.Where(o =>
                    o.BusinessOperationId == updatedOperation.BusinessOperationId).FirstOrDefault();
                if (oldOperation != null)
                {
                    IncomeBusinessOperations.Remove(oldOperation);
                    IncomeBusinessOperations.Add(updatedOperationForShow);
                }
                else
                {
                    oldOperation = ConsumptionBusinessOperations.Where(o =>
                    o.BusinessOperationId == updatedOperation.BusinessOperationId).First();
                    ConsumptionBusinessOperations.Remove(oldOperation);
                    ConsumptionBusinessOperations.Add(updatedOperationForShow);
                }
            }
        }

        public DelegateCommand CopyOperationCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CopyOperation((int)o);
                });
            }
        }
        private async void CopyOperation(int id)
        {
            BusinessOperation? newOperation = await _dialogManager.OpenBusinessOperationCopyDialog(await _businessOperationsHttpClient.GetAsync(id));
            if (newOperation != null)
            {
                newOperation = await _businessOperationsHttpClient.CreateAsync(newOperation);
                await GetBusinessOperations();
            }
        }

        public DelegateCommand CreateOperationCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CreateOperation();
                });
            }
        }
        private async void CreateOperation()
        {
            BusinessOperation? newOperation = await _dialogManager.OpenBusinessOperationCreateDialog(CurrentDate);
            if (newOperation != null)
            {
                newOperation = await _businessOperationsHttpClient.CreateAsync(newOperation);
                await GetBusinessOperations(); //TODO Продумать получения типа созданной операции
            }
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
                BusinessOperationForShow? oldOperation = IncomeBusinessOperations.Where(op => op.BusinessOperationId == id).FirstOrDefault();
                if (oldOperation != null)
                {
                    IncomeBusinessOperations.Remove(oldOperation);
                }
                else
                {
                    oldOperation = ConsumptionBusinessOperations.Where(op => op.BusinessOperationId == id).First();
                    ConsumptionBusinessOperations.Remove(oldOperation);
                }
            }
        }

        #endregion
    }
}
