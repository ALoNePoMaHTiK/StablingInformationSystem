using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.ViewModels
{
    public class DayOperationsViewModel : BaseViewModel
    {
        public TrainingsViewModel TrainingsViewModel { get;}
        public MoneyTransactionsViewModel MoneyTransactionsViewModel { get;}
        public BusinessOperationsViewModel BusinessOperationsViewModel { get;}
        public BalanceReplenishmentsViewModel BalanceReplenishmentsViewModel { get;}

        private Mediator _mediator;

        public DayOperationsViewModel(Mediator mediator,
            TrainingsViewModel trainingsViewModel,
            MoneyTransactionsViewModel moneyTransactionsViewModel,
            BusinessOperationsViewModel businessOperationsViewModel,
            BalanceReplenishmentsViewModel balanceReplenishmentsViewModel)
        {
            _mediator = mediator;
            TrainingsViewModel = trainingsViewModel;

            CurrentDate = DateTime.Now;
            MoneyTransactionsViewModel = moneyTransactionsViewModel;
            BusinessOperationsViewModel = businessOperationsViewModel;
            BalanceReplenishmentsViewModel = balanceReplenishmentsViewModel;

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
        private void GetToday() => CurrentDate = DateTime.Now;
        private void GetTomorrow() => CurrentDate = CurrentDate.Date.AddDays(1);
        private void GetNextWeek() => CurrentDate = CurrentDate.Date.AddDays(7);
        #endregion
    }
}
