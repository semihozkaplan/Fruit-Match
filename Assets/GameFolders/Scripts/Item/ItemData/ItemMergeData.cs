using System.Collections.Generic;

public struct ItemMergeData
{
    public string itemName;
    public List<Item> items;

    public ItemMergeData(Item initalItem)
    {
        itemName = initalItem.name;

        items = new List<Item>();
        items.Add(initalItem);
    }
}
