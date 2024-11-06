﻿using StablingApiClient;
using StablingClientWPF.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StablingClientWPF
{
    //TODO Реализовать возможность указания адреса API через модульное окно/вкладку
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }   
    }
}