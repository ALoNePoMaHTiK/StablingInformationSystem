using StablingApiClient;
using StablingClientWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class TrainingsViewModel : BaseViewModel
    {
        private ClientsHttpClient _clientsHttpClient;
        private TrainersHttpClient _trainersHttpClient;
        private TrainingsHttpClient _trainingsHttpClient;
        private TrainingTypesHttpClient _trainingTypesHttpClient;

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged(); GetTrainings(); }
        }

        private ObservableCollection<Training> _Trainings;
        public ObservableCollection<Training> Trainings
        {
            get { return _Trainings; }
            set { _Trainings = value; OnPropertyChanged(); }
        }

        public TrainingsViewModel(ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient, TrainingsHttpClient trainingsHttpClient,
            TrainingTypesHttpClient trainingTypesHttpClient)
        {
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
            _trainingTypesHttpClient = trainingTypesHttpClient;



            CurrentDate = DateTime.Now.Date;
        }

        public DelegateCommand GetTrainingsCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    GetTrainings();
                });
            }
        }
        private async Task GetTrainings()
        {
            Trainings = new ObservableCollection<Training>(await _trainingsHttpClient.GetByDayAsync(CurrentDate));
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
            OpenModalWindow(new Training());
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
        private void OpenEditTraining(int id)
        {
            Training trainingForEdit = Trainings.ToList().Find(o => o.TrainingId == id);
            OpenModalWindow(trainingForEdit);
        }

        private void OpenModalWindow(Training trainingForProcess)
        {
            EditTrainingViewModel viewModel = new (_clientsHttpClient,
                _trainersHttpClient,_trainingsHttpClient,_trainingTypesHttpClient);
            viewModel.CurrentTraining = trainingForProcess;
            Window modalWindow = new TrainingsModalWindow(viewModel);
            modalWindow.ShowDialog();
        }

    }
}
