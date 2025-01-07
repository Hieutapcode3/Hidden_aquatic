using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyb.Utils;
public class BoxManager : ManualSingletonMono<BoxManager>
{
    [SerializeField] private List<GameObject> boxCards;
    [SerializeField] private List<Sprite> charactors;
    [SerializeField] private Sprite hiddenBox;
    public void RandomizeBoxCards()
    {
        if (boxCards.Count != 10 || charactors.Count != 10)
        {
            Debug.LogError("Ensure there are exactly 10 boxCards and 10 charactors.");
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
        for (int i = 0; i < boxCards.Count; i++)
        {
            BoxCard boxCard = boxCards[i].GetComponent<BoxCard>();
            if (boxCard != null)
            {
                boxCard.TweenAnim();
                int assignedId = ids[i];
                boxCard.boxId = (CharactorID)assignedId;
                GameObject child = boxCards[i].transform.GetChild(0).gameObject;
                SpriteRenderer spriteRenderer = child.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = charactors[assignedId - 1];
                    spriteRenderer.color = Color.black;
                }
                else
                {
                    Debug.LogError("No SpriteRenderer found in children of " + boxCards[i].name);
                }
            }
            else
            {
                Debug.LogError("BoxCard script not found on " + boxCards[i].name);
            }
        }
    }
    public void HiddenSpriteBox()
    {
        foreach(GameObject box in boxCards)
        {
            GameObject child = box.transform.GetChild(0).gameObject;
            child.GetComponent<SpriteRenderer>().sprite = hiddenBox;
            child.GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (GameObject box in boxCards)
        {
            box.GetComponent<BoxCard>().TweenAnim();
        }
    }
}
