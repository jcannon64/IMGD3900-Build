using UnityEngine;

public class SpadesCards : MonoBehaviour
{
    private float playerDirection;
    
    void Start() {
        playerDirection = FindAnyObjectByType<Player>().GetComponent<Rigidbody2D>().transform.right.x;
    }
    
    void Update() {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-2.5f * playerDirection, 0, 0));
    }
}
