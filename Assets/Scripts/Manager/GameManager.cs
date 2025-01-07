using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public bool isMuted;
    [SerializeField] private List<Button> _musicButtons;
    [SerializeField] private List<Text> musicStateTxt;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private List<Text> scoreTxts;
    [SerializeField] private Text timeCountDown;
    [SerializeField] private GameObject pauseContainer;
    [SerializeField] private GameObject endContainer;
    [SerializeField] private GameObject containerFill;
    [SerializeField] private GameObject fill;
    [SerializeField] private GameObject pauseBtn;
    [SerializeField] private List<Image> background;
    [SerializeField] private List<Sprite> spriteBg;
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject turtorial;
    private int score;
    private float totalTime = 60f;
    public static GameManager Instance;
    private int correctCount = 0;
    public bool isPlaying;

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
        foreach(Text txt in scoreTxts)
        {
            txt.text = score.ToString();

        }
        Time.timeScale = 1;
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        if (_musicButtons.Count != 0)
        {
            foreach (Button btn in _musicButtons)
            {
                btn.onClick.AddListener(OnMusicButtonClick);
            }
        }
        if (isMuted)
        {
            foreach (Text txt in musicStateTxt)
            {
                txt.text = "off";
            }
        }
        else
        {
            foreach (Text txt in musicStateTxt)
            {
                txt.text = "on";
            }
        }
        foreach (var item in background)
        {
            item.sprite = spriteBg[PlayerPrefs.GetInt("GameMode")];
        }
        if(PlayerPrefs.GetInt("GameMode") == 0)
        {
            totalTime = 60;
            if (timeCountDown != null)
                timeCountDown.text = totalTime.ToString();
        }
        else if(PlayerPrefs.GetInt("GameMode")== 1)
        {
            totalTime = 90;
            if (timeCountDown != null)
                timeCountDown.text = totalTime.ToString();
        }
        else if (PlayerPrefs.GetInt("GameMode") == 2)
        {
            totalTime = 120;
            if (timeCountDown != null)
                timeCountDown.text = totalTime.ToString();
        }
        StartCoroutine(StartAnim());
    }

    private void Update()
    {
        if (totalTime > 0 && isPlaying)
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
    private void TurtorialMoveIn()
    {
        RectTransform tutorialRect = turtorial.GetComponent<RectTransform>();
        tutorialRect.DOAnchorPos(new Vector2(596f, -268f), 1f).SetEase(Ease.InOutQuad);
    }

    private void TurtorialMoveOut()
    {
        RectTransform tutorialRect = turtorial.GetComponent<RectTransform>();
        tutorialRect.DOAnchorPos(new Vector2(1451f, -268f), 1f).SetEase(Ease.InOutQuad);
    }

    IEnumerator StartAnim()
    {
        circle.transform.localScale = Vector3.one * 40;
        circle.transform.DOScale(0, 1f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
        CharactorManager.Instance.RandomizeCharactorCards();
        BoxManager.Instance.RandomizeBoxCards();
        AudioManager.Instance.PlayAudioBackground();
    }

    public void OnMusicButtonClick()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        AudioManager.Instance.audioSource.mute = isMuted;
        if (isMuted)
        {
            foreach(Text txt in musicStateTxt)
            {
                txt.text = "off";
            }
        }
        else
        {
            foreach (Text txt in musicStateTxt)
            {
                txt.text = "on";
            }
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
        isPlaying = false;
        pauseContainer.transform.localScale = Vector3.zero;
        pauseContainer.transform.DOScale(1, 1f).SetEase(Ease.InOutQuad);
    }

    public void ResumeGame()
    {
        isPlaying = true;
    }

    public IEnumerator ActiveLosePanel()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlayAudioFailGame();
        endContainer.transform.localScale = Vector3.zero;
        losePanel.SetActive(true);
        endContainer.transform.DOScale(1, 1f).SetEase(Ease.InOutQuad);
    }

    public void StartCountdown(int duration)
    {
        totalTime = duration;
    }
    public void AddScore(int points)
    {
        score += points;
        foreach (Text txt in scoreTxts)
        {
            txt.text = score.ToString();

        }
    }
    public void UpdateCorrectCount()
    {
        correctCount++;
        AddScore(10);
        if(correctCount == 10)
        {
            correctCount = 0;
            StartCoroutine(AnimCroutine());
        }
    }
    IEnumerator AnimCroutine()
    {
        yield return new WaitForSeconds(0.5f);
        CharactorManager.Instance.StartCoroutine(CharactorManager.Instance.TweenCharScale());
        yield return new WaitForSeconds(1.1f);
        CharactorManager.Instance.RandomizeCharactorCards();
        BoxManager.Instance.RandomizeBoxCards();
    }
    public void NormalGame()
    {
        isPlaying = true;
        
    }
    public void HardModeGame()
    {
        TurtorialMoveIn();
        isPlaying = false;
        containerFill.SetActive(true);
        pauseBtn.SetActive(false);
        StartCoroutine(HardModeCountdown(6f));
    }

    private IEnumerator HardModeCountdown(float duration)
    {
        float elapsedTime = 0f;
        Image fillImage = fill.GetComponent<Image>();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(1 - (elapsedTime / duration));
            fillImage.fillAmount = progress;

            yield return null;
        }
        TurtorialMoveOut();
        BoxManager.Instance.HiddenSpriteBox();
        containerFill.SetActive(false);
        pauseBtn.SetActive(true);
        isPlaying = true;
    }
    private IEnumerator VeryHardModeCountdown(float duration)
    {
        float elapsedTime = 0f;
        Image fillImage = fill.GetComponent<Image>();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(1 - (elapsedTime / duration));
            fillImage.fillAmount = progress;

            yield return null;
        }
        TurtorialMoveOut();
        BoxManager.Instance.HiddenSpriteBox();
        CharactorManager.Instance.HiddenSpriteCharactor();
        containerFill.SetActive(false);
        pauseBtn.SetActive(true);
        isPlaying = true;
    }
    public void VeryHardModeGame()
    {
        TurtorialMoveIn();
        isPlaying = false;
        containerFill.SetActive(true);
        pauseBtn.SetActive(false);
        StartCoroutine(VeryHardModeCountdown(10f));
    }
}
