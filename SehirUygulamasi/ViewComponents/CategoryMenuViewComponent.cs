using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SehirUygulamasi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirUygulamasi.ViewComponents
{
    public class CategoryMenuViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryMenuViewComponent(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(bool ShowEmpty=true)
        {
            var items = await dbContext.Categories.Where(c=> ShowEmpty || c.TodoItems.Count>0).ToListAsync();
            return View(items);
        }
    }
}
