using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    private Rigidbody ballRb;
    public float maxSpeed;

    private void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        transform.SetParent(GameController.instance.player.transform);
    }

    public void Update()
    {
        // mitigate stuff passing through objects by limiting velocity
        //https://answers.unity.com/questions/9985/limiting-rigidbody-velocity.html
        //LimitVelocity();
    }

    private void LimitVelocity()
    {
        var speed = Vector3.Magnitude(ballRb.velocity);
        if (speed > maxSpeed)
        {
            var brakeSpeed = speed - maxSpeed;
            var normalizedVelocity = ballRb.velocity.normalized;
            var brakeVelocity = normalizedVelocity * brakeSpeed;
            ballRb.AddForce(-brakeVelocity);
        }
    }
}