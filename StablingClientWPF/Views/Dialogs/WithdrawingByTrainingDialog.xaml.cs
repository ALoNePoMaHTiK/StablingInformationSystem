using StablingClientWPF.ViewModels.Dialogs;
using System.Windows.Controls;

namespace StablingClientWPF.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для WithdrawingByTrainingDialog.xaml
    /// </summary>
    public partial class WithdrawingByTrainingDialog : UserControl
    {
        public WithdrawingByTrainingDialog(WithdrawingByTrainingDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
