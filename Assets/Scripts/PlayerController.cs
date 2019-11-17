using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public PlayerController paddle;
    public float speed;
    public float paddleForce;
    private Rigidbody playerRb;
    private float horizontalInput;
    private const float Xbounds = 2.6f;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        paddleForce = GameController.instance.paddleForce;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(horizontalInput * speed * Time.fixedDeltaTime * -transform.right);
        }

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
            // var speed = Vector3.Magnitude(playerRb.velocity);
            // var normalized = playerRb.position.normalized;
            // playerRb.AddForce(speed * -normalized, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // send a moving ball back onto its incoming trajectory
        if (other.gameObject.CompareTag("Ball") && !GameController.instance.ballCaught)
        {
            var gameController = GameController.instance;
            Debug.Log("Rebound");
            var ballRb = other.gameObject.GetComponent<Rigidbody>();
            var direction = gameController.GetDirection(transform.position, other.transform.position);
            ballRb.AddForce(paddleForce * Time.deltaTime * -direction);
        }
    }
}