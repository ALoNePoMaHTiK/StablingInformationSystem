﻿using StablingApiClient;
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
    public class TrainingTypesViewModel : BaseViewModel
    {
        private TrainingsHttpClient _trainingsHttpClient;
        public TrainingTypesViewModel(TrainingsHttpClient trainingsHttpClient)
        {
            _trainingsHttpClient = trainingsHttpClient;
            GetTrainingTypes();
        }

        private ObservableCollection<TrainingType> _TrainingTypes;
        public ObservableCollection<TrainingType> TrainingTypes
        {
            get { return _TrainingTypes; }
            set { _TrainingTypes = value; OnPropertyChanged(); }
        }

        public AsyncDelegateCommand GetTrainingTypesCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { 
                    await GetTrainingTypes(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetTrainingTypes()
        {
            TrainingTypes = new ObservableCollection<TrainingType>(
                await _trainingsHttpClient.GetTypesAsync());
        }
    }
}
