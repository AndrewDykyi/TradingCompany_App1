using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TradingCompany_WPF.ViewModels;
using TradingCompany_WPF.Views;
using TradingCompany_WPF.Windows;
using BusinessLogic.Interface;
using BusinessLogic.Concrete;
using DAL.Interface;
using DAL.Concrete;

namespace TradingCompany_WPF
{
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider ServiceProvider => _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=TradingCompany_db;Integrated Security=True;TrustServerCertificate=True;";

            services.AddTransient<IUserDal>(sp => new UserDal(connectionString));
            services.AddTransient<IProductDal>(sp => new ProductDal(connectionString));
            services.AddTransient<ICategoryDal>(sp => new CategoryDal(connectionString));

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<ProductViewModel>();

            services.AddTransient<LoginView>();
            services.AddTransient<MainView>();
            services.AddTransient<CategoryView>();
            services.AddTransient<ProductView>();

            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<CategoryWindow>();
            services.AddTransient<ProductWindow>();
        }
    }
}