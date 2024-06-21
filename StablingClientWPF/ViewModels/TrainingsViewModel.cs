using StablingApiClient;
using StablingClientWPF.Commands;
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

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged(); 
                GetTrainings();
                ClearCurrentTraining();
                CloseTrainingDialog();
            }
        }

        private ObservableCollection<TrainingForShow> _Trainings;
        public ObservableCollection<TrainingForShow> Trainings
        {
            get { return _Trainings; }
            set { _Trainings = value; OnPropertyChanged(); }
        }

        public TrainingsViewModel(Mediator mediator, ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient, 
            TrainingsHttpClient trainingsHttpClient,
            TrainingTypesHttpClient trainingTypesHttpClient, 
            HorsesHttpClient horsesHttpClient)
        {
            mediator.GetDayOperationsDate += OnDateUpdate;

            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _trainingTypesHttpClient = trainingTypesHttpClient;
            _horsesHttpClient = horsesHttpClient;

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

        private bool _IsTrainingsDialogOpen = false;
        public bool IsTrainingsDialogOpen
        {
            get { return _IsTrainingsDialogOpen; }
            set { _IsTrainingsDialogOpen = value; OnPropertyChanged(); }
        }

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
            Trainings = new ObservableCollection<TrainingForShow>(await _trainingsHttpClient.GetForShowByDateAsync(CurrentDate));
        }

        public DelegateCommand OpenTrainingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenTrainingDialog();
                });
            }
        }
        private void OpenTrainingDialog()
        {
            IsTrainingsDialogOpen = true;
        }

        public DelegateCommand CloseTrainingDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseTrainingDialog();
                });
            }
        }
        private void CloseTrainingDialog()
        {
            IsTrainingsDialogOpen = false;
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
            OpenTrainingDialog();
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
            OpenTrainingDialog();
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
            CloseTrainingDialog();
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
                TrainingForShow oldTraining = Trainings.Where(tr => tr.TrainingId == id).FirstOrDefault();
                Trainings.Remove(oldTraining);
            }
        }

        private void OnDateUpdate(DateTime newDate)
        {
            CurrentDate = newDate;
        }
    }
}
