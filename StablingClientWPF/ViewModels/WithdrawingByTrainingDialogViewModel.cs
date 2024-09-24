using StablingApiClient;
using StablingClientWPF.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class WithdrawingByTrainingDialogViewModel : BaseViewModel
    {

        //Используется при вызове для ДОБАВЛЕНИЯ
        public WithdrawingByTrainingDialogViewModel(DateTime currentDate, TrainingForShow training)
        {
            Training = training;
            CurrentWithdrawing = new BalanceWithdrawing()
            {
                WithdrawingDate = currentDate,
                ClientId = Training.ClientId,
                TrainerId = Training.TrainerId,
                WithdrawingCause = "Training"
            };
        }

        //Используется при вызове для ИЗМЕНЕНИЯ
        public WithdrawingByTrainingDialogViewModel(BalanceWithdrawing withdrawing, TrainingForShow training)
        {
            CurrentWithdrawing = withdrawing;
            Training = training;
        }

        private BalanceWithdrawing _CurrentWithdrawing;
        public BalanceWithdrawing CurrentWithdrawing
        {
            get => _CurrentWithdrawing;
            set { _CurrentWithdrawing = value; OnPropertyChanged(); }
        }

        private TrainingForShow? _Training;
        public TrainingForShow Training
        {
            get => _Training;
            set { _Training = value; OnPropertyChanged(); }
        }
    }
}
