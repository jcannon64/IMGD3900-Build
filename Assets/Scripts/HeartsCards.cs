using UnityEngine;

public class HeartsCards : MonoBehaviour
{
    void FixedUpdate() {
        transform.Rotate(Vector3.back);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat") || collision.gameObject.CompareTag("Boss")) {
            if(Manager.heartsUpgrade) {
                FindAnyObjectByType<Player>().TakeDamage(-10);
            }
        }
    }
}
