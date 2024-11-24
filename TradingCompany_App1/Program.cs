using BusinessLogic.Concrete;
using DAL.Concrete;

class Program
{
    static string connStr = "Data Source=localhost;Initial Catalog=TradingCompany_db;Integrated Security=True;TrustServerCertificate=True;";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to Trading Company!\n");
        int option = -1;

        while (option != 0)
        {
            Console.WriteLine("Please select option:");
            Console.WriteLine("1. - List all Categories");
            Console.WriteLine("2. - Get Category by Id");
            Console.WriteLine("3. - Add Category");
            Console.WriteLine("4. - Update Category");
            Console.WriteLine("5. - Delete Category");
            Console.WriteLine("6. - List all Users");
            Console.WriteLine("7. - Add User");
            Console.WriteLine("8. - Update User");
            Console.WriteLine("9. - Delete User");
            Console.WriteLine("10. - List all Category Managements");
            Console.WriteLine("11. - Add Category Management");
            Console.WriteLine("12. - Delete Category Management");
            Console.WriteLine("13. - List all Product Managements");
            Console.WriteLine("14. - Add Product Management");
            Console.WriteLine("15. - Delete Product Management");
            Console.WriteLine("16. - List all Actions");
            Console.WriteLine("17. - Add Action");
            Console.WriteLine("18. - Delete Action");
            Console.WriteLine("0. - Quit\n");

            string selectedOption = Console.ReadLine() ?? "";
            bool isValidOption = int.TryParse(selectedOption, out option);

            if (!isValidOption || option < 0 || option > 18)
            {
                Console.WriteLine("Incorrect option selected!");
                continue;
            }

            switch (option)
            {
                case 1:
                    ListAllCategories();
                    break;
                case 2:
                    GetCategoryById();
                    break;
                case 3:
                    AddCategory();
                    break;
                case 4:
                    UpdateCategory();
                    break;
                case 5:
                    DeleteCategory();
                    break;
                case 6:
                    await ListAllUsers();
                    break;
                case 7:
                    await AddUser();
                    break;
                case 8:
                    await UpdateUser();
                    break;
                case 9:
                    await DeleteUser();
                    break;
                case 10:
                    ListAllCategoryManagement();
                    break;
                case 11:
                    AddCategoryManagement();
                    break;
                case 12:
                    DeleteCategoryManagement();
                    break;
                case 13:
                    ListAllProductManagement();
                    break;
                case 14:
                    AddProductManagement();
                    break;
                case 15:
                    DeleteProductManagement();
                    break;
                case 16:
                    ListAllActions();
                    break;
                case 17:
                    AddAction();
                    break;
                case 18:
                    DeleteAction();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Incorrect option selected!");
                    break;
            }
        }
    }

    static void ListAllCategories()
    {
        var categoryDal = new CategoryDal(connStr);
        List<CategoryDto> categories = categoryDal.GetAll();
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryID}.\t{category.CategoryName}");
        }
    }

    static void GetCategoryById()
    {
        Console.WriteLine("Please enter Category ID:");
        int categoryId = Convert.ToInt32(Console.ReadLine());

        var categoryDal = new CategoryDal(connStr);
        CategoryDto category = categoryDal.GetById(categoryId);

        Console.WriteLine($"{category.CategoryID}.\t{category.CategoryName}");
    }

    static void AddCategory()
    {
        Console.WriteLine("Please enter Category Name:");
        string categoryName = Console.ReadLine();

        var category = new CategoryDto
        {
            CategoryName = categoryName,
            IsDeleted = false
        };

        var categoryDal = new CategoryDal(connStr);
        categoryDal.Insert(category);
        Console.WriteLine($"New Category: {category.CategoryID}.\t{category.CategoryName}");
    }

    static void UpdateCategory()
    {
        Console.WriteLine("Please enter Category ID to update:");
        int categoryId = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter new Category Name:");
        string newCategoryName = Console.ReadLine();

        var categoryDal = new CategoryDal(connStr);
        var category = categoryDal.GetById(categoryId);
        if (category != null)
        {
            category.CategoryName = newCategoryName;
            categoryDal.Update(category);
            Console.WriteLine("Category updated successfully.");
        }
        else
        {
            Console.WriteLine("Category not found.");
        }
    }

    static void DeleteCategory()
    {
        Console.WriteLine("Please enter Category ID to delete:");
        int categoryId = Convert.ToInt32(Console.ReadLine());

        var categoryDal = new CategoryDal(connStr);
        categoryDal.Delete(categoryId);
        Console.WriteLine("Category deleted successfully.");
    }

    static async Task ListAllUsers()
    {
        try
        {
            var userDal = new UserDal(connStr);
            var users = await userDal.GetAllAsync();

            if (users.Any())
            {
                Console.WriteLine("List of Users:");
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.UserID}.\t{user.Username}");
                }
            }
            else
            {
                Console.WriteLine("No users found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while listing users: {ex.Message}");
        }
    }

    static async Task AddUser()
    {
        try
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                return;
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty.");
                return;
            }

            var userDal = new UserDal(connStr);
            var authService = new AuthService(userDal);

            var existingUser = await userDal.GetByUsernameAsync(username);
            if (existingUser != null)
            {
                Console.WriteLine("A user with this username already exists.");
                return;
            }

            var (passwordHash, passwordSalt) = authService.CreatePasswordHash(password);

            var newUser = new UserDto
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await userDal.InsertAsync(newUser, password);
            Console.WriteLine("User added successfully. UserID: " + newUser.UserID);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding user: {ex.Message}");
        }
    }

    static async Task UpdateUser()
    {
        try
        {
            Console.Write("Enter User ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid User ID.");
                return;
            }

            var userDal = new UserDal(connStr);
            var authService = new AuthService(userDal);

            var user = await userDal.GetByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Console.Write("Enter new Username (leave blank to keep current): ");
            string newUsername = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUsername))
            {
                user.Username = newUsername;
            }

            Console.Write("Enter new Password (leave blank to keep current): ");
            string newPassword = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var (passwordHash, passwordSalt) = authService.CreatePasswordHash(newPassword);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            await userDal.UpdateAsync(user, newPassword);
            Console.WriteLine("User updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating user: {ex.Message}");
        }
    }

    static async Task DeleteUser()
    {
        try
        {
            Console.Write("Enter User ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid User ID.");
                return;
            }

            var userDal = new UserDal(connStr);
            var user = await userDal.GetByIdAsync(userId);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            await userDal.DeleteAsync(userId);
            Console.WriteLine("User deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting user: {ex.Message}");
        }
    }

    static void ListAllCategoryManagement()
    {
        var categoryManagementDal = new CategoryManagementDal(connStr);
        List<CategoryManagementDto> categoryManagements = categoryManagementDal.GetAll();
        foreach (var cm in categoryManagements)
        {
            Console.WriteLine($"{cm.CategoryID}.\tCategoryID: {cm.CategoryID}, UserID: {cm.UserID}, ActionID: {cm.ActionID}, Action Date: {cm.ActionDate}");
        }
    }

    static void AddCategoryManagement()
    {
        Console.WriteLine("Please enter User ID:");
        int userId = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter Category ID:");
        int categoryId = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter Action ID:");
        int actionId = Convert.ToInt32(Console.ReadLine());

        var categoryManagement = new CategoryManagementDto
        {
            UserID = userId,
            CategoryID = categoryId,
            ActionID = actionId,
            ActionDate = DateTime.Now
        };

        var categoryManagementDal = new CategoryManagementDal(connStr);
        categoryManagementDal.Insert(categoryManagement);

        Console.WriteLine($"New CategoryManagement added with CategoryID: {categoryManagement.CategoryID}");
    }

    static void DeleteCategoryManagement()
    {
        Console.WriteLine("Please enter CategoryManagement ID to delete:");
        int categoryManagementId = Convert.ToInt32(Console.ReadLine());

        var categoryManagementDal = new CategoryManagementDal(connStr);
        categoryManagementDal.Delete(categoryManagementId);

        Console.WriteLine("CategoryManagement deleted successfully.");
    }

    static void ListAllProductManagement()
    {
        var productManagementDal = new ProductManagementDal(connStr);
        List<ProductManagementDto> productManagements = productManagementDal.GetAll();
        foreach (var pm in productManagements)
        {
            Console.WriteLine($"{pm.ProductID}.\tProductID: {pm.ProductID}, UserID: {pm.UserID}, ActionID: {pm.ActionID}, Action Date: {pm.ActionDate}");
        }
    }

    static void AddProductManagement()
    {
        Console.WriteLine("Please enter User ID:");
        int userId = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter Product ID:");
        int productId = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter Action ID:");
        int actionId = Convert.ToInt32(Console.ReadLine());

        var productManagement = new ProductManagementDto
        {
            UserID = userId,
            ProductID = productId,
            ActionID = actionId,
            ActionDate = DateTime.Now
        };

        var productManagementDal = new ProductManagementDal(connStr);
        productManagementDal.Insert(productManagement);

        Console.WriteLine($"New ProductManagement added with ProductID: {productManagement.ProductID}");
    }

    static void DeleteProductManagement()
    {
        Console.WriteLine("Please enter ProductManagement ID to delete:");
        int productManagementId = Convert.ToInt32(Console.ReadLine());

        var productManagementDal = new ProductManagementDal(connStr);
        productManagementDal.Delete(productManagementId);

        Console.WriteLine("ProductManagement deleted successfully.");
    }

    static void ListAllActions()
    {
        var actionDal = new ActionDal(connStr);
        List<ActionDto> actions = actionDal.GetAll();
        foreach (var action in actions)
        {
            Console.WriteLine($"{action.ActionID}.\t{action.ActionName}");
        }
    }

    static void AddAction()
    {
        Console.WriteLine("Please enter Action Name:");
        string actionName = Console.ReadLine();

        var action = new ActionDto
        {
            ActionName = actionName
        };

        var actionDal = new ActionDal(connStr);
        actionDal.Insert(action);

        Console.WriteLine($"New Action added with ID: {action.ActionID}");
    }

    static void DeleteAction()
    {
        Console.WriteLine("Please enter Action ID to delete:");
        int actionId = Convert.ToInt32(Console.ReadLine());

        var actionDal = new ActionDal(connStr);
        actionDal.Delete(actionId);

        Console.WriteLine("Action deleted successfully.");
    }
}