using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    public TextMeshProUGUI score;
    public int playerScore;

    void Update()
    {
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().score;
        score.text = playerScore.ToString();
    }
}
