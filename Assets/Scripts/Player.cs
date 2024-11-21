using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] attackSprites;
    public Sprite[] punchSprites;
    public Sprite[] daggerSprites;
    public Sprite[] batSprites;
    public Sprite[] glassSprites;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;
    public float health = 3f;
    public float chips = 0f;
    public float damage = 25f;
    public float spadesAmmo, heartsAmmo, clubsAmmo, diamondsAmmo = 4f;
    public TMP_Text UIText;
    public TMP_Text SuitText;
    public Image cardArt;
    public Sprite spades;
    public Sprite hearts;
    public Sprite clubs;
    public Sprite diamonds;
    public int killCount = 7;
    public bool StageStart = true;

    private bool grounded;
    private bool attacking = false;
    private int weapon = 1;
    public GameObject punchHitbox, spadesCard;
    public GameObject daggerHitbox, heartsCard;
    public GameObject batHitbox, clubsCard;
    public GameObject glassHitbox, diamondsCard;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];

        attackSprites = punchSprites;
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
        if(StageStart == true)
        {
            killCount = 7;
            StageStart= false;
        }
        
        UIText.SetText("Health: " + health.ToString() + " Chips: " + chips.ToString() + "\n" + " Enemies remaining: " + killCount.ToString());
        switch(weapon) {
            case 1:
                SuitText.SetText("Spades " + " Ammo: " + spadesAmmo);
                cardArt.sprite = spades;
                break;
            case 2:
                SuitText.SetText("Hearts " + " Ammo: " + heartsAmmo);
                cardArt.sprite = hearts;
                break;
            case 3:
                SuitText.SetText("Clubs " + " Ammo: " + clubsAmmo);
                cardArt.sprite = clubs;
                break;
            case 4:
                SuitText.SetText("Diamonds " + " Ammo: " + diamondsAmmo);
                cardArt.sprite = diamonds;
                break;
        }
        
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
            else if(weapon == 4) {
                attack = sounds[4];
            }
            attack.Play();
            attacking = true;
        }

        //right click to throw cards
        if(Input.GetButtonDown("Fire2")) {
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource card = sounds[5];
            
            GameObject newCard;
            switch(weapon) {
                case 1:
                    if(spadesAmmo > 0) {
                        newCard = Instantiate(spadesCard, new Vector3(transform.position.x + (2f * transform.right.x), transform.position.y, transform.position.z), new Quaternion(0, 0, 90, 0));
                        newCard.GetComponent<Rigidbody2D>().AddForce(new Vector3(1000 * transform.right.x, 0, 0));
                        spadesAmmo -= 1;
                        card.Play();
                    }
                    break;
                case 2:
                    if(heartsAmmo > 0) {
                        newCard = Instantiate(heartsCard, new Vector3(transform.position.x + (2f * transform.right.x), transform.position.y, transform.position.z), new Quaternion(0, 0, 90, 0));
                        newCard.GetComponent<Rigidbody2D>().AddForce(new Vector3(1000 * transform.right.x, 0, 0));
                        heartsAmmo -= 1;
                        card.Play();
                    }
                    break;
                case 3:
                    if(clubsAmmo > 0) {
                        newCard = Instantiate(clubsCard, new Vector3(transform.position.x + (2f * transform.right.x), transform.position.y, transform.position.z), new Quaternion(0, 0, 90, 0));
                        newCard.GetComponent<Rigidbody2D>().AddForce(new Vector3(1000 * transform.right.x, 0, 0));
                        clubsAmmo -= 1;
                        card.Play();
                    }
                    break;
                case 4:
                    if(diamondsAmmo > 0) {
                        newCard = Instantiate(diamondsCard, new Vector3(transform.position.x + (2f * transform.right.x), transform.position.y, transform.position.z), new Quaternion(0, 0, 90, 0));
                        newCard.GetComponent<Rigidbody2D>().AddForce(new Vector3(1000 * transform.right.x, 0, 0));
                        diamondsAmmo -= 1;
                        card.Play();
                    }
                    break;
            }
        }

        //left shift to swap weapons
        if(Input.GetButtonDown("Fire3")) {
            if(weapon >= 4) {
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
                case 4:
                    attackSprites = glassSprites;
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
                attacking = false;
            }

            switch(weapon) {
                case 1:
                    if(spriteIndex == 7) {
                        Instantiate(punchHitbox, new Vector3(transform.position.x + (1.5f * transform.right.x), transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    break;
                case 2:
                    if(spriteIndex == 5) {
                        Instantiate(daggerHitbox, new Vector3(transform.position.x + (1.5f * transform.right.x), transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    break;
                case 3:
                    if(spriteIndex == 7) {
                        Instantiate(batHitbox, new Vector3(transform.position.x + (1.5f * transform.right.x), transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    break;
                case 4:
                    if(spriteIndex == 8) {
                        Instantiate(glassHitbox, new Vector3(transform.position.x + (1.5f * transform.right.x), transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    break;
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
            //GameObject damageSource = GameObject.Find(collision.gameObject.name);
            //TakeDamage(FindFirstObjectByType<Enemy>().damage);
            //TakeDamage(damageSource.GetComponent<float>().damage);
            AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
            AudioSource hurt = sounds[3];
            hurt.Play();
            
            if(health <= 0) {
                enabled = false;
                FindAnyObjectByType<GameManager>().LevelFailed();
            }
        }
        else if(collision.gameObject.CompareTag("To Level")) {
            FindAnyObjectByType<GameManager>().LoadLevel(1);
        }
        else if(collision.gameObject.CompareTag("To Shop")) {
            FindAnyObjectByType<GameManager>().LoadLevel(3);
        }
        else if(collision.gameObject.CompareTag("To Next Area"))
        {
            if(killCount == 0)
            {
                if(FindAnyObjectByType<GameManager>().currLevel() == 1)
                {
                    FindAnyObjectByType<GameManager>().LoadLevel(4);
                }
                else if(FindAnyObjectByType<GameManager>().currLevel() == 4)
                {
                    FindAnyObjectByType<GameManager>().LoadLevel(1);
                }
            }
        }
        else if(collision.gameObject.CompareTag("Hearts Ammo")) {
            heartsAmmo += 1;
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Spades Ammo")) {
            spadesAmmo += 1;
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Clubs Ammo")) {
            clubsAmmo += 1;
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Diamonds Ammo")) {
            diamondsAmmo += 1;
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
    }

    public void GainChips(float amount) {
        chips += amount;
    }

    public void KillReduction()
    {
        killCount--;
    }

    public void NextStage()
    {
        if(killCount == 0)
        {
            //display the signs on the stage in respective locations, then change the edge of the map objects to go to respective areas
        }
    }
}
