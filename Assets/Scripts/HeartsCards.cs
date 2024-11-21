using UnityEngine;

public class HeartsCards : MonoBehaviour
{
    void Update() {
        transform.Rotate(Vector3.back);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat")) {
            //GameObject enemy = GameObject.Find(collision.gameObject.name);
            //enemy.TakeDamage(25);
            if(Manager.heartsUpgrade) {
                FindAnyObjectByType<Player>().TakeDamage(-10);
            }
        }
    }
}
