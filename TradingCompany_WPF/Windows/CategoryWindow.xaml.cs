using System.Windows;
using TradingCompany_WPF.Views;

namespace TradingCompany_WPF.Windows
{
    public partial class CategoryWindow : Window
    {
        public CategoryWindow(CategoryView categoryView)
        {
            InitializeComponent();
            Content = categoryView;
        }
    }
}