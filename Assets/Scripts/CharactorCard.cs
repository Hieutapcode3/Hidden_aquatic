using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharactorID
{
    Charactor1 = 1,
    Charactor2 = 2,
    Charactor3 = 3,
    Charactor4 = 4,
    Charactor5 = 5,
    Charactor6 = 6,
    Charactor7 = 7,
    Charactor8 = 8,
    Charactor9 = 9,
    Charactor10 = 10
}

public class CharactorCard : MonoBehaviour
{
    public CharactorID charactorID;
    private bool isDragging = false;
    private Vector3 startPosition;
    private Camera mainCamera;
    private bool shouldReturnToStart = false;
    private Transform targetBox;
    private bool canDrag;
    private Sprite spriteOrigin;
    private void Awake()
    {
        startPosition = transform.position;
    }
    void Start()
    {
        mainCamera = Camera.main;
        TweenAnim();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, startPosition.z);
        }

        if (shouldReturnToStart)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                transform.position = startPosition;
                shouldReturnToStart = false;
            }
        }
        if (targetBox != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetBox.position, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.position, targetBox.position) < 0.01f)
            {
                transform.position = targetBox.position;
                targetBox = null;
            }
        }
    }
    public void TweenAnim()
    {
        StartCoroutine(StartAnim());
    }
    private IEnumerator StartAnim()
    {
        canDrag = false;
        transform.position = startPosition;
        transform.localScale = Vector3.zero;
        transform.DOScale(0.7f, 1f).SetEase(Ease.InOutQuad);
        transform.DORotate(new Vector3(0, 0, -360f), 1f, RotateMode.FastBeyond360)
    .SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1f);
        canDrag = true;
    }
    public void TweenScale()
    {
        transform.DOScale(0, 0.1f).SetEase(Ease.InOutQuad);
    }
    private void OnMouseDown()
    {
        if (!canDrag || !GameManager.Instance.isPlaying) return;
        isDragging = true;
        shouldReturnToStart = false;
        AudioManager.Instance.PlayAudioCollect();
    }

    private void OnMouseUp()
    {
        if(!canDrag || !GameManager.Instance.isPlaying) return;
        isDragging = false;
        CheckCollisionWithBox();
    }

    private void CheckCollisionWithBox()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            BoxCard box = collider.GetComponent<BoxCard>();
            if (box != null)
            {
                if (box.boxId == charactorID)
                {
                    GameManager.Instance.UpdateCorrectCount();
                    AudioManager.Instance.PlayAudioCollect();
                    canDrag = false;
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteOrigin;
                    targetBox = box.transform;
                    transform.DOScale(1.3f,0.5f).SetEase(Ease.InOutQuad);
                    box.UnActiveChild();
                    return;
                }
            }
        }
        shouldReturnToStart = true;
    }
    public void SetSprite()
    {
        spriteOrigin = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

}
