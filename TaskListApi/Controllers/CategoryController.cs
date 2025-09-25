using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TaskListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService gCategoryService;

        public CategoryController(ICategoryService pCategoryService)
        {
            gCategoryService = pCategoryService;
        }

        [HttpGet("GetCategoriesList")]
        public async Task<List<CategoryFormat>> GetCategoriesList()
        {
            return await gCategoryService.GetCategoriesList();
        }

        [HttpGet("GetCategoryByCode")]
        public async Task<CategoryFormat> GetCategoryByCode(string pCategoryCode)
        {
            return await gCategoryService.GetCategoryByCode(pCategoryCode);
        }

        [HttpPost("CreateNewCategory")]
        public async Task<bool> CreateNewCategory(string pCategoryName)
        {
            return await gCategoryService.CreateNewCategory(pCategoryName);            
        }

        [HttpPut("UpdateCategoryData")]
        public async Task<bool> UpdateCategoryData(CategoryFormat pCategory)
        {
            return await gCategoryService.UpdateCategory(pCategory);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<bool> DeleteCategory(string pCategoryCode)
        {
            return await gCategoryService.DeleteCategory(pCategoryCode);
        }

    }
}
