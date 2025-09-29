using Infrastructure.Entities;

namespace Infrastructure.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> getCategoriesList();
        Task<Category> getCategoryByCode(string pCategoryCode);
        Task createNewCategory(Category pNewCategory);
        Task updateCategoryData(Category pCategory);
        Task deleteCategory(string pCategoryCode);
    }
}
