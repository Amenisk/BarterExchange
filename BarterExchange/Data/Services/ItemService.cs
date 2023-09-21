using BarterExchange.Data.Classes;

namespace BarterExchange.Data.Services
{
    public class ItemService
    {
        public int GetLastId()
        {
            var category = Database.GetLastItemCategory();

            if (category == null)
            {
                return 0;
            }
            else
            {
                return category.ItemCategoryId;
            }
        }

        public void SaveItemCategory(ItemCategory category)
        {
            category.ItemCategoryId = GetLastId() + 1;

            Database.SaveItemCategory(category);
        }

        public bool CheckItemCategory(string title)
        {
            var category = Database.GetItemCategoryByTitle(title);

            return category != null;
        }

        public bool CheckItemCategoryBeforeDelete(string title)
        {
            var category = Database.GetItemCategoryByTitle(title);

            return Database.CheckPresenceItemTypeByItemCategoryId(category.ItemCategoryId);
        }

        public List<string> GetTitlesItemCategories() 
        {
            var list = new List<string>();  
            var categoriesList = Database.GetAllItemCategory();

            foreach (var c in categoriesList)
            {
                list.Add(c.Title);
            }


            return list;
        }

        public void DeleteCategory(string title)
        {
            Database.DeleteItemCategoryByTitle(title);
        }
    }
}
