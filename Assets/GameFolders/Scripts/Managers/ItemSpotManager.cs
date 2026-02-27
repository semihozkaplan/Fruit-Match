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

    [Header("Data")]
    private Dictionary<EItemType, ItemMergeData> _itemMergeData = new Dictionary<EItemType, ItemMergeData>();

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
        if (_itemMergeData.ContainsKey(item.ItemType))
            HandleItemMergeDataFound(item);
        else
            MoveToFirstEmptySpot(item);
    }

    private void HandleItemMergeDataFound(Item item)
    {
        throw new NotImplementedException();
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

        targetSpot.SetItem(item);

        // Scale the item down, set its loacl position 0,0,0
        item.transform.localPosition = _itemLocalPosOnSpot;
        item.transform.localScale = _itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // Disable its shadow
        item.DisableShadows();
        // Disable its collider - physics
        item.DisablePhysics();

        HandleFirstItemReachedSpot(item);
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
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
        _itemMergeData.Add(item.ItemType, new ItemMergeData(item));
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
