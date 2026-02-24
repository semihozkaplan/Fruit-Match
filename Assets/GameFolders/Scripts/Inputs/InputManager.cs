using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<Item> OnItemClicked;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ApplyClickDown();
    }

    private void ApplyClickDown()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50);

        if (hit.collider == null)
            return;

        if (!hit.collider.TryGetComponent(out Item item))
            return;

        Debug.Log("We hit to " + hit.collider);
        OnItemClicked?.Invoke(item);
    }
}
