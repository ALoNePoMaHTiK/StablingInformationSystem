using StablingClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.Models
{
    public class MoneyDayInfo : BaseViewModel
    {
        private double _BalanceReplenishmentSum;
        public double BalanceReplenishmentSum
        {
            get => _BalanceReplenishmentSum;
            set { _BalanceReplenishmentSum = value; OnPropertyChanged(); }
        }
        private double _MoneyTransactionsSum;
        public double MoneyTransactionsSum
        {
            get => _MoneyTransactionsSum;
            set { _MoneyTransactionsSum = value; OnPropertyChanged(); }
        }
    }
}
