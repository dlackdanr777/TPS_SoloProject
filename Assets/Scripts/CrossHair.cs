using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private RectTransform _rightCrossHair;
    [SerializeField] private RectTransform _leftCrossHair;
    [SerializeField] private RectTransform _upCrossHair;
    [SerializeField] private RectTransform _downCrossHair;



    public void CrossHairAeraSize(float fireAccuracy)
    {
        float CrossHairDistance = 0.35f + (fireAccuracy * 2.5f);
        _rightCrossHair.anchoredPosition = new Vector2(CrossHairDistance, 0);
        _leftCrossHair.anchoredPosition = new Vector2(-CrossHairDistance, 0);
        _upCrossHair.anchoredPosition = new Vector2(0, CrossHairDistance);
        _downCrossHair.anchoredPosition = new Vector2(0, -CrossHairDistance);
    }
}
