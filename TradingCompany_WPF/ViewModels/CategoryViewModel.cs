using BusinessLogic.Interface;
using DAL.Concrete;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TradingCompany_WPF.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly ICategoryService _categoryService;

        private List<CategoryDto> _categories;
        private CategoryDto _selectedCategory;
        private string _newCategoryName;

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            LoadCategoriesCommand = new RelayCommand(async () => await LoadCategories());
            AddCategoryCommand = new RelayCommand(AddCategory, CanAddCategory);
            UpdateCategoryCommand = new RelayCommand(UpdateCategory, CanUpdateCategory);
            RemoveCategoryCommand = new RelayCommand(async () => await RemoveCategory(), CanRemoveCategory);

            _ = LoadCategories();
        }

        public List<CategoryDto> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(IsCategorySelected));

                (AddCategoryCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (UpdateCategoryCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (RemoveCategoryCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string NewCategoryName
        {
            get => _newCategoryName;
            set
            {
                _newCategoryName = value;
                OnPropertyChanged(nameof(NewCategoryName));
                (AddCategoryCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public bool IsCategorySelected => SelectedCategory != null;

        public ICommand LoadCategoriesCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand RemoveCategoryCommand { get; }

        private async Task LoadCategories()
        {
            Categories = await Task.Run(() => _categoryService.GetAllCategories().Where(c => !c.IsDeleted).ToList());
        }

        private void AddCategory()
        {
            if (!string.IsNullOrWhiteSpace(NewCategoryName))
            {
                var newCategory = new CategoryDto
                {
                    CategoryName = NewCategoryName
                };

                _categoryService.AddCategory(newCategory);

                NewCategoryName = string.Empty;

                _ = LoadCategories();
            }
        }

        private bool CanAddCategory()
        {
            return !string.IsNullOrWhiteSpace(NewCategoryName);
        }

        private void UpdateCategory()
        {
            if (SelectedCategory != null && !string.IsNullOrWhiteSpace(NewCategoryName))
            {
                SelectedCategory.CategoryName = NewCategoryName;

                _categoryService.UpdateCategory(SelectedCategory);

                _ = LoadCategories();
                NewCategoryName = string.Empty;
            }
        }

        private async Task RemoveCategory()
        {
            if (SelectedCategory != null)
            {
                await _categoryService.RemoveCategoryAsync(SelectedCategory.CategoryID);

                _ = LoadCategories();
            }
        }

        private bool CanRemoveCategory()
        {
            return SelectedCategory != null;
        }

        private bool CanUpdateCategory()
        {
            return SelectedCategory != null && !string.IsNullOrWhiteSpace(NewCategoryName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}