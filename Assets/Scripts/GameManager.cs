using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Limits limits;

    void Start()
    {
        var minX = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MinX);
        var maxX = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MaxX);
        var minZ = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MinZ);
        var maxZ = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MaxZ);

        var playerInstance = PhotonNetwork.Instantiate(player.gameObject.name, new Vector3(Random.Range(minX, maxX), player.transform.position.y, Random.Range(minZ, maxZ)), Quaternion.identity);
        playerInstance.transform.SetParent(target);
    }
}
