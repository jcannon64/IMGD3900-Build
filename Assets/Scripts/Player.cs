using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] attackSprites;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    private bool grounded;
    private bool attacking = false;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void OnEnable() {
        InvokeRepeating(nameof(AnimateSprite), 1f/20f, 1f/20f);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void CheckCollision() {
        grounded = false;
        //attacking = false;
        
        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        //size.x /= 2f;
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
        
        if(grounded && Input.GetButtonDown("Jump")) {
            direction = Vector2.up * jumpStrength;
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource jump = sounds[0];
            jump.Play();
        }
        else direction += Physics2D.gravity * Time.deltaTime;

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        if(grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if(direction.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        }
        else if(direction.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if(!attacking && Input.GetButtonDown("Fire1")) {
            spriteIndex = 0;
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource attack = sounds[1];
            attack.Play();
            attacking = true;
        }

        if(Input.GetButtonDown("Cancel")) {
            FindAnyObjectByType<GameManager>().QuitGame();
        }
    }

    private void FixedUpdate() {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite() {
        if(attacking) {
            spriteIndex++;
            
            if(spriteIndex >= attackSprites.Length) {
                spriteIndex = 0;
                attacking = false;
            }

            /*if(spriteIndex >= 6) {
                collider.bounds.Expand(1f);
            }*/

            spriteRenderer.sprite = attackSprites[spriteIndex];
        }
        else if(direction.x != 0f) {
            spriteIndex++;

            if(spriteIndex >= runSprites.Length) {
                spriteIndex = 0;
            }
            else if(spriteIndex == runSprites.Length - 1) {
                
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
        else {
            spriteIndex = 0;
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy")) {
            if(attacking) {
                Destroy(collision.gameObject);
            }
            else {
                enabled = false;
                FindAnyObjectByType<GameManager>().LevelFailed();
            }
        }
    }
}
