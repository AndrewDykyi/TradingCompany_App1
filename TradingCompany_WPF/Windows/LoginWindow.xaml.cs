using System.Windows;
using TradingCompany_WPF.Views;

namespace TradingCompany_WPF.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginView loginView)
        {
            InitializeComponent();
            Content = loginView;
        }
    }
}