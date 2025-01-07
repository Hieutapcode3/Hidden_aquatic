using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyb.Utils;
public class CharactorManager : ManualSingletonMono<CharactorManager>
{
    [SerializeField] private List<GameObject> charactorCards;
    [SerializeField] private List<Sprite> charactors;
    [SerializeField] private Sprite hiddenCharactor;
    [SerializeField] private GameObject starEffect;
    public void RandomizeCharactorCards()
    {
        if (charactorCards.Count != 10 || charactors.Count != 10)
        {
            Debug.LogError("Ensure there are exactly 10 charactorCards and 10 charactors.");
            return;
        }
        List<int> ids = new List<int>();
        for (int i = 1; i <= 10; i++)
        {
            ids.Add(i);
        }
        for (int i = 0; i < ids.Count; i++)
        {
            int randomIndex = Random.Range(0, ids.Count);
            int temp = ids[i];
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }
        for (int i = 0; i < charactorCards.Count; i++)
        {
            CharactorCard charactorCard = charactorCards[i].GetComponent<CharactorCard>();
            if (charactorCard != null)
            {
                charactorCard.TweenAnim(); ;
                int assignedId = ids[i];
                charactorCard.charactorID = (CharactorID)assignedId;
                SpriteRenderer spriteRenderer = charactorCards[i].GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = charactors[assignedId - 1];
                }
                else
                {
                    Debug.LogError("No SpriteRenderer found in children of " + charactorCards[i].name);
                }
                charactorCard.SetSprite();
            }
            else
            {
                Debug.LogError("CharactorCard script not found on " + charactorCards[i].name);
            }
        }
        if(PlayerPrefs.GetInt("GameMode") == 0) GameManager.Instance.NormalGame();
        else if (PlayerPrefs.GetInt("GameMode") == 1) GameManager.Instance.HardModeGame();
        else if(PlayerPrefs.GetInt("GameMode") == 2) GameManager.Instance.VeryHardModeGame();
    }
    public IEnumerator TweenCharScale()
    {
        foreach(GameObject obj in charactorCards)
        {
            if (obj != null)
            {
                obj.GetComponent<CharactorCard>().TweenScale();
                yield return new WaitForSeconds(0.1f);
                Instantiate(starEffect, obj.transform.position,Quaternion.identity);
                AudioManager.Instance.PlayAudioStart();
            }
        }
    }
    public void HiddenSpriteCharactor()
    {
        foreach (var item in charactorCards)
        {
            item.GetComponent<SpriteRenderer>().sprite = hiddenCharactor;
        }
        foreach (var item in charactorCards)
        {
            item.GetComponent<CharactorCard>().TweenAnim();
        }
    }
}
