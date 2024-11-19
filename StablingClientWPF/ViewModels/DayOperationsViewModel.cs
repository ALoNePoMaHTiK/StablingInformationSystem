using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StablingClientWPF.Helpers;

namespace StablingClientWPF.ViewModels
{
    public class DayOperationsViewModel : BaseViewModel
    {
        public TrainingsViewModel TrainingsViewModel { get;}
        public MoneyTransactionsViewModel MoneyTransactionsViewModel { get;}
        public BusinessOperationsViewModel BusinessOperationsViewModel { get;}
        public BalanceReplenishmentsViewModel BalanceReplenishmentsViewModel { get;}
        public BalanceWithdrawingsViewModel BalanceWithdrawingsViewModel { get;}

        private Mediator _mediator;

        public DayOperationsViewModel(Mediator mediator,
            TrainingsViewModel trainingsViewModel,
            MoneyTransactionsViewModel moneyTransactionsViewModel,
            BusinessOperationsViewModel businessOperationsViewModel,
            BalanceReplenishmentsViewModel balanceReplenishmentsViewModel,
            BalanceWithdrawingsViewModel balanceWithdrawingsViewModel)
        {
            _mediator = mediator;
            _mediator.NeedToCreateTrainingWithdrawing += OpenDayOperationsTab;

            CurrentDate = DateTime.Now.Date;
            TrainingsViewModel = trainingsViewModel;
            MoneyTransactionsViewModel = moneyTransactionsViewModel;
            BusinessOperationsViewModel = businessOperationsViewModel;
            BalanceReplenishmentsViewModel = balanceReplenishmentsViewModel;
            BalanceWithdrawingsViewModel = balanceWithdrawingsViewModel;

        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
                UpdateDate();
            }
        }

        private void UpdateDate()
        {
            _mediator.UpdateDayOperationsDate(CurrentDate);
        }

        #region Взаимодействие с календарем

        public DelegateCommand GetLastWeekCommand
        {
            get
            {
                return new DelegateCommand(o => { GetLastWeek(); });
            }
        }
        public DelegateCommand GetYesterdayCommand
        {
            get
            {
                return new DelegateCommand(o => { GetYesterday(); });
            }
        }
        public DelegateCommand GetTodayCommand
        {
            get
            {
                return new DelegateCommand(o => { GetToday(); });
            }
        }
        public DelegateCommand GetTomorrowCommand
        {
            get
            {
                return new DelegateCommand(o => { GetTomorrow(); });
            }
        }
        public DelegateCommand GetNextWeekCommand
        {
            get
            {
                return new DelegateCommand(o => { GetNextWeek(); });
            }
        }
        private void GetLastWeek() => CurrentDate = CurrentDate.Date.AddDays(-7);
        private void GetYesterday() => CurrentDate = CurrentDate.Date.AddDays(-1);
        private void GetToday()
        {
            if (CurrentDate.Date != DateTime.Now.Date)
                CurrentDate = DateTime.Now;
        }
        private void GetTomorrow() => CurrentDate = CurrentDate.Date.AddDays(1);
        private void GetNextWeek() => CurrentDate = CurrentDate.Date.AddDays(7);
        #endregion


        private int _SelectedTab;
        public int SelectedTab
        {
            get { return _SelectedTab; }
            set { _SelectedTab = value; OnPropertyChanged(); }
        }

        private void ChangeTab(int tabIndex) => SelectedTab = tabIndex;
        private void OpenDayOperationsTab(object o) => ChangeTab(4);
    }
}
