using System.Collections;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    [SerializeField] private float _disableTime;

    private void OnEnable()
    {
        StartCoroutine(BulletHoleDisable(_disableTime));
    }

    private void OnDisable()
    {
        StopCoroutine(BulletHoleDisable(_disableTime));
    }

    private IEnumerator BulletHoleDisable(float time)
    {
        yield return YieldCache.WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    
}
