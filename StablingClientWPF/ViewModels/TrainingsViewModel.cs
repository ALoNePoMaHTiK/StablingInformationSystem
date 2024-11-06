using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class TrainingsViewModel : BaseViewModel
    {
        private readonly ClientsHttpClient _clientsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;
        private readonly TrainingsHttpClient _trainingsHttpClient;
        private readonly TrainingTypesHttpClient _trainingTypesHttpClient;
        private readonly HorsesHttpClient _horsesHttpClient;

        private readonly AbonementsHttpClient _abonementsHttpClient;
        private readonly BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient;
        private readonly BalanceReplenishmentsHttpClient _balanceReplenishmentsHttpClient;

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged();
                GetTrainings();
                ClearCurrentTraining();
                CloseDialog();
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
            TrainingTypesHttpClient trainingTypesHttpClient, 
            HorsesHttpClient horsesHttpClient,
            AbonementsHttpClient abonementsHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient,
            BalanceReplenishmentsHttpClient balanceReplenishmentsHttpClient)
        {
            _mediator = mediator;
            _mediator.OnDayOperationsDateUpdated += OnDateUpdate;
            _mediator.OnTrainingFundsUpdated += OnTrainingFundsUpdatedAsync;

            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _trainingTypesHttpClient = trainingTypesHttpClient;
            _horsesHttpClient = horsesHttpClient;
            _abonementsHttpClient = abonementsHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
            _balanceReplenishmentsHttpClient = balanceReplenishmentsHttpClient;

            GetTrainers();
            GetClients();
            GetTrainingTypes();
            GetHorses();
        }

        #region Справочники

        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set { _Clients = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TrainingType> _TrainingTypes;
        public ObservableCollection<TrainingType> TrainingTypes
        {
            get { return _TrainingTypes; }
            set { _TrainingTypes = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Horse> _Horses;
        public ObservableCollection<Horse> Horses
        {
            get { return _Horses; }
            set { _Horses = value; OnPropertyChanged(); }
        }

        private async Task GetTrainers()
        {
            Trainers = new ObservableCollection<Trainer>(await _trainersHttpClient.GetAllAsync());
        }

        private async Task GetClients()
        {
            Clients = new ObservableCollection<Client>(await _clientsHttpClient.GetByAvailabilityAsync(true));
        }

        private async Task GetTrainingTypes()
        {
            TrainingTypes = new ObservableCollection<TrainingType>(await _trainingTypesHttpClient.GetAllAsync());
        }

        private async Task GetHorses()
        {
            Horses = new ObservableCollection<Horse>(await _horsesHttpClient.GetAllAsync());
        }

        #endregion

        #region Основная форма


        private bool _IsDialogOpen = false;
        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set { _IsDialogOpen = value; OnPropertyChanged(); }
        }
        public DelegateCommand OpenDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenDialog();
                });
            }
        }
        public virtual void OpenDialog()
        {
            IsDialogOpen = true;
        }
        public virtual void CloseDialog()
        {
            IsDialogOpen = false;
        }
        

        public DelegateCommand OpenEditTrainingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenEditTrainingDialog((int)o);
                });
            }
        }
        private async void OpenEditTrainingDialog(int id)
        {
            CurrentTraining = await _trainingsHttpClient.GetAsync(id);
            OpenDialog();
        }

        public DelegateCommand OpenCopyTrainingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenCopyTrainingDialog((int)o);
                });
            }
        }
        private async void OpenCopyTrainingDialog(int id)
        {
            CurrentTraining = await _trainingsHttpClient.GetAsync(id);
            CurrentTraining.TrainingId = 0;
            OpenDialog();
        }
        #endregion

        private Training _CurrentTraining;
        public Training CurrentTraining
        {
            get { return _CurrentTraining; }
            set { _CurrentTraining = value; OnPropertyChanged(); }
        }

        private void ClearCurrentTraining()
        {
            CurrentTraining = new Training() { TrainingDateTime = CurrentDate.Date };
        }

        private async Task GetTrainings()
        {
            Trainings = new ObservableCollection<TrainingForShow>(
                await _trainingsHttpClient.GetForShowByDateAsync(CurrentDate));
        }


        public AsyncDelegateCommand ProcessTrainingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ProcessTraining();
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessTraining()
        {
            if (CurrentTraining.TrainingId == 0)
            {
                await _trainingsHttpClient.CreateAsync(CurrentTraining);
                await GetTrainings();
            }
            else
            {
                await _trainingsHttpClient.UpdateAsync(CurrentTraining);
                TrainingForShow oldTraining = Trainings.Where(
                    tr => tr.TrainingId == CurrentTraining.TrainingId).First();
                Trainings.Remove(oldTraining);
                Trainings.Add(
                    await _trainingsHttpClient.GetForShowAsync(
                        CurrentTraining.TrainingId));
            }
            CloseDialog();
            ClearCurrentTraining();
        }

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
            await MaterialDesignThemes.Wpf.DialogHost.Show(
                new TrainingsDetailsDialog(new TrainingsDetailsDialogViewModel(_mediator,
                    _trainingsHttpClient, _balanceWithdrawingsHttpClient, CurrentDate, trainingId)), ROOT_IDENTIFIER);
        }

        #endregion

        #region Переключение на страницу клиентов
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
            _mediator.ShowClientInfo(training.ClientId);
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
            TrainingType trainingType = await _trainingTypesHttpClient.GetAsync(training.TrainingTypeId);
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
            await GetTrainings();
            //TODO Добавить в медиатор уведомления о создании списаний/пополнений
        }

        #endregion

        //TODO Добавить возможность оплаты тренировки при помощи абонемента

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

        #region Использование абонемента
        public AsyncDelegateCommand UseAbonementForPayCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await UseAbonementForPay((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }

        private async Task UseAbonementForPay(int trainingId)
        {
            TrainingForShow training = Trainings.First(t => t.TrainingId == trainingId);
            if (!training.IsPaid)
            {
                IEnumerable<AbonementForShow> abonements = 
                    await _abonementsHttpClient.GetForShowByClientAsync(training.ClientId);
                // TODO Добавить форму выбора абонемента для оплаты
                var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementUsageDialog(new AbonementUsageDialogViewModel(
                    abonements)), DAY_OPERATIONS_IDENTIFIER);
                if (result != null) 
                {
                    AbonementMark mark = new AbonementMark()
                    {
                        AbonementId = (int)result,
                        TrainingId = trainingId
                    };
                    await _abonementsHttpClient.CreateMarkAsync(mark);
                }
                // TODO Добавить триггер в БД для автоматического закрытия абонемента
                // TODO Добавить уведомление о создании использования абонемента
            }
            await _trainingsHttpClient.ChangePaidStatusAsync(trainingId);
        }

        #endregion
    }
}
