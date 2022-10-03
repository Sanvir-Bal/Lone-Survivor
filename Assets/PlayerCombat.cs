using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    private bool facingRight = true;
    public bool alive = true;
    public bool invincible = false;
    public Transform attackPoint;
    public float attackRange = 0.42f;
    public LayerMask enemyLayers;
    public int attackDamage = 50;
    public float attackRate = 10f;
    public float nextAttackTime = 0f;
    public float missAttackRate = 0.5f;
    public int numHit = 0;
    public int maxHealth = 10;
    public int playerHealth = 10;
    public int playerDeathHealth = 0;
    public Spawner_Right rightSpawner;
    public Spawner_Left leftSpawner;
    public Health_Bar healthBar;
    public Health_Bar deathBar;
    public Health_Bar takenBar;
    public GameObject harbinger;
    public int score = 0;
    public bool playerDeath;
    private int random;

    void Start()
    {
        Time.timeScale = 1f;
        playerHealth = maxHealth;
        playerDeathHealth = maxHealth;
        score = 0;
        healthBar.setMaxHealth(maxHealth);
        deathBar.setMaxHealth(maxHealth);
        takenBar.setMaxHealth(maxHealth);
        deathBar.setHealth(0);
        takenBar.setHealth(0);
        playerDeath = false;
        alive = true;
    }
    void Update()
    {
        if(Time.timeSinceLevelLoad >= nextAttackTime && !Pause_Menu.GameIsPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttackLeft();
                if (numHit == 0)
                {
                    nextAttackTime = Time.timeSinceLevelLoad + 1f / missAttackRate;
                }
                else
                {
                    nextAttackTime = 0;
                    numHit = 0;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                AttackRight();
                if (numHit == 0)
                {
                    nextAttackTime = Time.timeSinceLevelLoad + 1f / missAttackRate;
                }
                else
                {
                    nextAttackTime = 0;
                    numHit = 0;
                }
            }   
        }  
    }

    void AttackLeft()
    {
        if (facingRight)
        {
            animator.SetTrigger("AttackTurn");
            transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
        else
        {
            animator.SetTrigger("Attack");
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            numHit++;
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log("We hit" + enemy.name);
        }
    }

    void AttackRight()
    {
        if (!facingRight)
        {
            animator.SetTrigger("AttackTurn");
            transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        }
        else
        {
            animator.SetTrigger("Attack");
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            numHit++;
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log("We hit" + enemy.name);
        }
    }

    public void Hit()
    {
        if (invincible)
        {
            return;
        }
        if (alive)
        {
            playerHealth--;
            healthBar.setHealth(playerHealth);
            animator.SetTrigger("Hit");
            Debug.Log("Player Health is " + playerHealth);
            if (playerHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            playerDeathHealth--;
            deathBar.setHealth(playerDeathHealth);
            if(playerDeathHealth == 0)
            {
                takenBar.setHealth(10 - playerHealth);
                GameObject harbinger = GameObject.FindGameObjectWithTag("Harbinger");
                harbinger.GetComponent<Enemy>().immune = false;
                harbinger.GetComponent<Enemy>().TakeDamage(999999);
                animator.SetBool("Dead", true);
                this.enabled = false;
            }
            animator.SetTrigger("Hit");
            Debug.Log("Player Death Health is " + playerDeathHealth);
        }
    }

    public void Heal()
    {
        if(playerHealth < maxHealth)
        {
            playerHealth++;
            healthBar.setHealth(playerHealth);
        }
        if(playerHealth >= playerDeathHealth)
        {
            alive = true;
            maxHealth = playerHealth;
            takenBar.setHealth(10 - playerHealth);
            GameObject harbinger = GameObject.FindGameObjectWithTag("Harbinger");
            harbinger.GetComponent<Enemy>().immune = false;
            harbinger.GetComponent<Enemy>().TakeDamage(999999);
        }
    }

    public void spawnHarbinger()
    {
        random = Random.Range(1, 3);
        if (random == 1)
        {
            GameObject death = Instantiate(harbinger, new Vector3(Random.Range(14.5f, 3f), -0.68f, 0), Quaternion.identity);
        }
        else
        {
            GameObject death = Instantiate(harbinger, new Vector3(Random.Range(-14.5f, -3f), -0.68f, 0), Quaternion.identity);
        }
    }

    void Die()  
    {
        Debug.Log("Player Died");
        //GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("FightForLife");
        alive = false;
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(999999);
        }
        deathBar.setHealth(maxHealth);
    }

    public void Restart()
    {
        playerDeath = true;
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
