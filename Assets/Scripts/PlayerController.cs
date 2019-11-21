using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public float paddleForce;
    public float horizontalInput;
    private const float paddleBoundary = 6.05f;


    private void Update()
    {
        horizontalInput = Input.GetAxis("Mouse X");
    }

    private void FixedUpdate()
    {
        transform.Translate(horizontalInput * GameController.instance.paddleSpeed * Time.fixedDeltaTime * -transform.right);

        // keep the paddle within bounaries
        var position = transform.position;
        if (position.x < -paddleBoundary)
        {
            transform.position = new Vector3(-paddleBoundary, position.y, position.z);
        }

        if (position.x > paddleBoundary)
        {
            transform.position = new Vector3(paddleBoundary, position.y, position.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // send a moving ball back onto its incoming trajectory
        if (other.gameObject.CompareTag("Ball") && !GameController.ballCaught)
        {
            var direction = (Vector2) GameController.GetTargetDirection(transform.position, other.transform.position);
            var velocity = (Vector2) other.gameObject.GetComponent<Rigidbody>().velocity;
            //ballRb.AddForce(paddleForce * Time.fixedDeltaTime * velocity * -direction, ForceMode.VelocityChange);
            GameController.ballRb.AddRelativeForce(3 * velocity * paddleForce * direction);
        }
    }
}