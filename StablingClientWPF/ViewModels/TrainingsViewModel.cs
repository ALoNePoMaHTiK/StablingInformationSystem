using StablingApiClient;
using StablingClientWPF.Commands;
using StablingClientWPF.Views;
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
            set { _CurrentDate = value; OnPropertyChanged(); GetTrainings(); }
        }

        private ObservableCollection<TrainingForShow> _Trainings;
        public ObservableCollection<TrainingForShow> Trainings
        {
            get { return _Trainings; }
            set { _Trainings = value; OnPropertyChanged(); }
        }

        public TrainingsViewModel(ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient, 
            TrainingsHttpClient trainingsHttpClient,
            TrainingTypesHttpClient trainingTypesHttpClient, 
            HorsesHttpClient horsesHttpClient)
        {
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _trainingTypesHttpClient = trainingTypesHttpClient;
            _horsesHttpClient = horsesHttpClient;



            CurrentDate = DateTime.Now.Date;
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

        public AsyncDelegateCommand DeleteTrainingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await DeleteTraining((int)o); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteTraining(int id)
        {
            await _trainingsHttpClient.DeleteAsync(id);
            Trainings.Remove(Trainings.Where(t => t.TrainingId == id).First());
        }

        public DelegateCommand OpenCopyTrainingCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    OpenCopyTraining((int)o);
                });
            }
        }
        private async Task OpenCopyTraining(int id)
        {
            Training trainingForCopy = await _trainingsHttpClient.GetAsync(id);
            trainingForCopy.TrainingId = 0;
            OpenModalWindow(trainingForCopy);
        }

        public DelegateCommand OpenCreateTrainingCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    OpenCreateTraining();
                });
            }
        }
        private void OpenCreateTraining()
        {
            OpenModalWindow(new Training() { TrainingDateTime=CurrentDate});
        }

        public DelegateCommand OpenEditTrainingCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    OpenEditTraining((int)o);
                });
            }
        }
        private async void OpenEditTraining(int id)
        {
            Training trainingForEdit = await _trainingsHttpClient.GetAsync(id);
            OpenModalWindow(trainingForEdit);
        }

        private void OpenModalWindow(Training trainingForProcess)
        {
            EditTrainingViewModel viewModel = new (_clientsHttpClient,
                _trainersHttpClient,_trainingsHttpClient,_trainingTypesHttpClient, _horsesHttpClient);
            viewModel.CurrentTraining = trainingForProcess;
            Window modalWindow = new TrainingsModalWindow(viewModel);
            modalWindow.ShowDialog();
        }

    }
}
