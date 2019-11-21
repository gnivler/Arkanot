using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BrickController : MonoBehaviour
{
    private float brickForce = 3f;
    private void OnCollisionExit(Collision other)
    {
        var reflect = Vector3.Reflect(transform.position, other.transform.position.normalized);
        Debug.Log($"reflect {reflect}");
        GameController.ballRb.AddRelativeForce(reflect * brickForce);
        GameController.bricks--;
        Destroy(gameObject);
    }
}