using System.Collections.Generic;

public struct ItemMergeData
{
    public EItemType itemType;
    public List<Item> items;

    public ItemMergeData(Item initalItem)
    {
        itemType = initalItem.ItemType;

        items = new List<Item>();
        items.Add(initalItem);
    }

    public void AddItemToList(Item item)
    {
        items.Add(item);
    }

    public bool CanMerge()
    {
        return items.Count >= 3;
    }
}
