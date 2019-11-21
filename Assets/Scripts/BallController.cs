using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    private Rigidbody ballRb;
    private float minSpeed = 3f;
    private float previousSpeed;
    private float currentSpeed;

    private void Start()
    {
        ballRb = GameController.ballRb;
        // place the ball onto the paddle
        transform.SetParent(GameController.instance.player.transform);
    }

    public void Update()
    {
        // if speed is less than minSpeed for 3 seconds, boost it


        // mitigate stuff passing through objects by limiting velocity
        //https://answers.unity.com/questions/9985/limiting-rigidbody-velocity.html
        //Debug.Log((Vector2) ballRb.position.normalized);
        // MaintainVelocity();
    }

    public void FixedUpdate()
    {
        LimitVelocity();
        //throw new NotImplementedException();
    }

    private void LimitVelocity()
    {
        var speed = Vector3.Magnitude(ballRb.velocity);
        if (speed > GameController.maxBallSpeed)
        {
            var brakeSpeed = speed - GameController.maxBallSpeed;
            Debug.Log("braking " + brakeSpeed);
            var normalizedVelocity = ballRb.velocity.normalized;
            var brakeVelocity = normalizedVelocity * brakeSpeed;
            ballRb.AddForce(-brakeVelocity, ForceMode.VelocityChange);
        }

        // has to be under speed for a certain time..

        //if (speed < minSpeed)
        //{
        //    ballRb.AddRelativeForce(new Vector2(ballRb.position.x * 1.1f, ballRb.position.y * 1.1f), ForceMode.Acceleration);
        //    //var underSpeed = GameController.maxBallSpeed - speed;
        //
        //}
    }

    private void OnCollisionEnter(Collision other)
    {
        var speed = Vector3.Magnitude(ballRb.velocity);
        if (speed < minSpeed)
        {
            ballRb.AddForce(RandomVector3() * GameController.instance.paddleForce);
        }

        Vector3 RandomVector3()
        {
            var x = Random.Range(0f, 1f);
            var y = Random.Range(0f, 1f);
            return new Vector3(x, y, 0);
        }
    }
}