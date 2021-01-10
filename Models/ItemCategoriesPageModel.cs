using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect.Data;


namespace Proiect.Models
{
    public class ItemCategoriesPageModel:PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList;
        public void PopulateAssignedCategoryData(ProiectContext context,
        Item item)
        {
            var allCategories = context.Category;
            var itemCategories = new HashSet<int>(
            item.ItemCategories.Select(c => c.ItemID));
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = itemCategories.Contains(cat.ID)
                });
            }
        }
        public void UpdateItemCategories(ProiectContext context,
        string[] selectedCategories, Item itemToUpdate)
        {
            if (selectedCategories == null)
            {
                itemToUpdate.ItemCategories = new List<ItemCategory>();
                return;
            }
            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var itemCategories = new HashSet<int>
            (itemToUpdate.ItemCategories.Select(c => c.Category.ID));
            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.ID.ToString()))
                {
                    if (!itemCategories.Contains(cat.ID))
                    {
                        itemToUpdate.ItemCategories.Add(
                        new ItemCategory
                        {
                            ItemID = itemToUpdate.ID,
                            CategoryID = cat.ID
                        });
                    }
                }
                else
                {
                    if (itemCategories.Contains(cat.ID))
                    {
                        ItemCategory courseToRemove
                        = itemToUpdate
                        .ItemCategories
                        .SingleOrDefault(i => i.CategoryID == cat.ID);
                        context.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}
