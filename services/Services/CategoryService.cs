using Domain.DTOs;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository gCategoryRepo;
        private readonly IGlobalServices gGlobalServices;

        public CategoryService(ICategoryRepository pCategoryRepo, IGlobalServices pGlobalServices)
        {
            gCategoryRepo = pCategoryRepo;
            gGlobalServices = pGlobalServices;
        }

        public async Task<List<CategoryFormat>> GetCategoriesList()
        {
            try
            {
                List<CategoryFormat> lCategoriesList = new List<CategoryFormat>();
                List<Category> lCategoriesFound = await gCategoryRepo.getCategoriesList();
                if(lCategoriesFound != null)
                {
                    foreach (var item in lCategoriesFound)
                    {
                        CategoryFormat lCategory = new CategoryFormat
                        {
                            CategoryCode = item.CategoryCode,
                            CategoryName = item.Category1
                        };
                        lCategoriesList.Add(lCategory);
                    }
                }
                return lCategoriesList;
            }
            catch (Exception lEx) 
            {
                throw lEx;
            }
        }

        public async Task<CategoryFormat> GetCategoryByCode(string pCategoryCode)
        {
            try
            {
                CategoryFormat lCategory = new CategoryFormat();
                Category lCategoryFound = await gCategoryRepo.getCategoryByCode(pCategoryCode);
                if(lCategoryFound != null)
                {
                    lCategory.CategoryCode = lCategoryFound.CategoryCode;
                    lCategory.CategoryName = lCategoryFound.Category1;
                }
                return lCategory;
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> CreateNewCategory(string pCategoryName)
        {
            try
            {
                string lNewCode;
                while (true)
                {
                    lNewCode = gGlobalServices.createControlCode();
                    var lRegFound = await gCategoryRepo.getCategoryByCode(lNewCode);
                    if (lRegFound == null)
                    {
                        break;
                    }
                }

                Category lCategory = new Category
                {
                    CategoryCode = lNewCode,
                    Category1 = pCategoryName
                };
                await gCategoryRepo.createNewCategory(lCategory);
                return true;
            }
            catch(Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> UpdateCategory(CategoryFormat pCategory)
        {
            try
            {
                Category lCategory = await gCategoryRepo.getCategoryByCode(pCategory.CategoryCode);
                if(lCategory != null)
                {
                    lCategory.Category1 = pCategory.CategoryName;
                    await gCategoryRepo.updateCategoryData(lCategory);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> DeleteCategory(string pCategoryCode)
        {
            try
            {
                await gCategoryRepo.deleteCategory(pCategoryCode);
                return true;
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

    }
}
