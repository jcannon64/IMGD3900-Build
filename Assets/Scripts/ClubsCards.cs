using UnityEngine;

public class ClubsCards : MonoBehaviour
{
    private Rigidbody2D playerBody;

    void Start() {
        playerBody = FindAnyObjectByType<Player>().GetComponent<Rigidbody2D>();
        Invoke(nameof(playSound), 1f);
        Invoke(nameof(Teleport), 1.25f);
    }

    void Update() {
        transform.Rotate(Vector3.back);
    }

    private void playSound() {
        AudioSource teleport = gameObject.GetComponent<AudioSource>();
        teleport.Play();
    }

    public void Teleport() {
        playerBody.position = gameObject.GetComponent<Rigidbody2D>().position;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat")) {
            if(Manager.clubsUpgrade) {
                
            }
        }
    }
}
