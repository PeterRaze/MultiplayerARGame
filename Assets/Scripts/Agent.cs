using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private WayPoint wayPoint;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform turret;

    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private Transform bulletStartPoint;

    [SerializeField]
    private PlayerUI agentUI;

    private Vector3 randomPoint;

    public delegate void OnAgentDestruction();
    public event OnAgentDestruction onAgentDestruction;

    private bool isAllowedToShoot;
    private float hp = 30f;
    private float speed = 0.015f;

    void Start()
    {
        randomPoint = wayPoint.GetRandomWayPoint();
        agentUI.SetTotalHP(hp);

        StartCoroutine(AllowShoot(2f));
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(transform.position, randomPoint, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, randomPoint) < 0.001f)
        {
            randomPoint = wayPoint.GetRandomWayPoint();
        }

        Vector3 relativePos = player.transform.position - transform.position;

        var turretRotation = Quaternion.LookRotation(relativePos);

        turret.localRotation = turretRotation;

        if (isAllowedToShoot)
        {
            ShootAtPlayer();
            isAllowedToShoot = false;
        }
    }

    public void SetDamage(float damage)
    {
        hp -= damage;
        agentUI.ReduceHP(damage);

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void ShootAtPlayer()
    {
        var bulletInstance = PhotonNetwork.Instantiate(bullet.gameObject.name, bulletStartPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.transform.SetParent(transform.root, true);

        bulletInstance.Shoot(new Vector3(Mathf.Sin(turret.localEulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(turret.localEulerAngles.y * Mathf.Deg2Rad)));

        StartCoroutine(AllowShoot(1f));
    }

    private IEnumerator AllowShoot(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isAllowedToShoot = true;
    }

    private void OnDestroy()
    {
        onAgentDestruction?.Invoke();
    }

}
