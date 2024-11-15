using UnityEngine;

public class ClubsCards : MonoBehaviour
{
    private Rigidbody2D playerBody;

    void Start() {
        playerBody = FindAnyObjectByType<Player>().GetComponent<Rigidbody2D>();
        Invoke(nameof(playSound), 1f);
        Invoke(nameof(Teleport), 1.25f);
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
