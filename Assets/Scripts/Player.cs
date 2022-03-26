using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.2f;

    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private Transform turret;

    private Rigidbody playerRig;

    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        var vertical = CrossPlatformInputManager.GetAxis("Vertical");


        var direction = new Vector3(horizontal, 0, vertical);

        playerRig.velocity = direction * speed;

        if (playerRig.velocity.magnitude > speed)
        {
            playerRig.velocity = direction.normalized * speed;
        }
    }

    void Update()
    {
        var horizontalRotation = CrossPlatformInputManager.GetAxisRaw("HorizontalRotation");
        var verticalRotation = CrossPlatformInputManager.GetAxisRaw("VerticalRotation");

        if (horizontalRotation != 0 && verticalRotation != 0)
        {
            turret.localRotation = Quaternion.LookRotation(new Vector3(horizontalRotation, 0, verticalRotation), Vector3.up);
        }

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            if (FindObjectOfType<Bullet>()) return; 

            Fire();
        }
    }

    private void Fire()
    {
        var bulletInstance = Instantiate(bullet, this.transform.position, Quaternion.identity);
        bulletInstance.transform.SetParent(transform.root, true);

        bulletInstance.Shoot(new Vector3(Mathf.Sin(turret.localEulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(turret.localEulerAngles.y * Mathf.Deg2Rad)));
    }

}
