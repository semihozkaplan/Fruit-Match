using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Collider _collider;
    // Outline Shader - Selection
    [SerializeField] private Renderer _renderer;
    private Material _baseMat;

    void Awake()
    {
        _baseMat = _renderer.material;    
    }

    public void DisableShadows()
    { 
    }

    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true; 
        if (_collider != null)
            _collider.enabled = false;
    }

    public void SelectItem(Material outlineMat)
    {
        _renderer.materials = new Material[] {_baseMat, outlineMat};
    }
    
    public void DeselectItem()
    {
        _renderer.materials = new Material[] {_baseMat};
    }
}
