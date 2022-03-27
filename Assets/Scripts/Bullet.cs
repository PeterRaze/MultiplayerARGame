using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRig;
    private PhotonView photonView;

    void Awake()
    {
        bulletRig = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    public void Shoot(Vector3 direction)
    {
        photonView.RPC(nameof(ShootRPC), RpcTarget.AllBuffered, direction);
    }

    [PunRPC]
    private void ShootRPC(Vector3 direction)
    {
        bulletRig.AddForce(direction * 0.1f, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
            if (other.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.SetDamage(10f);
            }
        }
    }

    [PunRPC]
    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

}
