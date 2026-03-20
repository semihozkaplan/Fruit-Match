using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _itemParent;
    [SerializeField] private Animator _animator;

    [Header("Settings")]
    private Item _item;
    public Item Item => _item;

    public void SetItem(Item item)
    {
        _item = item;
        item.transform.SetParent(_itemParent);
        item.SetItemSpot(this);
    }

    public bool IsEmpty()
    {
        if (_item == null)
            return true;
        else
            return false;
    }

    public void ClearItem()
    {
        _item = null;
    }

    public void PlacedAnim()
    {
        _animator.Play("PlacedAnim", 0, 0);
    }
}
