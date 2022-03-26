using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.2f;
    
    private Rigidbody playerRig;

    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontal, 0, vertical);

        playerRig.velocity = direction * speed;

        if (playerRig.velocity.magnitude > speed)
        {
            playerRig.velocity = direction.normalized * speed;
        }
    }
}
