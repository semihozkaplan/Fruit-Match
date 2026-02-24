using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClickDown();
    }

    private void HandleClickDown()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50);
        Debug.Log("We hit to " + hit.collider);
    }
}
