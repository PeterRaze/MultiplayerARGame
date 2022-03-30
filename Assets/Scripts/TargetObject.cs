using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public delegate void OnTargetDestruction();
    public event OnTargetDestruction onTargetDestruction;
    
    private void Update()
    {
        var eulerAngles = transform.localEulerAngles;
        eulerAngles += Vector3.up * Time.deltaTime * 45f;

        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet>(out Bullet bullet))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        onTargetDestruction?.Invoke();
    }

}
