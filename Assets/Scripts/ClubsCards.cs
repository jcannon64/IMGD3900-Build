using UnityEngine;

public class ClubsCards : MonoBehaviour
{
    private Rigidbody2D playerBody;

    void Start() {
        playerBody = FindAnyObjectByType<Player>().GetComponent<Rigidbody2D>();
        Invoke(nameof(playSound), 1f);
        Invoke(nameof(Teleport), 1.25f);
    }

    void FixedUpdate() {
        transform.Rotate(Vector3.back);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Default")) {
            Teleport();
        }
        else if(Manager.clubsUpgrade && other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    private void playSound() {
        AudioSource teleport = gameObject.GetComponent<AudioSource>();
        teleport.Play();
    }

    public void Teleport() {
        playerBody.position = gameObject.GetComponent<Rigidbody2D>().position;
        Destroy(gameObject);
    }
}
