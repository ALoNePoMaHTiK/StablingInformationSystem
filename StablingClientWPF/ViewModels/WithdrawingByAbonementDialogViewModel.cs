using StablingApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.ViewModels
{
    public class WithdrawingByAbonementDialogViewModel : BaseViewModel
    {
        //Используется при вызове для ДОБАВЛЕНИЯ
        public WithdrawingByAbonementDialogViewModel(DateTime currentDate, AbonementForShow abonement)
        {
            Abonement = abonement;
            CurrentWithdrawing = new BalanceWithdrawing()
            {
                WithdrawingDate = currentDate,
                ClientId = Abonement.ClientId,
                TrainerId = Abonement.TrainerId,
                WithdrawingCause = "Abonement"
            };
        }

        //Используется при вызове для ИЗМЕНЕНИЯ
        public WithdrawingByAbonementDialogViewModel(BalanceWithdrawing withdrawing, AbonementForShow abonement)
        {
            CurrentWithdrawing = withdrawing;
            Abonement = abonement;
        }

        private BalanceWithdrawing _CurrentWithdrawing;
        public BalanceWithdrawing CurrentWithdrawing
        {
            get => _CurrentWithdrawing;
            set { _CurrentWithdrawing = value; OnPropertyChanged(); }
        }

        private AbonementForShow? _Abonement;
        public AbonementForShow Abonement
        {
            get => _Abonement;
            set { _Abonement = value; OnPropertyChanged(); }
        }
    }
}
