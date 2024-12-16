using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro scoreBotText;
    public TMPro.TextMeshPro result;
    static string rtext;
    static int comboScore;
    static int comboScoreBot;
    public Animator botAnimator; // Ссылка на Animator бота
    void Start()
    {
        Instance = this;
        Instance.botAnimator.speed = 8f;
        comboScore = 0;
        comboScoreBot = 0;
    }

    public static void ResetScore()
    {
        rtext = "";
        comboScore = 0;
        comboScoreBot = 0;
    }
    public static void Hit()
    {
        comboScore += 1;
        Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        Instance.missSFX.Play();
    }
    private void Update()
    {
        result.text = rtext;
        scoreText.text = comboScore.ToString();
        scoreBotText.text = comboScoreBot.ToString();
    }

    public static void HitBotLeft()
    {
        comboScoreBot += 1;
        Instance.botAnimator.SetTrigger("LeftArm"); // Запуск анимации левой руки
    }
    public static void HitBotRight()
    {
        comboScoreBot += 1;
        Instance.botAnimator.SetTrigger("RightArm"); // Запуск анимации правой руки
    }
    public static void MissBot()
    {
    }
    public static void CheckScores()
    {
        if(comboScoreBot > comboScore)
        {
            rtext = "BOT WINS";
        }
        else if(comboScoreBot < comboScore) 
        {
            rtext = "PLAYER WINS";
        }
        else
        {
            rtext = "WE HAVE A TIE";
        }
    }
}