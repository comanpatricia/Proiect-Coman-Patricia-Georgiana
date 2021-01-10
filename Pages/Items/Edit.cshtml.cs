using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using Proiect.Models;

namespace Proiect.Pages.Items
{
    public class EditModel : ItemCategoriesPageModel
    {
        private readonly Proiect.Data.ProiectContext _context;

        public EditModel(Proiect.Data.ProiectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Item Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Item = await _context.Item
            .Include(b => b.Designer)
            .Include(b => b.ItemCategories).ThenInclude(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (Item == null)
            {
                return NotFound();
            }
            PopulateAssignedCategoryData(_context, Item);

            ViewData["DesignerID"] = new SelectList(_context.Set<Designer>(), "ID", "DesignerName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemToUpdate = await _context.Item
                 .Include(i => i.Designer)
                 .Include(i => i.ItemCategories)
                 .ThenInclude(i => i.Category)
                 .FirstOrDefaultAsync(s => s.ID == id);

            if (itemToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Item>(itemToUpdate, "Item",
                 i => i.Article, i => i.Color,
                 i => i.Price, i => i.Designer))
            {
                UpdateItemCategories(_context, selectedCategories, itemToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            UpdateItemCategories(_context, selectedCategories, itemToUpdate);
            PopulateAssignedCategoryData(_context, itemToUpdate);
            return Page();
        }

    private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ID == id);
        }
    }
}
