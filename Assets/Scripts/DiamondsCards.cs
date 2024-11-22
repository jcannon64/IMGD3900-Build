using UnityEngine;

public class DiamondsCards : MonoBehaviour
{
    public GameObject explosion;

    void Update() {
        transform.Rotate(Vector3.back);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat")) {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
