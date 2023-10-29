using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;
using System.Collections;
using System;

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

    [SerializeField]
    private Transform bulletStartPoint;

    [SerializeField]
    private PlayerUI playerUI;

    [SerializeField]
    private MeshRenderer bodyMeshRenderer;

    private Rigidbody playerRig;
    private PhotonView photonView;

    public delegate void OnPlayerDestruction();
    public event OnPlayerDestruction onPlayerDestruction;

    private bool isAllowedToShoot;

    public float hp = 30f;

    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        playerUI.SetTotalHP(hp);
        isAllowedToShoot = true;
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

    public void SetDamage(float damage)
    {
        photonView.RPC(nameof(SetDamageRPC), RpcTarget.AllBuffered, damage);
    }

    [PunRPC]
    private void SetDamageRPC(float damage)
    {
        hp -= damage;
        playerUI.ReduceHP(damage);

        if (hp <= 0 && photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
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

            if (CrossPlatformInputManager.GetButtonDown("Fire1") && isAllowedToShoot)
            {
                isAllowedToShoot = false;
                Fire();
            }
        }
    }

    private void Fire()
    {
        var bulletInstance = PhotonNetwork.Instantiate(bullet.gameObject.name, bulletStartPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.transform.SetParent(null, true);

        bulletInstance.Shoot(new Vector3(Mathf.Sin(turret.localEulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(turret.localEulerAngles.y * Mathf.Deg2Rad)));
        StartCoroutine(AllowShoot());
    }

    public void SetColor(Color color)
    {
        bodyMeshRenderer.material.color = color;
    }

    private IEnumerator AllowShoot()
    {
        yield return new WaitForSeconds(1f);
        isAllowedToShoot = true;
    }

    private void OnDestroy()
    {
        onPlayerDestruction?.Invoke();
    }

}
