﻿using StablingClientWPF.ViewModels.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StablingClientWPF.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AbonementUsageDialog.xaml
    /// </summary>
    public partial class AbonementUsageDialog : UserControl
    {
        public AbonementUsageDialog(AbonementUsageDialogViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
