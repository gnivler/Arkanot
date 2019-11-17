using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [NonSerialized] public static GameController instance;
    public GameObject prefab;
    public GameObject ball;
    public GameObject player;
    public float paddleForce = 2f;
    private Rigidbody playerRb;
    private Rigidbody ballRb;
    private const float upperBound = 4.4f;
    private const float lowerBound = -7f;
    private const float Xbounds = 3.1f;
    internal bool ballCaught;

    public void Start()
    {
        // setup Singleton
        instance = this;

        playerRb = player.GetComponent<Rigidbody>();
        ball = Instantiate(prefab);
        ballRb = ball.GetComponent<Rigidbody>();
        ballCaught = true;
    }

    private void Update()
    {
        var hotkey = Input.GetKeyDown(KeyCode.Space);
        if (ballCaught && hotkey)
        {
            Debug.Log("Push!");
            ballRb.AddForce(new Vector3(Random.Range(-1f, 1f), 1, 0) * 10, ForceMode.Impulse);
            ball.transform.SetParent(transform.root);
            ballCaught = false;
        }
    }

    private void FixedUpdate()
    {
        // physical bounces
        var position = ballRb.transform.position;
        if (position.y > upperBound)
            ballRb.AddForce(Vector3.down, ForceMode.Impulse);

        if (position.x < -Xbounds)
            ballRb.AddForce(Vector3.right, ForceMode.Impulse);

        if (position.x > Xbounds)
            ballRb.AddForce(Vector3.left, ForceMode.Impulse);

        // lost the ball
        if (position.y < lowerBound)
        {
            StartCoroutine(Respawn());
            Destroy(ball);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.5f);
        ball = Instantiate(prefab);
        ballRb = ball.GetComponent<Rigidbody>();
        // position it above the paddle (diameter of ball is 0.15f)
        ball.transform.position = playerRb.transform.position + new Vector3(0, 0.15f, 0);
        // flag prevents it from shooting without pressing space
        ballCaught = true;
    }
}