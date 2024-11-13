using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.Views;
using StablingClientWPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class AbonementsDetailsDialogViewModel : BaseViewModel
    {
        private readonly AbonementsHttpClient _abonementsHttpClient;
        private readonly BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient;

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
            }
        }

        private readonly Mediator _mediator;

        public AbonementsDetailsDialogViewModel(Mediator mediator,
            AbonementsHttpClient abonementsHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient,
            DateTime currentDate, int abonementId)
        {
            _mediator = mediator;
            _abonementsHttpClient = abonementsHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
            CurrentDate = currentDate;
            Abonement = _abonementsHttpClient.GetForShowAsync(abonementId).Result;
            GetWithdrawings();
        }

        public AsyncDelegateCommand CreateAbonementWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await CreateAbonementWithdrawing(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task CreateAbonementWithdrawing()
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new WithdrawingByAbonementDialog(
                    new WithdrawingByAbonementDialogViewModel(CurrentDate, Abonement)), ABONEMENTS_IDENTIFIER);
            if (result != null)
            {
                await _balanceWithdrawingsHttpClient.CreateByAbonementAsync((BalanceWithdrawing)result, Abonement.AbonementId);
                await GetWithdrawings();
                _mediator.UpdateAbonementFunds(Abonement.AbonementId); //Уведомление основной viewModel об изменении
            }
        }

        public AsyncDelegateCommand UpdateAbonementWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await UpdateAbonementWithdrawing((Guid)o); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task UpdateAbonementWithdrawing(Guid id)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new WithdrawingByAbonementDialog(
                    new WithdrawingByAbonementDialogViewModel(await _balanceWithdrawingsHttpClient.GetAsync(id),
                    await _abonementsHttpClient.GetForShowAsync(Abonement.AbonementId))), ABONEMENTS_IDENTIFIER);
            if (result != null)
            {
                await _balanceWithdrawingsHttpClient.UpdateAsync((BalanceWithdrawing)result);
                BalanceWithdrawingForShow oldWithdrawing = Withdrawings.Where(
                    w => w.BalanceWithdrawingId == ((BalanceWithdrawing)result).BalanceWithdrawingId).First();
                Withdrawings.Remove(oldWithdrawing);
                Withdrawings.Add(
                    await _balanceWithdrawingsHttpClient.GetForShowAsync(
                        ((BalanceWithdrawing)result).BalanceWithdrawingId));
                _mediator.UpdateAbonementFunds(Abonement.AbonementId); //Уведомление основной viewModel об изменении
            }
        }

        public AsyncDelegateCommand DeleteWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o =>
                {
                    await DeleteWithdrawing((Guid)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteWithdrawing(Guid id)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _balanceWithdrawingsHttpClient.DeleteAsync(id);
                BalanceWithdrawingForShow oldWithdrawing = Withdrawings.Where(w => w.BalanceWithdrawingId == id).First();
                Withdrawings.Remove(oldWithdrawing);
            }
        }

        private ObservableCollection<BalanceWithdrawingForShow> _Withdrawings;
        public ObservableCollection<BalanceWithdrawingForShow> Withdrawings
        {
            get => _Withdrawings;
            set { _Withdrawings = value; OnPropertyChanged(); }
        }
        private async Task GetWithdrawings()
        {
            Withdrawings = new ObservableCollection<BalanceWithdrawingForShow>(
                await _balanceWithdrawingsHttpClient.GetForShowByAbonementAsync(Abonement.AbonementId));
        }

        private AbonementForShow _Abonement;
        public AbonementForShow Abonement
        {
            get => _Abonement;
            set { _Abonement = value; OnPropertyChanged(); }
        }
    }
}
