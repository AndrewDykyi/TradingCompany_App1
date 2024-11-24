using System.Windows;
using TradingCompany_WPF.Views;

namespace TradingCompany_WPF.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainView mainView)
        {
            InitializeComponent();
            Content = mainView;
        }
    }
}