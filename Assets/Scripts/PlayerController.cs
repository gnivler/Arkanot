using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public PlayerController paddle;
    private Rigidbody playerRb;
    public float speed;
    public float horizontalInput;
    public float paddleForce;
    private const float Xbounds = 2.6f;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        paddleForce = GameController.instance.paddleForce;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        var position = transform.position;
        if (position.x < -Xbounds)
        {
            transform.position = new Vector3(-Xbounds, position.y, position.z);
        }

        if (position.x > Xbounds)
        {
            transform.position = new Vector3(Xbounds, position.y, position.z);

            // TODO make physics counter the movement?
            // not working:
            var speed = Vector3.Magnitude(playerRb.velocity);
            var normalized = playerRb.position.normalized;
            playerRb.AddForce(speed * -normalized, ForceMode.VelocityChange);
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(horizontalInput * speed * Time.deltaTime * -transform.right);
        }
    }

    
    private void OnCollisionEnter(Collision other)
    {
        // send a moving ball back onto its incoming trajectory
        if (other.gameObject.CompareTag("Ball") && !GameController.instance.ballCaught)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.position.normalized * paddleForce);
        }
    }
}