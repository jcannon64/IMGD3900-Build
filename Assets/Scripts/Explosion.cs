using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start() {
        AudioSource boom = gameObject.GetComponent<AudioSource>();
        boom.Play();
        Invoke(nameof(removeExplosion), 0.5f);
    }

    private void removeExplosion() {
        Destroy(gameObject);
    }
}
