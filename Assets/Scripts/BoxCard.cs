using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxCard : MonoBehaviour
{
    public CharactorID boxId;
    private Transform child;
    private void Awake()
    {
        child = gameObject.transform.GetChild(0);
    }
    void Start()
    {
        TweenAnim();
    }
    public void TweenAnim()
    {
        StartCoroutine(StartAnim());
    }
    private IEnumerator StartAnim()
    {
        child.gameObject.SetActive(true);
        child.localScale = Vector3.zero;
        child.DOScale(1.2f, 1f).SetEase(Ease.InOutQuad);
        child.DORotate(new Vector3(0, 0, -360f), 1f, RotateMode.FastBeyond360)
    .SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1f);
    }
    public void UnActiveChild()
    {
        StartCoroutine(TurnOffChildCroutine());
    }
    private IEnumerator TurnOffChildCroutine()
    {
        yield return new WaitForSeconds(0.5f);
        child.gameObject.SetActive(false);
    }
}
