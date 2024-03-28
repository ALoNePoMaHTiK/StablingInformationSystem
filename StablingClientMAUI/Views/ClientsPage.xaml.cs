using StablingClientMAUI.ViewModels;

namespace StablingClientMAUI.Views;

public partial class ClientsPage : ContentPage
{
    public ClientsPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}