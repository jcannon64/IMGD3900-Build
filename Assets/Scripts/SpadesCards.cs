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
        if(Manager.spadesUpgrade) {
            collider.isTrigger = true;
        }
        else collider.isTrigger = false;
    }
    
    void FixedUpdate() {
        transform.Rotate(Vector3.back);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-10f * playerDirection, 0, 0));
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            if(other.gameObject.CompareTag("Boss")) {
                other.GetComponent<Boss>().TakeDamage(25);
            }
            else other.GetComponent<Enemy>().TakeDamage(25);
        }
    }
}
