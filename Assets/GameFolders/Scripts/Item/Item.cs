using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    public void DisableShadows()
    {
        
    }

    public void DisablePhysics()
    {
        _rigidbody.GetComponent<Rigidbody>().isKinematic = true;
        _collider.GetComponent<Collider>().enabled = false;
    }
}
