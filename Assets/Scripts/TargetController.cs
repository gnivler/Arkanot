using System.Net.Mime;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TargetController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        // destroy the target and send the ball flying back the way it came
        Debug.Log($"Collided with {gameObject.name}");
        var instance = GameController.instance;
        var paddleForce = instance.paddleForce;
        var direction = instance.GetDirection(transform.position, other.transform.position);
        instance.ball.GetComponent<Rigidbody>().AddForce(paddleForce * Time.deltaTime * -direction, ForceMode.Impulse);

        Destroy(gameObject);
    }
}