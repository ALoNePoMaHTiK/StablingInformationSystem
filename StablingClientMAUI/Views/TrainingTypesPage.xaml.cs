using StablingClientMAUI.ViewModels;

namespace StablingClientMAUI.Views;

public partial class TrainingTypesPage : ContentPage
{
	public TrainingTypesPage(TrainingTypesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

    }
}