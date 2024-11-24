using System.Windows;
using TradingCompany_WPF.Views;

namespace TradingCompany_WPF.Windows
{
    public partial class ProductWindow : Window
    {
        public ProductWindow(ProductView productView)
        {
            InitializeComponent();
            Content = productView;
        }
    }
}