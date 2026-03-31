using HotellDB.ViewModels;
using HotellDB.Services;
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

namespace HotellDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
{
    InitializeComponent();
    NavigationService.NavigateTo += OnNavigate;
    NavigationService.Navigate(new LoginViewModel());
}

private void OnNavigate(object viewModel)
{
    var view = new ContentControl { Content = CreateViewForViewModel(viewModel) };
    MainContent.Content = view;
}

private FrameworkElement CreateViewForViewModel(object viewModel)
{
    // Здесь можно использовать DataTemplate или создавать View динамически
    // Для простоты создадим UserControl
    return new System.Windows.Controls.UserControl { DataContext = viewModel };
}
    }
}
