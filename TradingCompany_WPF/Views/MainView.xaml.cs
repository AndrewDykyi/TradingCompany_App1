using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}