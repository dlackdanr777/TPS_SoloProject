using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonHandler<UIManager>
{
    [SerializeField] private TextMeshProUGUI _centerText;
    private Coroutine ShowCenterTextRoutine;

    public void Start()
    {
        _centerText.alpha = 0;
    }

    public void ShowCenterText(string textContent)
    {
        if(ShowCenterTextRoutine != null)
            StopCoroutine(ShowCenterTextRoutine);

        StartCoroutine(ShowText(_centerText, textContent));
    }

    private IEnumerator ShowText(TextMeshProUGUI tmpText, string textContent)
    {
        tmpText.alpha = 1.0f;
        tmpText.text = textContent;

        yield return YieldCache.WaitForSeconds(1);

        while (tmpText.alpha > 0)
        {
            tmpText.alpha -= 0.02f;
            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }
}
