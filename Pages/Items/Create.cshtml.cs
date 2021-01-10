using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect.Data;
using Proiect.Models;


namespace Proiect.Pages.Items
{
    public class CreateModel : ItemCategoriesPageModel
    {
        private readonly Proiect.Data.ProiectContext _context;

        public CreateModel(Proiect.Data.ProiectContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["DesignerID"] = new SelectList(_context.Set<Designer>(), "ID", "DesignerName");
            var item = new Item();
            item.ItemCategories = new List<ItemCategory>();
            PopulateAssignedCategoryData(_context, item);


            return Page();
        }

        [BindProperty]
        public Item Item { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newItem= new Item();
            if (selectedCategories != null)
            {
                newItem.ItemCategories = new List<ItemCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new ItemCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newItem.ItemCategories.Add(catToAdd);
                }
            }
            if (await TryUpdateModelAsync<Item>(
            newItem,
            "Book",
            i => i.Article, i => i.Color,
            i => i.Price, //i => i.PublishingDate, 
            i => i.DesignerID))
            {
                _context.Item.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateAssignedCategoryData(_context, newItem);
            return Page();
        }

    }
}
