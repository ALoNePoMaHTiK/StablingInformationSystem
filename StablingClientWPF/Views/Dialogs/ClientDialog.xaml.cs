using StablingClientWPF.ViewModels.Dialogs;
using System.Windows.Controls;

namespace StablingClientWPF.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ClientDialog.xaml
    /// </summary>
    public partial class ClientDialog : UserControl
    {
        public ClientDialog(ClientDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
