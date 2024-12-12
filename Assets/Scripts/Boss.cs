using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public GameObject player;
    public GameObject spawner;
    public GameObject platform;
    public GameObject sign;

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
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
    public float MaxHealth;
    public float damage = 1f;
    private bool grounded;
    private bool phaseTwo = false;
    public TMP_Text healthText;

    [SerializeField] HealthBar healthBar;
    //private HealthBar healthBar;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        damage = damage + (20 * Manager.loops);
        health = health + (50 * Manager.loops);
        healthBar = GetComponentInChildren<HealthBar>();
        MaxHealth = health;
    }

    /*private void OnEnable() {
        InvokeRepeating(nameof(AnimateSprite), 1f/20f, 1f/20f);
    }

    private void OnDisable() {
        CancelInvoke();
    }*/

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
            else if(hit.layer == LayerMask.NameToLayer("Player")) {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);
            }
        }
    }

    private void Update() {
        CheckCollision();

        healthText.SetText(health.ToString());
        if(health <= MaxHealth / 2 && phaseTwo == false) {
            EndPhaseOne();
        }

        direction += Physics2D.gravity * Time.deltaTime;

        float playerLocation = player.transform.position.x - transform.position.x;
        float playerHeight = player.transform.position.y - transform.position.y;
        if(grounded && playerHeight > 6.5 && playerLocation < 3 && playerLocation > -3) {
            direction = Vector2.up * jumpStrength;
        }
        else if(playerLocation < 0) {
            direction.x = -moveSpeed;
        }
        else if(playerLocation > 0) {
            direction.x = moveSpeed;
        }
        else direction.x = 0;

        if(grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if(direction.x > 0f) {
            transform.eulerAngles = Vector3.zero;
            healthText.transform.eulerAngles = Vector3.zero;
            healthBar.transform.eulerAngles = Vector3.zero;
        }
        else if(direction.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            healthText.transform.eulerAngles = Vector3.zero;
            healthBar.transform.eulerAngles = Vector3.zero;
        }
    }

    private void FixedUpdate() {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Hitbox")) {
            TakeDamage(FindAnyObjectByType<Player>().damage);
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Card")) {
            TakeDamage(FindAnyObjectByType<Player>().damage);
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Explosion")) {
            TakeDamage(10);
        }
        else if(collision.gameObject.CompareTag("Player")) {
            FindAnyObjectByType<Player>().TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        healthBar.UpdateHealthBar(health, MaxHealth);
        AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
        AudioSource hurt = sounds[0];
        hurt.Play();

        if(health <= 0) {
            FindAnyObjectByType<Player>().GainChips(500);
            gameObject.SetActive(false);
            FindAnyObjectByType<Player>().KillReduction();
            FindAnyObjectByType<SignToggle>().toggle();
            FindAnyObjectByType<SignToggle1>().toggle();
        }

        spriteRenderer.sprite = sprites[1];
        Invoke(nameof(normalizeSprite), 0.25f);
    }

    public void Heal(float healing) {
        health += healing;
        healthBar.UpdateHealthBar(health, MaxHealth);
    }

    private void normalizeSprite() {
        spriteRenderer.sprite = sprites[0];
    }

    private void EndPhaseOne() {
        phaseTwo = true;
        direction = Vector2.up * jumpStrength * 2.5f;
        moveSpeed = 0;
        Invoke(nameof(StartPhaseTwo), 7.5f);
    }

    private void StartPhaseTwo() {
        Destroy(platform);
        moveSpeed = 4f;
    }
}