using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class PlayerManagerZ : MonoBehaviour
{
    public static float playerHealth, currnetScore;
    public Image healthImg;
    public TMP_Text scoreText, highScoreText;

    public GameObject gameON, gameOFF, enemies;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
            GameOver();
        healthImg.fillAmount = playerHealth;
        scoreText.text = currnetScore.ToString();
    }
    public void GameStart()
    {
        gameON.SetActive(true);
        gameOFF.SetActive(false);
        playerHealth = 1;
        currnetScore = 0;
        scoreText.text = "0";
    }
    public void GameOver()
    {
        if (currnetScore > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", currnetScore);
            highScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString();
        }
        foreach (Transform child in enemies.transform)
            child.gameObject.GetComponent<ZB>().Death();
        gameON.SetActive(false);
        gameOFF.SetActive(true);
        Debug.Log("quit");
    }
}