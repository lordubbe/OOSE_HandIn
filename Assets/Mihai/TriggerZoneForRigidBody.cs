using UnityEngine;
using System.Collections;



public class TriggerZoneForRigidBody : MonoBehaviour {

    public GameObject targetObject;

    public float mass = 1;
    public float angularDrag = .5f;
    public float drag = 0;

    public bool useGravity = true;
    public bool isKinematic = false;



    public RigidbodyConstraints constraints;
    void Awake()
    {
        if (targetObject == null) targetObject = transform.parent.gameObject;
        SphereCollider sp = GetComponent<SphereCollider>();
        if (sp != null)
        {
            if (sp.isTrigger)
            {
                sp.radius = 5;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetObject.AddComponent<Rigidbody>();
            targetObject.rigidbody.mass = mass;
            targetObject.rigidbody.angularDrag = angularDrag;
            targetObject.rigidbody.drag = drag;
            targetObject.rigidbody.useGravity = useGravity;
            targetObject.rigidbody.isKinematic = isKinematic;
            targetObject.rigidbody.constraints = constraints;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (targetObject.rigidbody != null) Destroy(targetObject.rigidbody,5.0f);
    }
}
