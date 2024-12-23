using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite _musicImg;
    [SerializeField] private Sprite _muteImg;
    public bool isMuted;
    [SerializeField] private List<Button> _musicButtons;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text timeCountDown;
    private int score;
    private float totalTime = 60f;
    public static GameManager Instance;
    private int correctCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        score = 0;
        scoreTxt.text = score.ToString();
        Time.timeScale = 1;
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        if (_musicButtons.Count != 0)
        {
            foreach (Button btn in _musicButtons)
            {
                btn.image.sprite = isMuted ? _muteImg : _musicImg;
                btn.onClick.AddListener(OnMusicButtonClick);
            }
        }
    }

    private void Update()
    {
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);
            timeCountDown.text = string.Format("{0:00}:{1:00}", Mathf.Max(minutes, 0), Mathf.Max(seconds, 0));

            if (totalTime <= 0)
            {
                StartCoroutine(ActiveLosePanel());
            }
        }
    }

    public void OnMusicButtonClick()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        foreach (Button btn in _musicButtons)
        {
            btn.image.sprite = isMuted ? _muteImg : _musicImg;
        }
    }

    public void BackToLevelScene()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void PlayGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToStartGame()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public IEnumerator ActiveLosePanel()
    {
        yield return new WaitForSeconds(1f);
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void StartCountdown(int duration)
    {
        totalTime = duration;
    }
    public void AddScore(int points)
    {
        score += points;
        scoreTxt.text = score.ToString();
    }
    public void UpdateCorrectCount()
    {
        correctCount++;
        AddScore(10);
        if(correctCount == 10)
        {
            correctCount = 0;
            CharactorManager.Instance.RandomizeCharactorCards();
            BoxManager.Instance.RandomizeBoxCards();    
        }
    }
}
