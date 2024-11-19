using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.ViewModels.Dialogs;
using StablingClientWPF.Views;
using StablingClientWPF.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class TrainingsViewModel : BaseViewModel
    {
        private readonly ClientsHttpClient _clientsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;
        private readonly TrainingsHttpClient _trainingsHttpClient;
        private readonly HorsesHttpClient _horsesHttpClient;

        private readonly AbonementsHttpClient _abonementsHttpClient;
        private readonly BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient;
        private readonly BalanceReplenishmentsHttpClient _balanceReplenishmentsHttpClient;
        
        private readonly DialogManager _dialogManager;

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged();
                GetTrainings();
            }
        }

        private ObservableCollection<TrainingForShow> _Trainings;
        public ObservableCollection<TrainingForShow> Trainings
        {
            get { return _Trainings; }
            set { _Trainings = value; OnPropertyChanged(); }
        }

        private readonly Mediator _mediator;

        public TrainingsViewModel(Mediator mediator, ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient, 
            TrainingsHttpClient trainingsHttpClient, 
            HorsesHttpClient horsesHttpClient,
            AbonementsHttpClient abonementsHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient,
            BalanceReplenishmentsHttpClient balanceReplenishmentsHttpClient,
            DialogManager dialogManager)
        {
            _mediator = mediator;
            _mediator.OnDayOperationsDateUpdated += OnDateUpdate;
            _mediator.OnTrainingFundsUpdated += OnTrainingFundsUpdatedAsync;

            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _horsesHttpClient = horsesHttpClient;
            _abonementsHttpClient = abonementsHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
            _balanceReplenishmentsHttpClient = balanceReplenishmentsHttpClient;

            _dialogManager = dialogManager;
        }

        #region Основные операции

        public DelegateCommand UpdateTrainingCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    UpdateTraining((int)o);
                });
            }
        }
        private async void UpdateTraining(int id)
        {
            Training? updatedTraining = await _dialogManager.OpenTrainingUpdateDialog(
                await _trainersHttpClient.GetAllAsync(),
                await _clientsHttpClient.GetAllAsync(),
                await _trainingsHttpClient.GetTypesAsync(),
                await _horsesHttpClient.GetAllAsync(),
                await _trainingsHttpClient.GetAsync(id));
            if (updatedTraining != null)
            {
                await _trainingsHttpClient.UpdateAsync(updatedTraining);
                TrainingForShow oldTraining = Trainings.Where(
                    tr => tr.TrainingId == updatedTraining.TrainingId).First();
                Trainings.Remove(oldTraining);
                Trainings.Add(
                    await _trainingsHttpClient.GetForShowAsync(
                        updatedTraining.TrainingId));
            }
        }

        public DelegateCommand CopyTrainingCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CopyTraining((int)o);
                });
            }
        }
        private async void CopyTraining(int id)
        {
            Training? newTraining = await _dialogManager.OpenTrainingCopyDialog(
                await _trainersHttpClient.GetAllAsync(),
                await _clientsHttpClient.GetAllAsync(),
                await _trainingsHttpClient.GetTypesAsync(),
                await _horsesHttpClient.GetAllAsync(),
                await _trainingsHttpClient.GetAsync(id));
            if (newTraining != null)
            {
                newTraining = await _trainingsHttpClient.CreateAsync(newTraining);
                Trainings.Add(
                    await _trainingsHttpClient.GetForShowAsync(
                        newTraining.TrainingId));
            }
        }

        public DelegateCommand CreateTrainingCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CreateTraining();
                });
            }
        }
        private async void CreateTraining()
        {
            Training? newTraining = await _dialogManager.OpenTrainingCreateDialog(
                await _trainersHttpClient.GetAllAsync(),
                await _clientsHttpClient.GetAllAsync(),
                await _trainingsHttpClient.GetTypesAsync(),
                await _horsesHttpClient.GetAllAsync(), CurrentDate);
            if (newTraining != null)
            {
                newTraining = await _trainingsHttpClient.CreateAsync(newTraining);
                Trainings.Add(
                    await _trainingsHttpClient.GetForShowAsync(
                        newTraining.TrainingId));
            }
        }

        #endregion

        public AsyncDelegateCommand DeleteTrainingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await DeleteTraining((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteTraining(int id)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _trainingsHttpClient.DeleteAsync(id);
                TrainingForShow oldTraining = Trainings.Where(tr => tr.TrainingId == id).First();
                Trainings.Remove(oldTraining);
            }
        }

        private async void GetTrainings()
        {
            Trainings = new ObservableCollection<TrainingForShow>(
                await _trainingsHttpClient.GetForShowByDateAsync(CurrentDate));
        }

        public AsyncDelegateCommand ChangePaidStatusCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ChangePaidStatus((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangePaidStatus(int id)
        {
            await _trainingsHttpClient.ChangePaidStatusAsync(id);
            TrainingForShow oldTraining = Trainings.Where(t => t.TrainingId == id).First();
            TrainingForShow newTraining = oldTraining;
            newTraining.IsPaid = !newTraining.IsPaid;
            Trainings.Remove(oldTraining);
            Trainings.Add(newTraining);
        }

        #region Детали

        public DelegateCommand OpenTrainingDetailsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenTrainingDetailsDialogAsync((int)o);
                });
            }
        }
        private async Task OpenTrainingDetailsDialogAsync(int trainingId)
        {
            await _dialogManager.OpenTrainingDetailsDialogAsync(CurrentDate,trainingId);
        }

        #endregion

        #region Отображение информации о клиенте
        public AsyncDelegateCommand ShowClientInfoCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ShowClientInfo((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ShowClientInfo(int trainingId)
        {
            Training training = await _trainingsHttpClient.GetAsync(trainingId);
            await _dialogManager.OpenClientShowDialog(await _trainersHttpClient.GetAllAsync(),
                await _clientsHttpClient.GetAsync(training.ClientId));
        }
        #endregion

        #region Полная оплата
        public AsyncDelegateCommand MakeFullPaymentCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await MakeFullPayment((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task MakeFullPayment(int trainingId)
        {
            TrainingForShow training = Trainings.Where(tr => tr.TrainingId == trainingId).First();
            TrainingType trainingType = await _trainingsHttpClient.GetTypeAsync(training.TrainingTypeId);
            BalanceReplenishment newReplenishment = new()
            {
                TrainerId = training.TrainerId,
                ClientId = training.ClientId,
                ReplenishmentDate = CurrentDate,
                Amount = trainingType.TypePrice
            };
            await _balanceReplenishmentsHttpClient.CreateAsync(newReplenishment);
            BalanceWithdrawing newWithdrawing = new()
            {
                ClientId = training.ClientId,
                TrainerId = training.TrainerId,
                WithdrawingCause = "Training",
                WithdrawingDate = CurrentDate,
                Amount = trainingType.TypePrice
            };
            await _balanceWithdrawingsHttpClient.CreateByTrainingAsync(newWithdrawing, trainingId);
            await ChangePaidStatus(trainingId);
            GetTrainings();
            //TODO Добавить в медиатор уведомления о создании списаний/пополнений
        }

        #endregion

        #region Оплата при помощи абонемента

        public AsyncDelegateCommand MakeAbonementPaymentCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await MakeAbonementPayment((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task MakeAbonementPayment(int trainingId)
        {
            TrainingForShow training = Trainings.Where(tr => tr.TrainingId == trainingId).First();
            IEnumerable<AbonementForShow> clientAbonements = await _abonementsHttpClient.GetForShowByClientAsync(training.ClientId);
            Client client = await _clientsHttpClient.GetAsync(training.ClientId);
            if(clientAbonements.Count() == 0)
            {
                var result = MessageBox.Show($"У клиента {client.FullName} нет абонементов!\nХотите создать?", "Отсутствие абонементов", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Abonement? abonement = await _dialogManager.OpenAbonementCreationFormAsync(training.ClientId);
                    if (abonement != null)
                    {
                        abonement = await _abonementsHttpClient.CreateAsync(abonement);
                        await _abonementsHttpClient.CreateUsageAsync(new AbonementUsage() {AbonementId = abonement.AbonementId, TrainingId = trainingId });
                        await ChangePaidStatus(trainingId);
                        // TODO Добавить триггер в БД для автоматического закрытия абонемента
                        // TODO Добавить уведомление о создании использования абонемента
                    }
                }
            }
            else
            {
                IEnumerable<AbonementForShow> abonements =
                    await _abonementsHttpClient.GetForShowByClientAsync(training.ClientId);
                int? abonementId = await _dialogManager.OpenAbonementListForCreateUsageDialog(abonements);
                if (abonementId != null)
                {
                    await _abonementsHttpClient.CreateUsageAsync(new AbonementUsage() 
                        {AbonementId = (int)abonementId, TrainingId = trainingId});
                    await ChangePaidStatus(trainingId);
                }
                // TODO Добавить триггер в БД для автоматического закрытия абонемента
                // TODO Добавить уведомление о создании использования абонемента
            }
            //TODO Добавить в медиатор уведомления о создании списаний/пополнений
        }

        #endregion

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }

        private async void OnTrainingFundsUpdatedAsync(int trainingId)
        {
            TrainingForShow oldTraining = Trainings.Where(
                    tr => tr.TrainingId == trainingId).First();
            Trainings.Remove(oldTraining);
            Trainings.Add(
                await _trainingsHttpClient.GetForShowAsync(trainingId));
        }

    }
}
