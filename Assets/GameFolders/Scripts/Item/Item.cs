using System.Buffers.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody _rigidbody;
    private Collider _collider;
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
        if (TryGetComponent(out _rigidbody))
            _rigidbody.isKinematic = true;
            
        if (TryGetComponent(out _collider))
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
