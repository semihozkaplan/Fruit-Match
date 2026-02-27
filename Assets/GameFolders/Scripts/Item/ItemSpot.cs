using UnityEngine;

public class ItemSpot : MonoBehaviour
{   
    [Header("Settings")]
    private Item _item;

    public void SetItem(Item item)
    {
        _item = item;
        item.transform.SetParent(this.transform);
    }

    public bool IsEmpty()
    {
        if (_item == null)
            return true;
        else
            return false;
    }
}
