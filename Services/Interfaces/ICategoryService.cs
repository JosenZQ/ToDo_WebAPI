using Domain.DTOs;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryFormat>> GetCategoriesList();
        Task<CategoryFormat> GetCategoryByCode(string pCategoryCode);
        Task<bool> CreateNewCategory(string pCategoryName);
        Task<bool> UpdateCategory(CategoryFormat pCategory);
        Task<bool> DeleteCategory(string pCategoryCode);
    }
}
