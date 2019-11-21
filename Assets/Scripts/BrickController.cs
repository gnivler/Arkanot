using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BrickController : MonoBehaviour
{
    private float brickForce = 5f;
    private void OnCollisionExit(Collision other)
    {
        // send the ball flying back the way it came and destroy the brick (not working)
        // attempt at computing the "incidental angle?" to return ball on its incoming path
        //var direction = (Vector2) GameController.GetTargetDirection(other.transform.position, transform.position);
        //Debug.Log($"direction {direction}");
        //var velocity = (Vector2) other.gameObject.GetComponent<Rigidbody>().velocity;
        //Debug.Log($"velocity {velocity}");
        //var angle = Vector3.Angle(other.transform.position, transform.position);
        //Debug.Log($"angle {angle}");
        
        //Debug.Log($"forceVector {forceVector}");
        var reflect = Vector3.Reflect(transform.position, other.transform.position.normalized);
        Debug.Log($"reflect {reflect}");
        GameController.ballRb.AddRelativeForce(reflect * 3);
        GameController.bricks--;
        Destroy(gameObject);
    }
}