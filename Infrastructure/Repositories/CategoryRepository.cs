using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TaskListDbContext gDbContext;

        public CategoryRepository(TaskListDbContext pDbContext)
        {
            gDbContext = pDbContext;
        }

        public async Task<List<Category>> getCategoriesList()
        {
            return await gDbContext.Categories.ToListAsync();
        }

        public async Task<Category> getCategoryByCode(string pCategoryCode)
        {
            return await gDbContext.Categories.FirstOrDefaultAsync(x => x.CategoryCode == pCategoryCode);
        }

        public async Task createNewCategory(Category pNewCategory)
        {
            await gDbContext.Categories.AddAsync(pNewCategory);
            gDbContext.SaveChanges();
        }

        public async Task updateCategoryData(Category pCategory)
        {
            var lCategoryFound = await gDbContext.Categories.FirstOrDefaultAsync(x => x.CategoryCode == pCategory.CategoryCode);
            if (lCategoryFound != null)
            {
                lCategoryFound.Category1 = pCategory.Category1;
                await gDbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Category not found.");
            }
        }

        public async Task deleteCategory(string pCategoryCode)
        {
            var lCategoryFound = await gDbContext.Categories.FirstOrDefaultAsync(x => x.CategoryCode == pCategoryCode);
            if (lCategoryFound != null)
            {
                gDbContext.Categories.Remove(lCategoryFound);
                await gDbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Category not found.");
            }
        }

    }
}
