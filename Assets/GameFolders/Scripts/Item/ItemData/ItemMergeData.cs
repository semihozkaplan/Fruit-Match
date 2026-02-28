using System.Collections.Generic;

public struct ItemMergeData
{
    //public string itemName;
    public List<Item> items;

    public ItemMergeData(Item initalItem)
    {
        // İstersek item tipini enum yerine string olarak kullanabiliriz.
        //itemName = initalItem.name;

        items = new List<Item>();
        items.Add(initalItem);
    }

    public void AddItemToList(Item item)
    {
        items.Add(item);
    }
}
