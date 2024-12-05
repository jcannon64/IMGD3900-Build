using UnityEngine;

public class Explosion : MonoBehaviour
{
    /*private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;*/

    void Start() {
        AudioSource boom = gameObject.GetComponent<AudioSource>();
        boom.Play();
        Invoke(nameof(removeExplosion), 0.5f);
    }

    private void removeExplosion() {
        Destroy(gameObject);
    }

    /*private void OnEnable() {
        InvokeRepeating(nameof(AnimateSprite), 1f/20f, 1f/20f);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void AnimateSprite() {
        spriteIndex++;
        spriteRenderer.sprite = sprites[spriteIndex];
    }*/
}
