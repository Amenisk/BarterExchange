using BarterExchange.Data.Classes;

namespace BarterExchange.Data.Services
{
    public class ItemService
    {
        public int GetLastCategoryId()
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

        public void CreateItemCategory(ItemCategory category)
        {
            category.ItemCategoryId = GetLastCategoryId() + 1;

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
            var categoriesList = Database.GetAllItemCategories();

            foreach (var c in categoriesList)
            {
                list.Add(c.Title);
            }

            return list;
        }

        public void DeleteItemCategory(string title)
        {
            Database.DeleteItemCategoryByTitle(title);
        }

        public int GetLastTypeId()
        {
            var type = Database.GetLastItemType();

            if (type == null)
            {
                return 0;
            }
            else
            {
                return type.ItemTypeId;
            }
        }

        public void CreateItemType(ItemType type)
        {
            type.ItemTypeId = GetLastTypeId() + 1;

            Database.SaveItemType(type);    
        }

        public ItemCategory GetItemCategoryByTitle(string title)
        {
            return Database.GetItemCategoryByTitle(title);
        }
        public bool CheckItemType(string title)
        {
            var type = Database.GetItemTypeByTitle(title);

            return type != null;
        }

        public List<string> GetTitlesItemTypesByCategory(int itemCategoryId)
        {
            var list = new List<string>();
            var typesList = Database.GetItemTypesByCategory(itemCategoryId);

            foreach (var c in typesList)
            {
                list.Add(c.Title);
            }

            return list;
        }

        public void EditItemType(string title, int value)
        { 
            Database.EditValueItemType(title, value);
        }

        public void DeleteItemType(string title) 
        {
            Database.DeleteItemTypeByTitle(title);
        }

        public ItemType GetItemTypeById(int id)
        {
            return Database.GetItemTypeById(id);
        }

        public ItemCategory GetItemCategoryById(int id)
        {
            return Database.GetItemCategoryById(id);
        }

        public ItemCategory GetItemCategoryByItemTypeId(int itemTypeId)
        {
            var itemType = Database.GetItemTypeById(itemTypeId);

            return GetItemCategoryById(itemType.ItemCategoryId);
        }
    }
}
