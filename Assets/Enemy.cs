using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int random;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public Animator animator;
    public Transform player;
    public bool isFlipped = false;
    public PlayerCombat playerCharacter;
    public bool immune = false;
    void Start()
    {
        currentHealth = maxHealth;
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    public void LookAtPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void Attack()
    {
        playerCharacter.Hit();
    }

    public void TakeDamage(int damage)
    {
        if (immune)
        {
            Debug.Log("Immune");
            return;
        }
        if(currentHealth == 1 && damage <= 100)
        {
            animator.SetTrigger("Hurt");
            playerCharacter.Heal();
            return;
        }
        else if(currentHealth == 1 && damage > 100000)
        {
            Die();
        }
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        Debug.Log("Enemy Health is " + currentHealth);
        if (currentHealth <= 0)
        {
            if(damage < 100000)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().score++;
            }
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died");
        animator.SetBool("Dead", true);
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject);
        this.enabled = false;
    }

    public void Respawn()
    {
        random = Random.Range(1, 3);
        if(random == 1)
        {
            transform.position = new Vector3(Random.Range(-14.5f, -3f), -0.68f, 0f);
        }
        else
        {
            transform.position = new Vector3(Random.Range(14.5f, 3f), -0.68f, 0f);
        }
    }
}
