using UnityEngine;

public class TargetController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // destroy the target and send the ball flying
        Debug.Log($"Collided with {gameObject.name}");
        var instance = GameController.instance;
        var inverseVector = instance.ball.transform.position.normalized * -1;
        var paddleForce = instance.paddleForce;
        instance.ball.GetComponent<Rigidbody>().AddForce(inverseVector * paddleForce, ForceMode.Impulse);
        
        Destroy(gameObject);
    }
}
