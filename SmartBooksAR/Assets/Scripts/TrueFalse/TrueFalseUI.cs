using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrueFalseUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_Text timerText;

    [Header("Large Text")]
    [SerializeField] TMP_Text largeText;
    [SerializeField] CanvasGroup largeTextCG;
    [SerializeField] float textFadeInTime;

    [Header("Buttons")]
    [SerializeField] Button RestartButton;
    [SerializeField] Button QuitButton;
    private int score;
    private TrueFalseQuestionSpawner tfQuestionSpawner;
    [Header("Trophy")]
    public GameObject scoreTrophy;
    public GameObject winningTrophy;
    public TMP_Text winningScoreText;
    [SerializeField] CanvasGroup fadeCG;

    private void Awake()
    {
        scoreText.SetText("0");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    void Start()
    {
        tfQuestionSpawner = FindObjectOfType<TrueFalseQuestionSpawner>();
        tfQuestionSpawner.OnUpdateScore += UpdateScore;
        tfQuestionSpawner.OnUpdateTimeRemaining += UpdateTimeRemaining;
        tfQuestionSpawner.OnDisplayLargeText += DisplayLargeText;
        tfQuestionSpawner.OnSpawnNextQuestion += UpdateQuestion;
        tfQuestionSpawner.OnFinished += EnableTrophy;
        RestartButton.onClick.AddListener(Restart);
        QuitButton.onClick.AddListener(Quit);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Quit()
    {
        SceneManager.LoadScene(2);
    }

    void EnableTrophy() 
    {
        StartCoroutine(FadeInImage(1f, fadeCG));
        scoreTrophy.SetActive(false);
        winningScoreText.SetText(score.ToString());
        winningTrophy.SetActive(true);
    }
    void UpdateScore(int val) 
    {
        Debug.Log("updating score text");
        score += val;
        scoreText.SetText(score.ToString());
    }
    void DisplayLargeText(string text)
    {
        largeText.SetText(text);
        StartCoroutine(FadeInImage(textFadeInTime, largeTextCG));
    }

    //TO DO: This Fading code is common to most of the different UI classes. Maybe the UIs could all inherit from a base UI class that contain this function definition?
    IEnumerator FadeInImage(float fadeInTime, CanvasGroup cg)
    {
        cg.gameObject.SetActive(true);
        float timeElapsed = 0f;
        while (timeElapsed < fadeInTime)
        {
            cg.alpha = fadeInTime - timeElapsed/fadeInTime;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0f;
        cg.gameObject.SetActive(false);

    }
    void UpdateTimeRemaining(string time) 
    {
        timerText.SetText(time);
    }
    void UpdateQuestion(string text) 
    {
        questionText.SetText(text);
    }

}
