using UnityEngine;

public class SpadesCards : MonoBehaviour
{
    private float playerDirection;
    private new Collider2D collider;
    
    void Start() {
        playerDirection = FindAnyObjectByType<Player>().GetComponent<Rigidbody2D>().transform.right.x;
        collider = GetComponent<Collider2D>();
    }

    void Update() {
        transform.Rotate(Vector3.back);
        if(Manager.spadesUpgrade) {
            collider.isTrigger = true;
        }
        else collider.isTrigger = false;
    }
    
    void FixedUpdate() {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-10f * playerDirection, 0, 0));
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(25);
        }
    }
}
