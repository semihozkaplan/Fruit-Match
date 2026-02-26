using System;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _itemSpotsParent;
    private ItemSpot[] _itemSpots;

    [Header("Settings")]
    [SerializeField] private Vector3 _itemLocalPosOnSpot;
    [SerializeField] private Vector3 _itemLocalScaleOnSpot;
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
        if (!IsFreeSpotExists())
        {
            Debug.Log("There is no free spot available! Game Over!!!!");
            return;
        }

        HandleClickOnItem(item);
    }

    private void HandleClickOnItem(Item item)
    {
        MoveToFirstEmptySpot(item);
    }

    private void MoveToFirstEmptySpot(Item item)
    {
        ItemSpot targetSpot = GetEmptySpot();

        if (targetSpot == null)
        {
            Debug.LogError("Target Spot is null!");
            return;
        }

        targetSpot.SetItem(item);

        // Scale the item down, set its loacl position 0,0,0
        item.transform.localPosition = _itemLocalPosOnSpot;
        item.transform.localScale = _itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // Disable its shadow
        item.DisableShadows();
        // Disable its collider - physics
        item.DisablePhysics();
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
