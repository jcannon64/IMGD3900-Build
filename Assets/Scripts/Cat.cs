using Unity.VisualScripting;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject player;
    private GameObject target;

    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;
    public int healCharges = 3;
    private bool grounded;
    private bool cooldown = false;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        PickTarget();
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

        direction += Physics2D.gravity * Time.deltaTime;

        Object enemy = FindAnyObjectByType<Enemy>();
        if(enemy == null || target == null) {
            PickTarget();
        }

        float targetLocation = target.transform.position.x - transform.position.x;
        if(healCharges <= 0) {
            direction.x = -moveSpeed * 2.5f;
        }
        else if(targetLocation < 1.5) {
            direction.x = -moveSpeed;
        }
        else if(targetLocation > 1.5) {
            direction.x = moveSpeed;
        }
        else direction.x = 0;

        if(grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if(direction.x > 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if(direction.x < 0f) {
            transform.eulerAngles = Vector3.zero;
        }
    }

    private void FixedUpdate() {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite() {
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
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject == target && healCharges > 0 && cooldown == false) {
            if(target == player) {
                collision.gameObject.GetComponent<Player>().TakeDamage(-25);
                healCharges--;
                cooldown = true;
                Invoke(nameof(EndCD), 2f);
            }
            else if(collision.gameObject.CompareTag("Rat")) {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
                PickTarget();
            }
            else {
                if(collision.gameObject.CompareTag("Boss")) {
                    collision.gameObject.GetComponent<Boss>().Heal(25);
                }
                else collision.gameObject.GetComponent<Enemy>().Heal(25);
                healCharges--;
                cooldown = true;
                Invoke(nameof(EndCD), 2f);
                PickTarget();
            }
        }
    }

    private void PickTarget() {
        Object enemy = FindAnyObjectByType<Enemy>();
        Object boss = FindAnyObjectByType<Boss>();
        if(boss != null) {
            target = boss.GameObject();
        }
        else if(enemy == null) {
            target = player;
        }
        else target = enemy.GameObject();
    }

    private void EndCD() {
        cooldown = false;
    }
}
