using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Collider _collider;
    // Outline Shader - Selection
    [SerializeField] private Renderer _renderer;
    private Material _baseMat;

    private ItemSpot _itemSpot;
    public ItemSpot ItemSpot => _itemSpot;

    [Header("Data")]
    [SerializeField] private EItemType _itemType;
    public EItemType ItemType => _itemType;
    [SerializeField] private Sprite _itemIcon;
    public Sprite ItemIcon => _itemIcon;

    void Awake()
    {
        _baseMat = _renderer.material;
    }

    public void DisableShadows()
    {
        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true; 
        if (_collider != null)
            _collider.enabled = false;
    }

    public void SetItemSpot(ItemSpot itemSpot)
    {
        _itemSpot = itemSpot;
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
