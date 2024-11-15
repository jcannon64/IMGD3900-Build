using UnityEngine;

public class DiamondsCards : MonoBehaviour
{
    public GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat")) {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
