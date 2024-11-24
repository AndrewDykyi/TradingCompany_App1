using BusinessLogic.Interface;
using DAL.Concrete;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TradingCompany_WPF.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly IProductService _productService;

        private ProductDto _selectedProduct;
        private List<ProductDto> _products;

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;

            LoadProductsCommand = new RelayCommand(async () => await LoadProducts());
            UpdateProductCommand = new RelayCommand(async () => await UpdateProduct(), CanUpdateOrRemoveProduct);
            RemoveProductCommand = new RelayCommand(async () => await RemoveProduct(), CanUpdateOrRemoveProduct);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<ProductDto> Products
        {
            get => _products;
            private set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ProductDto SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(IsProductSelected));

                (UpdateProductCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (RemoveProductCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public bool IsProductSelected => SelectedProduct != null;

        public ICommand LoadProductsCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand RemoveProductCommand { get; }

        private async Task LoadProducts()
        {
            Products = await _productService.GetAllProductsAsync();
        }

        private async Task UpdateProduct()
        {
            if (SelectedProduct != null)
            {
                await _productService.UpdateProductAsync(SelectedProduct);
                await LoadProducts();
            }
        }

        private async Task RemoveProduct()
        {
            if (SelectedProduct != null)
            {
                await _productService.RemoveProductAsync(SelectedProduct.ProductID);
                await LoadProducts();
            }
        }

        private bool CanUpdateOrRemoveProduct()
        {
            return SelectedProduct != null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}