using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<Item> OnItemClicked;

    [Header("Settings")]
    [SerializeField] private Material _outlineMat;
    private Item _currentItem;

    void Update()
    {
        if (Input.GetMouseButton(0))
            ApplyDragGesture();
        
        else if (Input.GetMouseButtonUp(0))
            ApplyMouseUp();
    }

    private void ApplyDragGesture()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50);

        if (hit.collider == null)
        {
            DeselectCurrentItem();
            return;
        }

        if (!hit.collider.TryGetComponent(out Item item))
        {
            DeselectCurrentItem();
            return;
        }

        DeselectCurrentItem();
        _currentItem = item;
        _currentItem.SelectItem(_outlineMat);
    }

    private void ApplyMouseUp()
    {
        if (_currentItem == null)
            return;
        
        OnItemClicked?.Invoke(_currentItem);
        DeselectCurrentItem();
    }

    private void DeselectCurrentItem()
    {
        if (_currentItem != null)
            _currentItem.DeselectItem();

        _currentItem = null;
    }
}
