using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.2f;

    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private Transform turret;

    private Rigidbody playerRig;
    private PhotonView photonView;

    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
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
    }

    void Update()
    {
        if (photonView.IsMine)
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
    }

    private void Fire()
    {
        var bulletInstance = PhotonNetwork.Instantiate(bullet.gameObject.name, this.transform.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.transform.SetParent(transform.root, true);

        bulletInstance.Shoot(new Vector3(Mathf.Sin(turret.localEulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(turret.localEulerAngles.y * Mathf.Deg2Rad)));
    }

}
