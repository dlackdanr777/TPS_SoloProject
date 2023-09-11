using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private RectTransform _rightCrossHair;
    [SerializeField] private RectTransform _leftCrossHair;
    [SerializeField] private RectTransform _upCrossHair;
    [SerializeField] private RectTransform _downCrossHair;

    [Space(10f)]
    [SerializeField] private Image[] _sideCrossHair;

    private Coroutine _SideCrossHairEnableRoutine;

    private void Start()
    {
        GameManager.Instance.Player.OnTargetDamaged += SideCrossHairEnable;
    }

    public void OnDisable()
    {
        for (int i = 0; i < _sideCrossHair.Length; i++)
        {
            Color color = new Color(0, 1, 0, 0);
            _sideCrossHair[i].color = color;
        }
    }

    public void CrossHairAeraSize(float fireAccuracy)
    {
        float CrossHairDistance = 0.35f + (fireAccuracy * 2.5f);
        _rightCrossHair.anchoredPosition = new Vector2(CrossHairDistance, 0);
        _leftCrossHair.anchoredPosition = new Vector2(-CrossHairDistance, 0);
        _upCrossHair.anchoredPosition = new Vector2(0, CrossHairDistance);
        _downCrossHair.anchoredPosition = new Vector2(0, -CrossHairDistance);
    }


    public void SideCrossHairEnable()
    {
        if (_SideCrossHairEnableRoutine != null)
            StopCoroutine(_SideCrossHairEnableRoutine);

        _SideCrossHairEnableRoutine = StartCoroutine(IESideCrossHairEnable());
    }

    IEnumerator IESideCrossHairEnable()
    {
        for (int i = 0; i < _sideCrossHair.Length; i++)
        {
            Color color = new Color(0, 1, 0, 1);
            _sideCrossHair[i].color = color;
        }
        yield return YieldCache.WaitForSeconds(0.1f);

        float alpha = 1;
        while(alpha > 0)
        {
            alpha -= 0.2f;
            for (int i = 0; i < _sideCrossHair.Length; i++)
            {
                Color color = new Color(0, 1, 0, alpha);
                _sideCrossHair[i].color = color;
            }
            yield return YieldCache.WaitForSeconds(0.02f);
        }


    } 
}
