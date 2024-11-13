using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.Views;
using StablingClientWPF.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class TrainingsDetailsDialogViewModel : BaseViewModel
    {
        public TrainingsHttpClient _trainingsHttpClient { get; }
        public BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient { get; }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set
            {
                _CurrentDate = value; OnPropertyChanged();
            }
        }

        public Mediator _mediator { get; }

        public TrainingsDetailsDialogViewModel(Mediator mediator, TrainingsHttpClient trainingsHttpClient, BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient,
            DateTime currentDate, int trainingId)
        {
            _mediator = mediator;
            _trainingsHttpClient = trainingsHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
            CurrentDate = currentDate;
            Training = _trainingsHttpClient.GetForShowAsync(trainingId).Result;
            Withdrawings = new ObservableCollection<BalanceWithdrawingForShow>(
                _balanceWithdrawingsHttpClient.GetForShowByTrainingAsync(trainingId).Result);
        }

        public AsyncDelegateCommand CreateTrainingWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await CreateTrainingWithdrawing(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task CreateTrainingWithdrawing()
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new WithdrawingByTrainingDialog(
                    new WithdrawingByTrainingDialogViewModel(CurrentDate, Training)), DAY_OPERATIONS_IDENTIFIER);
            if (result != null)
            {
                await _balanceWithdrawingsHttpClient.CreateByTrainingAsync((BalanceWithdrawing)result, Training.TrainingId);
                await GetWithdrawings();
                _mediator.UpdateTrainingFunds(Training.TrainingId); //Уведомление основной viewModel об изменении
            }
        }

        public AsyncDelegateCommand UpdateTrainingWithdrawingCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await UpdateTrainingWithdrawing((Guid)o); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task UpdateTrainingWithdrawing(Guid id)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new WithdrawingByTrainingDialog(
                    new WithdrawingByTrainingDialogViewModel(await _balanceWithdrawingsHttpClient.GetAsync(id), await _trainingsHttpClient.GetForShowAsync(Training.TrainingId))), DAY_OPERATIONS_IDENTIFIER);
            if (result != null)
            {
                await _balanceWithdrawingsHttpClient.UpdateAsync((BalanceWithdrawing)result);
                BalanceWithdrawingForShow oldWithdrawing = Withdrawings.Where(
                    w => w.BalanceWithdrawingId == ((BalanceWithdrawing)result).BalanceWithdrawingId).First();
                Withdrawings.Remove(oldWithdrawing);
                Withdrawings.Add(
                    await _balanceWithdrawingsHttpClient.GetForShowAsync(
                        ((BalanceWithdrawing)result).BalanceWithdrawingId));
                _mediator.UpdateTrainingFunds(Training.TrainingId); //Уведомление основной viewModel об изменении
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
                await _balanceWithdrawingsHttpClient.GetForShowByTrainingAsync(Training.TrainingId));
        }

        private TrainingForShow _Training;
        public TrainingForShow Training
        {
            get => _Training;
            set { _Training = value; OnPropertyChanged(); }
        }
    }
}
