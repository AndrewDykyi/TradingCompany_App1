using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();

            DataContext = App.ServiceProvider.GetRequiredService<CategoryViewModel>();
        }
    }
}