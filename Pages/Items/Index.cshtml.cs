using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using Proiect.Models;

namespace Proiect.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly Proiect.Data.ProiectContext _context;

        public IndexModel(Proiect.Data.ProiectContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; }
        public ItemData ItemD { get; set; }
        public int ItemID { get; set; }
        public int CategoryID { get; set; }
        public async Task OnGetAsync(int? id, int? categoryID)
        {
            ItemD = new ItemData();

            ItemD.Items = await _context.Item
            .Include(b => b.Designer)
            .Include(b => b.ItemCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Article)
            .ToListAsync();
            if (id != null)
            {
                ItemID = id.Value;
                Item item = ItemD.Items
                .Where(i => i.ID == id.Value).Single();
                ItemD.Categories = item.ItemCategories.Select(s => s.Category);
            }
        }
    }
}
