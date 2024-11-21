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

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat")) {
            
        }
    }
}
