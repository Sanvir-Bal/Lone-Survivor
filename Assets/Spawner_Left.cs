using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Left : MonoBehaviour
{
    [SerializeField]
    private GameObject lightbanditPrefab;
    [SerializeField]
    private GameObject heavybanditPrefab;

    [SerializeField]
    private float lightbanditSpawnInterval = 3.5f;
    [SerializeField]
    private float heavybanditSpawnInterval = 10f;

    private int heavybanditupdateThreshold = 25;
    private int lightbanditupdateThreshold = 25;
    public int playerScore;
    public bool playerAlive;

    void Start()
    {
        StartCoroutine(spawnEnemy(lightbanditSpawnInterval, lightbanditPrefab, lightbanditupdateThreshold));
        StartCoroutine(spawnEnemy(heavybanditSpawnInterval, heavybanditPrefab, heavybanditupdateThreshold));
        playerAlive = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().alive;
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy, int threshold)
    {
        playerAlive = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().alive;
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().score;
        if (playerAlive)
        {
            if (playerScore >= threshold)
            {
                interval *= Random.Range(0.9f, 1f);
                threshold += threshold;
            }
            Debug.Log("Spawn Left Interval is " + interval);
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-14.5f, -2.75f), -1.15f, 0), Quaternion.identity);
        }
        else
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject eachEnemy in enemies)
            {
                eachEnemy.GetComponent<Enemy>().TakeDamage(999999);
            }
            yield return new WaitForSeconds(interval);
        }
        StartCoroutine(spawnEnemy(interval, enemy, threshold));
    }

    
}
