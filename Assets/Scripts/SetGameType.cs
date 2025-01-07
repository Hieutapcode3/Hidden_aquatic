using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameType : MonoBehaviour
{
    private int gameMode;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject circle;
    [SerializeField] private List<GameObject> btns;
    private void Start()
    {
        circle.transform.localScale = Vector3.zero;
        gameMode = PlayerPrefs.GetInt("GameMode", 0);
        AudioManager.Instance.PlayAudioBackground();
    }

    public void NormalGame()
    {
        gameMode = 0;
        SaveGameMode();
        StartCoroutine(TransitionAnim());
    }

    public void HardGame()
    {
        gameMode = 1;
        SaveGameMode();
        StartCoroutine(TransitionAnim());

    }

    public void VeryHardGame()
    {
        gameMode = 2;
        SaveGameMode();
        StartCoroutine(TransitionAnim());
    }

    public int GetGameMode()
    {
        return gameMode;
    }

    private void SaveGameMode()
    {
        PlayerPrefs.SetInt("GameMode", gameMode);
        PlayerPrefs.Save(); 
    }
    IEnumerator TransitionAnim()
    {
        foreach (var item in btns)
        {
            item.SetActive(false);
        }
        title.transform.localEulerAngles = new Vector3(0,0,0);
        title.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
        title.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad);
        title.transform.DOScale(0, 1f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1f);
        circle.transform.DOScale(40, 1f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1f);
        LoadNextScene() ;
    }
    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}