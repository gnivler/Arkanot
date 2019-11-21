using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming

[RequireComponent(typeof(PlayerController), typeof(BallController), typeof(BrickController))]
public class GameController : MonoBehaviour
{
    [NonSerialized] public static GameController instance;
    public GameObject prefab;
    public GameObject player;
    public float paddleForce;
    public float paddleSpeed;
    public static float maxBallSpeed = 30f;
    public GameObject[] brickPrefabs = new GameObject[3];
    internal static Rigidbody ballRb;
    internal static bool ballCaught;
    internal static int bricks;
    private static bool firedOnce;
    private static GameObject ball;
    private static Rigidbody playerRb;
    private const float upperBound = 3.3f;
    private const float lowerBound = -7.3f;
    private const float Xbounds = 6.87f;
    

    public void Start()
    {
        // setup Singleton
        instance = this;

        // spawn the ball, which sticks to the paddle via BallController.Start()
        playerRb = player.GetComponent<Rigidbody>();
        ball = Instantiate(prefab);
        ballRb = ball.GetComponent<Rigidbody>();
        ballCaught = true;
        BuildBoard();
    }

    private void Update()
    {
        var hotkey = Input.GetKeyDown(KeyCode.Mouse0);
        if (ballCaught && hotkey)
        {
            // push slightly left or right randomly
            Debug.Log("Push!");
            var random = Random.Range(-0.5f, 0.5f);
            ballRb.AddForce(paddleForce * Time.fixedDeltaTime * new Vector3(random, 10, 0), ForceMode.VelocityChange);
            // release the ball
            ball.transform.SetParent(transform.root);
            ballCaught = false;
        }
    }

    private void FixedUpdate()
    {
        // physical bounces.  maintain velocity..
        var position = ballRb.transform.position;
        var velocity = ballRb.velocity.magnitude;

        if (position.y >= upperBound && !firedOnce)
        {
            var reflect = Vector3.Reflect(position, new Vector3(0, position.x, 0));
            ballRb.AddForce(reflect, ForceMode.VelocityChange);
            firedOnce = true;
        }

        if (position.x <= -Xbounds && !firedOnce)
        {
            var reflect = Vector3.Reflect(position, new Vector3(position.y, 0, 0));
            ballRb.AddForce(reflect, ForceMode.VelocityChange);
        }

        if (position.x >= Xbounds)
        {
            var reflect = Vector3.Reflect(position, new Vector3(position.y, 0, 0));
            ballRb.AddForce(reflect, ForceMode.VelocityChange);
        }

        
        // lost the ball
        if (position.y <= lowerBound)
        {
            StartCoroutine(Respawn());
        }
        
        firedOnce = false;
        
        if (bricks == 0)
        {
            BuildBoard();
        }
    }

    private void BuildBoard()
    {
        // board dimensions
        var columns = 11;
        var rows = 5;
        var spacing = 0.1825f;
        bricks = columns * rows;
        var startingPosition = new Vector2(-5.9f, 3.3f);
        var brickWidth = brickPrefabs[0].GetComponent<Transform>().localScale.x;
        var brickHeight = brickPrefabs[0].GetComponent<Transform>().localScale.y;
        var position = startingPosition;
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                var randomBlock = brickPrefabs[Random.Range(0, brickPrefabs.Length)];
                Instantiate(randomBlock, position, Quaternion.identity);
                // move it over 
                position += new Vector2(brickWidth + spacing, 0);
            }

            // move it down
            position = startingPosition += new Vector2(0, -brickHeight - spacing);
        }
    }

    private IEnumerator Respawn()
    {
        Destroy(ball);
        yield return new WaitForSeconds(0.5f);
        ball = Instantiate(prefab);
        ballRb = ball.GetComponent<Rigidbody>();
        // position it above the paddle (diameter of ball is 0.15f)
        ball.transform.position = playerRb.transform.position + new Vector3(0, 0.15f, 0);
        // flag prevents it from shooting without pressing space
        ballCaught = true;
    }

    // https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
    internal static Vector3 GetTargetDirection(Vector3 target, Vector3 source)
    {
        var heading = target - source;
        var distance = heading.magnitude;
        return heading / distance;
    }
}