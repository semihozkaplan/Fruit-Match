using System;
using UnityEngine;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _itemSpot;

    [Header("Settings")]
    [SerializeField] private Vector3 _itemLocalPosOnSpot;
    [SerializeField] private Vector3 _itemLocalScaleOnSpot;
    void Awake()
    {
        InputManager.OnItemClicked += HandleItemClicked;
    }

    private void OnDestroy()
    {
       InputManager.OnItemClicked -= HandleItemClicked;
    }

    private void HandleItemClicked(Item item)
    {
        // Turn the item as a child of the item spot
        item.transform.SetParent(_itemSpot);
        // Scale the item down, set its loacl position 0,0,0
        item.transform.localPosition = _itemLocalPosOnSpot;
        item.transform.localScale = _itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // Disable its shadow
        item.DisableShadows();
        // Disable its collider - physics
        item.DisablePhysics();
    }   
}
