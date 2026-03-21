using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _itemSpotsParent;
    private ItemSpot[] _itemSpots;

    [Header("Settings")]
    [SerializeField] private Vector3 _itemLocalPosOnSpot;
    [SerializeField] private Vector3 _itemLocalScaleOnSpot;
    private bool _isBusy;

    [Header("Animation Settings")]
    [SerializeField] private float _animDuration = 0.2f;
    [SerializeField] private LeanTweenType _animType = LeanTweenType.easeOutQuart;

    [Header("Data")]
    private Dictionary<EItemType, ItemMergeData> _itemMergeDataDictionary = new Dictionary<EItemType, ItemMergeData>();

    [Header("Actions")]
    public static Action<List<Item>> OnMergeStarted;

    void Awake()
    {
        StoreItemSpots();
    }

    void OnEnable()
    {
        InputManager.OnItemClicked += HandleItemClicked;
    }

    void OnDisable()
    {
        InputManager.OnItemClicked -= HandleItemClicked;
    }

    private void HandleItemClicked(Item item)
    {
        if (_isBusy)
        {
            Debug.LogWarning("Item Spot Manager is busy! Please wait until the current operation is finished.");
            return;
        }

        if (!IsFreeSpotExists())
        {
            Debug.Log("There is no free spot available! Game Over!!!!");
            return;
        }

        _isBusy = true;
        HandleClickOnItem(item);
    }

    private void HandleClickOnItem(Item item)
    {
        if (_itemMergeDataDictionary.ContainsKey(item.ItemType))
            HandleItemMergeDataFound(item);
        else
            MoveToFirstEmptySpot(item);
    }

    private void HandleItemMergeDataFound(Item item)
    {
        ItemSpot correctSpot = GetCorrectSpot(item);
  
        _itemMergeDataDictionary[item.ItemType].AddItemToList(item);

        TryMoveItemToCorrectSpot(item, correctSpot);
    }

    private ItemSpot GetCorrectSpot(Item item)
    {
        List<Item> items = _itemMergeDataDictionary[item.ItemType].items;
        List<ItemSpot> itemSpots = new List<ItemSpot>();

        // In here, we are having all item spots that has been using with same type
        for (int i = 0; i < items.Count; i++)
        {
            itemSpots.Add(items[i].ItemSpot); 
        }

        // If we have only one spot than grab the next spot
        if (itemSpots.Count >= 2)
        {
            // Compare it, if b transform is bigger then sort it, in this way we can access the most right one spot as first index
            itemSpots.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }

        int correctSpotIndex = itemSpots[0].transform.GetSiblingIndex() + 1;

        return _itemSpots[correctSpotIndex];
    }

    private void TryMoveItemToCorrectSpot(Item item, ItemSpot correctSpot)
    {
        if (!correctSpot.IsEmpty())
        {
            HandleCorrectSpotFull(item, correctSpot);
            return;
        }

        MoveItemToTargetSpot(item, correctSpot, ()=>HandleItemPlacedOnCorrectSpot(item));
    }

    private void MoveItemToTargetSpot(Item item, ItemSpot targetSpot, Action completeCallback)
    {
        targetSpot.SetItem(item);

        // Animate and move items
        LeanTween.moveLocal(item.gameObject, _itemLocalPosOnSpot, _animDuration)
            .setEase(_animType);
        LeanTween.scale(item.gameObject, _itemLocalScaleOnSpot, _animDuration)
            .setEase(_animType);
        LeanTween.rotateLocal(item.gameObject, Vector3.zero, _animDuration)
            .setOnComplete(completeCallback);

        // Disable its shadow
        item.DisableShadows();
        // Disable its collider - physics
        item.DisablePhysics();
    }

    private void HandleItemPlacedOnCorrectSpot(Item item, bool checkMerge = true)
    {
        item.ItemSpot.PlacedAnim();

        if (!checkMerge)
            return;

        if (_itemMergeDataDictionary[item.ItemType].CanMerge())
        {
            MergeSameItems(_itemMergeDataDictionary[item.ItemType]);
        }
        else
        {
            CheckGameOver();
        }
    }

    private void MergeSameItems(ItemMergeData itemMergeData)
    {
        List<Item> items = itemMergeData.items;

        _itemMergeDataDictionary.Remove(itemMergeData.itemType);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].ItemSpot.ClearItem();
            //Destroy(items[i].gameObject);
        }

        // If all items that has been selected all merged, and no items moving to left
        if (_itemMergeDataDictionary.Count <= 0)
        {
            _isBusy = false;
        }
        else
        {
            MoveItemsToTheLeftSide(HandleAllItemsMovedLeft);
        }

        OnMergeStarted?.Invoke(items);
    }

    private void MoveItemsToTheLeftSide(Action completeCallback)
    {
        bool callbackInvoked = false;
        for (int i = 3; i < _itemSpots.Length; i++)
        {
            ItemSpot itemSpot = _itemSpots[i];

            if (itemSpot.IsEmpty())
                continue;

            Item item = itemSpot.Item;
            itemSpot.ClearItem();
            ItemSpot targetSpot = _itemSpots[i - 3];

            if (!targetSpot.IsEmpty())
            {
                Debug.LogError("Error: Something went wrong, please check the logic!");
                _isBusy = false;
                return;
            }
            completeCallback += () => HandleItemPlacedOnCorrectSpot(item, false);
            MoveItemToTargetSpot(item, targetSpot, completeCallback);

            callbackInvoked = true;
        }
        if (!callbackInvoked)
            completeCallback?.Invoke();
    }

    private void HandleAllItemsMovedLeft()
    {
        _isBusy = false;
    }

    private void HandleCorrectSpotFull(Item comingItem, ItemSpot correctSpot)
    {
        MoveItemsToTheRightSide(comingItem, correctSpot);
    }

    private void MoveItemsToTheRightSide(Item comingItem, ItemSpot correctSpot)
    {
        // Check right spots first
        int itemSpotIndex = correctSpot.transform.GetSiblingIndex();
        for (int i = _itemSpots.Length - 2; i >= itemSpotIndex; i--)
        {
            ItemSpot itemSpot = _itemSpots[i];

            if (itemSpot.IsEmpty())
                continue;

            // This is the item we want to move to the right spot
            Item item = itemSpot.Item;
            // Clear the next right spot
            itemSpot.ClearItem();
            // Store the next right spot
            ItemSpot targetSpot = _itemSpots[i + 1];

            if (!targetSpot.IsEmpty())
            {
                Debug.LogError("Error: Something went wrong, please check the logic!");
                _isBusy = false;
                return;
            }

            MoveItemToTargetSpot(item, targetSpot, () => HandleItemPlacedOnCorrectSpot(item, false));
        }

        MoveItemToTargetSpot(comingItem, correctSpot, () => HandleItemPlacedOnCorrectSpot(comingItem));
    }

    private void MoveToFirstEmptySpot(Item item)
    {
        ItemSpot targetSpot = GetEmptySpot();

        if (targetSpot == null)
        {
            Debug.LogError("Target Spot is null!");
            return;
        }

        CreateItemMergeData(item);

        MoveItemToTargetSpot(item, targetSpot, () => HandleFirstItemPlacedOnSpot(item));
    }

    private void HandleFirstItemPlacedOnSpot(Item item)
    {
        item.ItemSpot.PlacedAnim();
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (GetEmptySpot() == null)
            Debug.Log("Game Over !!!");
        else
            _isBusy = false;
    }

    private void CreateItemMergeData(Item item)
    {
        _itemMergeDataDictionary.Add(item.ItemType, new ItemMergeData(item));
    }

    private ItemSpot GetEmptySpot()
    {
        for (int i = 0; i < _itemSpots.Length; i++)
        {
            if (_itemSpots[i].IsEmpty())
                return _itemSpots[i];
        }
        return null;
    }

    private void StoreItemSpots()
    {
        _itemSpots = new ItemSpot[_itemSpotsParent.childCount];

        for (int i = 0; i < _itemSpotsParent.childCount; i++)
        {
            ItemSpot itemSpot = _itemSpotsParent.GetChild(i).GetComponent<ItemSpot>();
            if (itemSpot != null)
            {
                _itemSpots[i] = itemSpot;
            }
        }
    }

    private bool IsFreeSpotExists()
    {
        for (int i = 0; i < _itemSpots.Length; i++)
        {
            if (_itemSpots[i].IsEmpty())
                return true;
        }
        return false;
    }   
}
