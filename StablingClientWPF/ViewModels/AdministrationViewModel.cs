namespace StablingClientWPF.ViewModels
{
    public class AdministrationViewModel : BaseViewModel
    {
        public TrainingTypesViewModel TrainingTypesViewModel { get; }

        public AdministrationViewModel(TrainingTypesViewModel _trainingTypesViewModel)
        {
            TrainingTypesViewModel = _trainingTypesViewModel;
        }
    }
}