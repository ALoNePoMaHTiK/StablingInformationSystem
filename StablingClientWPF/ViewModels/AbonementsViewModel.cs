using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Helpers.Commands;
using StablingClientWPF.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class AbonementsViewModel : BaseViewModel
    {
        private readonly AbonementsHttpClient _abonementsHttpClient;
        private readonly ClientsHttpClient _clientsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;
        private readonly BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient;
        private readonly BalanceReplenishmentsHttpClient _balanceReplenishmentsHttpClient;

        private readonly Mediator _mediator;
        public AbonementsViewModel(Mediator mediator, AbonementsHttpClient abonementsHttpClient,
            ClientsHttpClient clientsHttpClient, TrainersHttpClient trainersHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient, 
            BalanceReplenishmentsHttpClient balanceReplenishmentsHttpClient)
        {
            _mediator = mediator;
            _abonementsHttpClient = abonementsHttpClient;
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
            _balanceReplenishmentsHttpClient = balanceReplenishmentsHttpClient;
            GetAbonements();
        }

        private ObservableCollection<AbonementForShow> _ActiveAbonements;
        public ObservableCollection<AbonementForShow> ActiveAbonements
        {
            get { return _ActiveAbonements; }
            set { _ActiveAbonements = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AbonementForShow> _InactiveAbonements;
        public ObservableCollection<AbonementForShow> InactiveAbonements
        {
            get { return _InactiveAbonements; }
            set { _InactiveAbonements = value; OnPropertyChanged(); }
        }

        public AsyncDelegateCommand GetAbonementsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await GetAbonements(); },
                    ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetAbonements()
        {
            ActiveAbonements = new ObservableCollection<AbonementForShow>(
                await _abonementsHttpClient.GetForShowByAvailabilityAsync(true));
            InactiveAbonements = new ObservableCollection<AbonementForShow>(
                await _abonementsHttpClient.GetForShowByAvailabilityAsync(false));
        }

        public AsyncDelegateCommand CreateAbonementCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await CreateAbonement(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task CreateAbonement()
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementCreationForm(
                    new AbonementCreationFormViewModel(
                    new Abonement()
                    {
                        OpenDateTime = DateTime.Now,
                        IsAvailable = true,
                    },
                    await _clientsHttpClient.GetByAvailabilityAsync(true),
                    await _abonementsHttpClient.GetTypesAsync(),
                    await _trainersHttpClient.GetAllAsync())), ROOT_IDENTIFIER);
            if (result != null)
            {
                await _abonementsHttpClient.CreateAsync((Abonement)result);
                await GetAbonements();
            }
        }

        #region Закрытие/открытие абонемента
        public AsyncDelegateCommand ChangeAvailabilityCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await ChangeAvailability((int)o); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangeAvailability(int abonementId)
        {
            AbonementForShow abonementForWork = ActiveAbonements.FirstOrDefault(o => o.AbonementId == abonementId);
            abonementForWork ??= InactiveAbonements.First(o => o.AbonementId == abonementId);
            if (abonementForWork.IsAvailable)
            {
                ActiveAbonements.Remove(abonementForWork);  //Проблема с отображением при перемещеннии из групп
                InactiveAbonements.Add(abonementForWork);
            }
            else
            {
                InactiveAbonements.Remove(abonementForWork);
                ActiveAbonements.Add(abonementForWork);
            }
            await _abonementsHttpClient.ChangeAvailabilityAsync(abonementId);
        }
        #endregion

        #region Изменение статуса оплаты абонемента
        public AsyncDelegateCommand ChangePaidStatusCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await ChangePaidStatus((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task ChangePaidStatus(int abonementId)
        {
            await _abonementsHttpClient.ChangePaidStatusAsync(abonementId);
            AbonementForShow oldAbonement = ActiveAbonements.Where(a => a.AbonementId == abonementId).First();
            AbonementForShow newAbonement = oldAbonement;
            newAbonement.IsPaid = !newAbonement.IsPaid;
            ActiveAbonements.Remove(oldAbonement);
            ActiveAbonements.Add(newAbonement);
        }
        #endregion

        #region Удаление абонемента
        public AsyncDelegateCommand DeleteAbonementCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await DeleteAbonementAsync((int)o); },
                    ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task DeleteAbonementAsync(int abonementId)
        {
            AbonementForShow abonementForDelete = ActiveAbonements.Where(ab =>
            ab.AbonementId == abonementId).FirstOrDefault();
            if (abonementForDelete == null)
            {
                abonementForDelete = InactiveAbonements.Where(ab => ab.AbonementId == abonementId).FirstOrDefault();
                InactiveAbonements.Remove(abonementForDelete);
            }
            else
                ActiveAbonements.Remove(abonementForDelete);
            await _abonementsHttpClient.DeleteAsync(abonementId);
        }
        #endregion


        #region Детали

        public DelegateCommand OpenAbonementDetailsDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenAbonementDetailsDialogAsync((int)o);
                });
            }
        }
        private async Task OpenAbonementDetailsDialogAsync(int abonementId)
        {
            await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementsDetailsDialog(new AbonementsDetailsDialogViewModel(_mediator,
                    _abonementsHttpClient, _balanceWithdrawingsHttpClient, DateTime.Now, abonementId)), ROOT_IDENTIFIER);
        }

        #endregion

        #region Полная оплата
        public AsyncDelegateCommand MakeFullPaymentCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => {
                    await MakeFullPayment((int)o);
                }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task MakeFullPayment(int abonementId)
        {
            AbonementForShow abonement = ActiveAbonements.Where(ab => ab.AbonementId == abonementId).First();
            AbonementType abonementType = await _abonementsHttpClient.GetTypeAsync(abonement.AbonementTypeId);
            BalanceReplenishment newReplenishment = new BalanceReplenishment()
            {
                TrainerId = abonement.TrainerId,
                ClientId = abonement.ClientId,
                ReplenishmentDate = DateTime.Now.Date,
                Amount = abonementType.TypePrice
            };
            await _balanceReplenishmentsHttpClient.CreateAsync(newReplenishment);
            BalanceWithdrawing newWithdrawing = new BalanceWithdrawing()
            {
                ClientId = abonement.ClientId,
                TrainerId = abonement.TrainerId,
                WithdrawingCause = "Abonement",
                WithdrawingDate = DateTime.Now.Date,
                Amount = abonementType.TypePrice
            };
            await _balanceWithdrawingsHttpClient.CreateByAbonementAsync(newWithdrawing, abonementId);
            await ChangePaidStatus(abonementId);
            await GetAbonements();
            // TODO Добавить в медиатор уведомления о создании списаний/пополнений
        }

        #endregion
    }
}
