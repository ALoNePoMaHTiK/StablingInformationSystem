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
    public class EditTrainingViewModel : BaseViewModel
    {
        private ClientsHttpClient _clientsHttpClient;
        private TrainersHttpClient _trainersHttpClient;
        private TrainingsHttpClient _trainingsHttpClient;
        private TrainingTypesHttpClient _trainingTypesHttpClient;
        private HorsesHttpClient _horsesHttpClient;
        public EditTrainingViewModel(ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient, TrainingsHttpClient trainingsHttpClient,
            TrainingTypesHttpClient trainingTypesHttpClient, HorsesHttpClient horsesHttpClient)
        {
            _clientsHttpClient = clientsHttpClient; //Подумать над определением списка клиентов на основании выбранного тренера
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _trainingTypesHttpClient = trainingTypesHttpClient;
            _horsesHttpClient = horsesHttpClient;
            GetTrainers();
            GetClients();
            GetTrainingTypes();
            GetHorses();
        }

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
            Clients = new ObservableCollection<Client>(await _clientsHttpClient.GetAllAsync());
        }

        private async Task GetTrainingTypes()
        {
            TrainingTypes = new ObservableCollection<TrainingType>(await _trainingTypesHttpClient.GetAllAsync());
        }

        private async Task GetHorses()
        {
            Horses = new ObservableCollection<Horse>(await _horsesHttpClient.GetAllAsync());
        }

        private Training _CurrentTraining;
        public Training CurrentTraining
        {
            get { return _CurrentTraining; }
            set { _CurrentTraining = value; OnPropertyChanged(); }
        }
        public AsyncDelegateCommand ProcessTrainingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await ProcessTrainingAsync(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ProcessTrainingAsync()
        {
            try
            {
                if (CurrentTraining.TrainingId != 0)
                    await _trainingsHttpClient.UpdateAsync(CurrentTraining);
                else
                    await _trainingsHttpClient.CreateAsync(CurrentTraining);
            }
            catch (ApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
            CloseWindow();
        }
        private void CloseWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            currentWindow?.Close();
        }
    }
}
