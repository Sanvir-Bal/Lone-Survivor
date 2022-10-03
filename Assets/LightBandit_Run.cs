using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBandit_Run : StateMachineBehaviour
{
    public float speed = 1f;
    public float attackRange = 0.48f;
    public float attackRate = 0.33f;
    public float nextAttackTime = 0f;
    Transform player;
    Rigidbody2D body;
    Enemy enemy;
    public float updateTime = 1f;
    public float updateRate = 1f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        body = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
        if (Time.timeSinceLevelLoad >= updateTime)
        {
            speed += (Time.timeSinceLevelLoad * 0.01f);
            updateTime = Time.timeSinceLevelLoad + 1f / updateRate;
        }
        Debug.Log("Light Speed is " + speed);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, body.position.y);
        Vector2 newPosition = Vector2.MoveTowards(body.position, target, speed * Time.fixedDeltaTime);
        body.MovePosition(newPosition);
        if (Vector2.Distance(player.position, body.position) <= attackRange)
        {
            if (Time.timeSinceLevelLoad >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.timeSinceLevelLoad + 1f / attackRate;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
