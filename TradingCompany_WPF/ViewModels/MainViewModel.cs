using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using TradingCompany_WPF.Windows;

namespace TradingCompany_WPF.ViewModels
{
    public class MainViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public ICommand OpenProductWindowCommand { get; }
        public ICommand OpenCategoryWindowCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            OpenProductWindowCommand = new RelayCommand(OpenProductWindow);
            OpenCategoryWindowCommand = new RelayCommand(OpenCategoryWindow);
            LogoutCommand = new RelayCommand(Logout);
        }

        private void OpenProductWindow()
        {
            var productWindow = _serviceProvider.GetRequiredService<ProductWindow>();
            productWindow.Show();
            Application.Current.MainWindow.Close();
        }

        private void OpenCategoryWindow()
        {
            var categoryWindow = _serviceProvider.GetRequiredService<CategoryWindow>();
            categoryWindow.Show();
            Application.Current.MainWindow.Close();
        }

        private void Logout()
        {
            Application.Current.Shutdown();
        }
    }
}
