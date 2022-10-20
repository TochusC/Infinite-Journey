using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Camera cam;
    public GameObject Player;
    protected Rigidbody2D UnitRb;
    protected bool isLasering = false;
    protected Vector3 randomPosition;
    protected Vector3 moveDireciton;
    public float laseringModified = 1f;
    public bool isFriend = false;
    public bool isBoss = false;
    public bool isFinalBoss = false;
    public bool lost = false;
    [SerializeField] public float minlaseringModified = 0.15f;
    [SerializeField] public AudioSource unitAudioSource;
    [SerializeField] public AudioClip PrimSound;
    [SerializeField] public AudioClip SecondSound;
    [SerializeField] public AudioClip SpecialSound;
    [SerializeField] public AudioClip HittedSound;
    [SerializeField] public AudioClip desSound;
    [SerializeField] public AudioClip PlayerHittedSound;
     protected bool isRest = true;
    [SerializeField] protected float AttackDelay = 1.25f;
    [SerializeField] protected float secondFireDelay = 3f;
    [SerializeField] protected float specialSkillDelay = 5f;
    [SerializeField] protected float nonPlayerModified = 0.6f;
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected GameObject hittedEffect;
    [SerializeField] protected GameObject destroyedEffect;
    [SerializeField] protected GameObject myProjectile;
    [SerializeField] protected GameObject[] bulletPefab;
    [SerializeField] protected GameObject[] firePlace;
    [SerializeField] protected GameObject[] secondFirePlace;
    [SerializeField] protected GameObject[] specialSkillPlace;
    [SerializeField] public bool isPlayer = false;
    [SerializeField] protected float playerBonus = 0.02f;
    [SerializeField] protected float fireDelay = 0.25f;
    [SerializeField] protected float Speed = 4;
    [SerializeField] protected float roamSpeed = 2;
    [SerializeField] protected float roamRange = 2f;
    [SerializeField] protected float contactRange = 0.01f;
    [SerializeField] public float sethealth = 100;
    [SerializeField] public float setmaxHealth = 100;
    protected Vector3 RotationDirection;
    public float playerBoost = 1f;
    protected bool isCoolDown = false;
    public bool isImmune = false;
    protected bool isLasered = false;
    protected bool isSecondFireCoolDown = false;
    protected bool isSpecialSkillCoolDown = false;
    protected bool isRoaming = false;
    protected bool isArrived = false;
    public bool isBoosted = false;
    [SerializeField] public float health = 100;
    public float maxHealth = 100;
    protected float lasedDelay = 0.2f;
    [SerializeField] private float healingSpeed = 5;
    private float healingDelay = 1;
    public bool isHealing = true;
    public bool isIntialized = false;
    [SerializeField] protected int healingCoolDown = 5;
     protected float hitDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
        unitAudioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(CoolDown());
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        health = (int)(health * nonPlayerModified);
        maxHealth = (int)(maxHealth * nonPlayerModified);
        cam = Camera.main;
        UnitRb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Initialized()
    {

    }
    void Update()
    {
        if (isLasering)
            laseringModified = minlaseringModified;
        else
            laseringModified = 1f;
        if(!gameManager.isPlayerDead)
            Player = GameObject.FindGameObjectWithTag("Player");
        if (isPlayer)
        {
            hitDelay += Time.deltaTime;
            if (hitDelay >= healingCoolDown)
                isHealing = true;
            PlayerControl();
            if (!isIntialized)
            {
                health = sethealth;
                maxHealth = setmaxHealth;
                StartCoroutine(Healing());
                isIntialized = true;
            }
        }
        else if (isFriend)
        {
            AllyPlayer();
        }
        else
        {
            AttackPlayer();
        }
        PositionCostrain();
    }

    protected virtual void AllyPlayer() {
        AllyMove();
    }
    protected virtual void AllyMove() { 
    
    }

    protected virtual void AllyAttack()
    {

    }

    protected void LateUpdate()
    {
        if (isPlayer)
            PlayerAttack();
        else if (isFriend)
            AllyAttack();
        else
        {
            if (isRest && !gameManager.isPlayerDead)
            {
                StartCoroutine(Active());
            }
            else
            {
                if (!gameManager.isPlayerDead)
                {
                    EnermyAttack();
                }
                else
                {
                    isRest = true;
                }
            }
        }
    }

    protected void EnermyAttack()
    {
        PrimeAttack();
        SecondAttack();
        SpecialSkill();
    }
    virtual protected void AttackPlayer()
    {

    }
    virtual protected void PlayerControl()
    {
        PlayerMove();
    }
    protected IEnumerator Active()
    {
        yield return new WaitForSeconds(Random.Range(AttackDelay, AttackDelay + 2));
        isRest = false;
    }
    IEnumerator Healing()
    {
        yield return new WaitForSeconds(healingDelay);
        if (isHealing)
        {
            if (health + healingSpeed > maxHealth)
                health = maxHealth;
            else
                health += healingSpeed;
        }
        StartCoroutine(Healing());
    }

    IEnumerator HealingCoolDown()
    {
        yield return new WaitForSeconds(healingCoolDown);
        isHealing = true;
    }

    virtual protected void PlayerAttack()
    {
        if (Input.GetMouseButton(0))
        {
            PrimeAttack();
        }
        if (Input.GetMouseButton(1))
        {
            SecondAttack();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpecialSkill();
        }
    }

    protected void Fire(GameObject bullet, GameObject[] firePlace_t)
    {
        for(int fireCount = 0; fireCount < firePlace_t.Length; fireCount++)
        {
            myProjectile = Instantiate(bullet, firePlace_t[fireCount].transform.position, firePlace_t[fireCount].transform.rotation);
            myProjectile.GetComponent<BulletBehavior>().shooter = gameObject;
        }
    }


    virtual protected void PlayerMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        UnitRb.AddForce((Vector3.right * horizontalInput + Vector3.up * verticalInput).normalized * Speed * Time.deltaTime * laseringModified, ForceMode2D.Impulse);
    }

    protected virtual void PrimeAttack() { }
    protected virtual void SecondAttack() { }
    protected virtual void SpecialSkill() { }
    protected void PositionCostrain()
    {
        if (transform.position.x > gameManager.rightBoundary)
            transform.position = new Vector3(gameManager.rightBoundary, transform.position.y, transform.position.z);
        if (transform.position.x < -gameManager.rightBoundary)
            transform.position = new Vector3(-gameManager.rightBoundary, transform.position.y, transform.position.z);
        if (transform.position.y > gameManager.upBoundary)
            transform.position = new Vector3(transform.position.x, gameManager.upBoundary, transform.position.z);
        if (transform.position.y < -gameManager.upBoundary)
            transform.position = new Vector3(transform.position.x, -gameManager.upBoundary, transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            DamageUnit(collision);
        }
        if (collision.gameObject.CompareTag("Laser"))
        {
            LaserDamageUnit(collision);
        }
        if (collision.gameObject.CompareTag("Shockwave"))
        {
            BulletBehavior bullet = collision.gameObject.GetComponent<BulletBehavior>();
            Vector3 forceDirection = (bullet.shooter.transform.position - transform.position).normalized;
            UnitRb.AddForce(forceDirection * bullet.pushForce, ForceMode2D.Impulse);
        }
    }
    protected void LaserDamageUnit(Collider2D other)
    {
        if (!isLasered && other.gameObject.GetComponent<BulletBehavior>().shooter != gameObject)
        {
            isLasered = true;
            StartCoroutine(LaseredCoolDown());
            BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
            Unit ShooterStatus = bullet.shooter.GetComponent<Unit>();
            if (isPlayer || isFriend)
            {
                if (!ShooterStatus.isPlayer && !ShooterStatus.isFriend)
                {
                    isHealing = false;
                    hitDelay = 0;
                    bullet.bulletDamage *= 0.8f;
                    if (isBoosted)
                        bullet.bulletDamage *= 0.5f;
                    if (!isImmune)
                    {
                        health -= bullet.bulletDamage;
                        Instantiate(hittedEffect, transform.position + new Vector3(0, 0, -0.5f), hittedEffect.transform.rotation);
                        if (isPlayer)
                            unitAudioSource.PlayOneShot(PlayerHittedSound, 0.1f);
                        else
                            unitAudioSource.PlayOneShot(HittedSound, 0.1f);
                        CheckUnitHealth(bullet);
                        if (!bullet.doNotDestoryOnHit)
                            Destroy(other.gameObject);
                    }
                }
            }
            else
            {
                if (bullet.shooter.tag != gameObject.tag)
                {
                    isHealing = false;
                    hitDelay = 0;
                    if (isBoosted)
                        bullet.bulletDamage *= 0.5f;
                    if (!isImmune)
                    {
                        health -= bullet.bulletDamage;
                        Instantiate(hittedEffect, transform.position + new Vector3(0, 0, -0.5f), hittedEffect.transform.rotation);
                        if (isPlayer)
                            unitAudioSource.PlayOneShot(PlayerHittedSound, 0.1f);
                        else
                            unitAudioSource.PlayOneShot(HittedSound, 0.1f);
                        CheckUnitHealth(bullet);
                        if (!bullet.doNotDestoryOnHit)
                            Destroy(other.gameObject);
                    }
                }

            }
        }
    }
    protected void DamageUnit(Collider2D other)
    {
        BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
        Unit ShooterStatus = bullet.shooter.GetComponent<Unit>();
        if (isPlayer || isFriend)
        {
            if (!ShooterStatus.isPlayer&&!ShooterStatus.isFriend)
            {
                isHealing = false;
                hitDelay = 0;
                if (isBoosted)
                    bullet.bulletDamage *= 0.15f;
                bullet.bulletDamage *= 0.75f;
                if (!isImmune)
                {
                    health -= bullet.bulletDamage;
                    if(other.gameObject.tag == "Laser")
                    {
                        Instantiate(hittedEffect, transform.position + new Vector3(0, 0, -0.5f), hittedEffect.transform.rotation);
                    }
                    else
                    {
                        Instantiate(hittedEffect, transform.position + new Vector3(0, 0, -0.5f), hittedEffect.transform.rotation);
                    }
                    
                    if (isPlayer)
                        unitAudioSource.PlayOneShot(PlayerHittedSound, 0.1f);
                    else
                        unitAudioSource.PlayOneShot(HittedSound, 0.1f);
                    CheckUnitHealth(bullet);
                    if (!bullet.doNotDestoryOnHit)
                        Destroy(other.gameObject);
                }
            }
        }
        else
        {
            if (bullet.shooter.tag != gameObject.tag)
            {
                isHealing = false;
                hitDelay = 0;
                if (isBoosted)
                    bullet.bulletDamage *= 0.25f;
                if (!isImmune)
                {
                    health -= bullet.bulletDamage;
                    Instantiate(hittedEffect, bullet.transform.position + new Vector3(0,0, -0.5f), hittedEffect.transform.rotation);
                    if (isPlayer)
                        unitAudioSource.PlayOneShot(PlayerHittedSound, 0.1f);
                    else
                        unitAudioSource.PlayOneShot(HittedSound, 0.1f);
                    CheckUnitHealth(bullet);
                    if (!bullet.doNotDestoryOnHit)
                        Destroy(other.gameObject);
                }
            }   

        }
    }
    protected void CheckUnitHealth(BulletBehavior bullet)
    {
        if (health <= 0)
        {
            Instantiate(destroyedEffect, gameObject.transform.position, destroyedEffect.transform.rotation);
            if (isPlayer)
            {
                gameManager.isPlayerDead = true;
                gameManager.PlayerUnit = bullet.shooter.GetComponent<Unit>();
                gameManager.NewPlayer = bullet.shooter;
                Destroy(gameObject);
            }
            else
            {
                Unit playerStatus = Player.GetComponent<Unit>();
                playerStatus.playerBoost += playerBonus;
                playerStatus.maxHealth = playerStatus.setmaxHealth * playerStatus.playerBoost;
                playerStatus.health += playerStatus.healingSpeed * 0.2f;
                Destroy(gameObject);
            }
        }
    }
    protected IEnumerator CoolDown()
    {
        isCoolDown = true;
        if (gameObject.CompareTag("Enermy"))
        {
            yield return new WaitForSeconds(fireDelay * (3 - 2 * nonPlayerModified));
        }
        else
        {
            yield return new WaitForSeconds(fireDelay / playerBoost);
        }
        isCoolDown = false;
    }

    protected IEnumerator SecondFireCoolDown()
    {
        isSecondFireCoolDown = true;
        if (gameObject.CompareTag("Enermy"))
        {
            yield return new WaitForSeconds(secondFireDelay * (3 - 2 * nonPlayerModified));
        }
        else
        {
            yield return new WaitForSeconds(secondFireDelay / playerBoost);
        }
        isSecondFireCoolDown = false;
    }
    
    protected IEnumerator SpecialSkillCoolDown()
    {
        isSpecialSkillCoolDown = true;
        if (gameObject.CompareTag("Enermy"))
        {
            yield return new WaitForSeconds(specialSkillDelay * (3 - 2 * nonPlayerModified));
        }
        else
        {
            yield return new WaitForSeconds(specialSkillDelay / playerBoost);
        }
        isSpecialSkillCoolDown = false;
    }
    protected IEnumerator LaseredCoolDown()
    {
        yield return new WaitForSeconds(lasedDelay);
        isLasered = false;
    }

    protected Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-roamRange, roamRange), Random.Range(-roamRange, roamRange), 0);
    }

    protected Vector3 PositionConstrain(Vector3 position)
    {
        if (abs(position.x) > gameManager.rightBoundary || abs(position.y) > gameManager.upBoundary)
        {
            Vector3 refinedPosition = GetRandomDirection();
            return refinedPosition;
        }
        return position;
    }

    protected float abs(float num)
    {
        return num > 0 ? num : -num;
    }
}
