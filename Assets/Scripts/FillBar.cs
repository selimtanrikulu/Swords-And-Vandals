using DG.Tweening;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    [SerializeField] private GameObject fillBar;
    [SerializeField] private GameObject gradientBar;
    
    
    [SerializeField] private float changeDuration;
    
    
    public void UpdateBar(float currentValue, float maxValue)
    {
        float endValue = currentValue / maxValue;
        
        Vector3 localScale = fillBar.transform.localScale;
        localScale.y = endValue + 0.01f;
        fillBar.transform.localScale = localScale;
        
        gradientBar.transform.DOKill();
        gradientBar.transform.DOScaleY(endValue-0.01f,changeDuration);
    }
}
