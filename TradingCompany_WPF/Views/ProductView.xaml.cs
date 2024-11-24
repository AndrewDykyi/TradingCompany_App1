using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class ProductView : UserControl
    {
        public ProductView()
        {
            InitializeComponent();

            DataContext = App.ServiceProvider.GetRequiredService<ProductViewModel>();
        }
    }
}