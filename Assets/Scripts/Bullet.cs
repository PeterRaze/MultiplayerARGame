using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRig;

    void Awake()
    {
        bulletRig = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 direction)
    {
        bulletRig.AddForce(direction * 0.1f, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Player>())
        {
            Destroy(this.gameObject);
        }
    }


}
