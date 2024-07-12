using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class TrainingsViewModel : BaseViewModel
    {
        private ClientsHttpClient _clientsHttpClient;
        private TrainersHttpClient _trainersHttpClient;
        private TrainingsHttpClient _trainingsHttpClient;
        private TrainingTypesHttpClient _trainingTypesHttpClient;
        private HorsesHttpClient _horsesHttpClient;
        public BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged(); 
                GetTrainings();
                ClearCurrentTraining();
                MainForm.CloseDialog();
            }
        }

        private ObservableCollection<TrainingForShow> _Trainings;
        public ObservableCollection<TrainingForShow> Trainings
        {
            get { return _Trainings; }
            set { _Trainings = value; OnPropertyChanged(); }
        }

        private Mediator _mediator;

        public TrainingsViewModel(Mediator mediator, ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient, 
            TrainingsHttpClient trainingsHttpClient,
            TrainingTypesHttpClient trainingTypesHttpClient, 
            HorsesHttpClient horsesHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient)
        {
            _mediator = mediator;
            _mediator.GetDayOperationsDate += OnDateUpdate;

            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _trainingTypesHttpClient = trainingTypesHttpClient;
            _horsesHttpClient = horsesHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;

            MainForm = new DialogManager();
            CurrentDate = DateTime.Now.Date;

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

        public DialogManager MainForm { get; }

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
            MainForm.OpenDialog();
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
            MainForm.OpenDialog();
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

        public AsyncDelegateCommand GetTrainingsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await GetTrainings(); }, ex => MessageBox.Show(ex.ToString()));
            }
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
                    tr => tr.TrainingId != CurrentTraining.TrainingId).First();
                Trainings.Remove(oldTraining);
                Trainings.Add(
                    await _trainingsHttpClient.GetForShowAsync(
                        CurrentTraining.TrainingId));
            }
            MainForm.CloseDialog();
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

        #region Форма для отображения деталей

        private bool _IsTrainingDetailsDialogOpen = false;
        public bool IsTrainingDetailsDialogOpen
        {
            get { return _IsTrainingDetailsDialogOpen; }
            set { _IsTrainingDetailsDialogOpen = value; OnPropertyChanged(); }
        }
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
            IsTrainingDetailsDialogOpen = true;
            CurrentTraining = await _trainingsHttpClient.GetAsync(trainingId);
            GetBalanceWithdrawings();
        }
        public DelegateCommand CloseTrainingDetailsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseTrainingDetailsDialog();
                });
            }
        }
        private void CloseTrainingDetailsDialog()
        {
            IsTrainingDetailsDialogOpen = false;
        }

        #endregion
        private ObservableCollection<BalanceWithdrawingForShow> _BalanceWithdrawings;
        public ObservableCollection<BalanceWithdrawingForShow> BalanceWithdrawings
        {
            get { return _BalanceWithdrawings; }
            set { _BalanceWithdrawings = value; OnPropertyChanged(); }
        }
        private async Task GetBalanceWithdrawings()
        {
            BalanceWithdrawings = new ObservableCollection<BalanceWithdrawingForShow>(
                await _balanceWithdrawingsHttpClient.GetForShowByTrainingAsync(CurrentTraining.TrainingId));
        }

        #endregion

        #region Переключение на страницу списаний с баланса
        public AsyncDelegateCommand CreateTrainingWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await CreateTrainingWithdrawing((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task CreateTrainingWithdrawing(int trainingId)
        {
            _mediator.CreateTrainingWithdrawing(await _trainingsHttpClient.GetAsync(trainingId));
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

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }
    }
}
