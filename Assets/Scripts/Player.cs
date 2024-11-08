using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] attackSprites;
    public Sprite[] punchSprites;
    public Sprite[] daggerSprites;
    public Sprite[] batSprites;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;
    public float health = 3f;
    public float money = 0f;
    public TMP_Text UIText;

    private bool grounded;
    private bool attacking = false;
    private int weapon = 1;
    public GameObject hitbox;
    public GameObject card;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];

        attackSprites = punchSprites;
        //hitbox.SetActive(false);
    }

    private void OnEnable() {
        InvokeRepeating(nameof(AnimateSprite), 1f/20f, 1f/20f);
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

        UIText.SetText("Health: " + health.ToString() + " Money: " + money.ToString());
        
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

        //left click to attack
        if(!attacking && Input.GetButtonDown("Fire1")) {
            spriteIndex = 0;
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource attack = sounds[1];
            if(weapon == 3) {
                attack = sounds[2];
            }
            attack.Play();
            attacking = true;
        }

        //right click to throw cards
        if(Input.GetButtonDown("Fire2")) {
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource attack = sounds[1];
            attack.Play();
            GameObject newCard = Instantiate(card, transform.position, Quaternion.identity);
            newCard.GetComponent<Rigidbody2D>().AddForce(new Vector3(750 * transform.right.x, 0, 0));
        }

        //left shift to swap weapons
        if(Input.GetButtonDown("Fire3")) {
            if(weapon >= 3) {
                weapon = 1;
            }
            else weapon++;

            switch(weapon) {
                case 1:
                    attackSprites = punchSprites;
                    break;
                case 2:
                    attackSprites = daggerSprites;
                    break;
                case 3:
                    attackSprites = batSprites;
                    break;
            }
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
                //hitbox.SetActive(false);
                //DestroyImmediate(hitbox, true);
                attacking = false;
            }

            if(spriteIndex == 6) {
                //hitbox.SetActive(true);
                Instantiate(hitbox, new Vector3(transform.position.x + (1.5f * transform.right.x), transform.position.y, transform.position.z), Quaternion.identity);
            }

            spriteRenderer.sprite = attackSprites[spriteIndex];
        }
        else if(direction.x != 0f) {
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
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Security") || collision.gameObject.CompareTag("Rat")) {
            if(attacking) {
                if(collision.gameObject.CompareTag("Security")) {
                    money += 100;
                }
                else money += 50;
            }
            else {
                health -= 1;
                AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
                AudioSource hurt = sounds[3];
                hurt.Play();
            }
            //rigidbody.AddForce(transform.right * -direction.x, ForceMode2D.Impulse);
            
            if(health == 0) {
                enabled = false;
                FindAnyObjectByType<GameManager>().LevelFailed();
            }
        }
    }
}
