using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject cardDrop;
    
    //private SpriteRenderer spriteRenderer;
    //public Sprite[] runSprites;
    //public Sprite[] attackSprites;
    //private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;
    public float health = 3f;
    public float damage = 1f;
    private bool grounded;

    private void Awake() {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void OnEnable() {
        //InvokeRepeating(nameof(AnimateSprite), 1f/20f, 1f/20f);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void CheckCollision() {
        grounded = false;
        
        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for(int i = 0; i < amount; i++) {
            GameObject hit = results[i].gameObject;
            if(hit.layer == LayerMask.NameToLayer("Ground")) {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
        }
    }

    private void Update() {
        CheckCollision();
        
        direction += Physics2D.gravity * Time.deltaTime;

        float playerLocation = player.transform.position.x - transform.position.x;
        if(playerLocation < 0) {
            direction.x = -moveSpeed;
        }
        else direction.x = moveSpeed;

        if(grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if(direction.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        }
        else if(direction.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void FixedUpdate() {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    /*private void AnimateSprite() {
        if(direction.x != 0f) {
            spriteIndex++;

            if(spriteIndex >= runSprites.Length) {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
        else {
            spriteIndex = 0;
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Hitbox")) {
            TakeDamage(FindAnyObjectByType<Player>().damage);
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource hurt = sounds[0];
            hurt.Play();
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Explosion")) {
            TakeDamage(10);
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource hurt = sounds[0];
            hurt.Play();
        }
        else if(collision.gameObject.CompareTag("Player")) {
            FindAnyObjectByType<Player>().TakeDamage(damage);
        }

        if(health <= 0) {
            if(gameObject.CompareTag("Security")) {
                Instantiate(cardDrop, transform.position, Quaternion.identity);
                FindAnyObjectByType<Player>().GainChips(100);
            }
            else FindAnyObjectByType<Player>().GainChips(50);
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
    }
}
