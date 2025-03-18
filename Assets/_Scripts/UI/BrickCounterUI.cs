using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
public class BrickCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI toUpdate;
    [SerializeField] private Transform coinTextContainer;
    [SerializeField] private float tweenDuration =0.025f;
    [SerializeField] private float totalAnimationTime = 0.04f;
    [SerializeField] private Ease animationCurve = Ease.OutQuad;

    private float containerInitPosition;
    private float moveAmount;
    void Start()
    {
        Canvas.ForceUpdateCanvases();
        current.SetText("0");
        toUpdate.SetText("0");
        containerInitPosition = coinTextContainer.localPosition.y;
        moveAmount = current.rectTransform.rect.height;
    }
    public void UpdateScore(int score)
    {
        toUpdate.SetText(score.ToString());
        
        // Immediately kill any ongoing animations to prevent stuttering
        coinTextContainer.DOKill();
        
        coinTextContainer
            .DOLocalMoveY(containerInitPosition + moveAmount, tweenDuration)
            .SetEase(animationCurve);
            
        StartCoroutine(ResetCoinContainer(score, totalAnimationTime));
    }
    private IEnumerator ResetCoinContainer(int score, float delay)
    {
        yield return new WaitForSeconds(delay);
        current.SetText(score.ToString());
        
        Vector3 localPosition = coinTextContainer.localPosition;
        coinTextContainer.localPosition = new Vector3(
            localPosition.x,
            containerInitPosition,
            localPosition.z
        );
    }
}