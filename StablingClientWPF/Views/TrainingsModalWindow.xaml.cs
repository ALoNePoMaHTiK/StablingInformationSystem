using StablingClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StablingClientWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для TrainingsModalWindow.xaml
    /// </summary>
    public partial class TrainingsModalWindow : Window
    {
        public TrainingsModalWindow(EditTrainingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
