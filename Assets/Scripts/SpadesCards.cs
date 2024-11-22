using UnityEngine;

public class SpadesCards : MonoBehaviour
{
    private float playerDirection;
    
    void Start() {
        playerDirection = FindAnyObjectByType<Player>().GetComponent<Rigidbody2D>().transform.right.x;
    }

    void Update() {
        transform.Rotate(Vector3.back);
    }
    
    void FixedUpdate() {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-10f * playerDirection, 0, 0));
    }
}
