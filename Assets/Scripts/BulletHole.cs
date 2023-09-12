using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletHole : MonoBehaviour
{
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _hitPartClip;
    [SerializeField] private AudioClip _groundClip;
    [SerializeField] private float _disableTime;

    private void OnEnable()
    {
        StartCoroutine(BulletHoleDisable(_disableTime));
        StartCoroutine(PlaySoundEffect());
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        StopCoroutine(BulletHoleDisable(_disableTime));
    }

    private IEnumerator BulletHoleDisable(float time)
    {
        yield return YieldCache.WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private IEnumerator PlaySoundEffect()
    {
        yield return YieldCache.WaitForSeconds(0.01f);

        if (transform.parent.tag == "HitPart" || transform.parent.tag == "Enemy")
            _AudioSource.PlayOneShot(_hitPartClip);

        else if (transform.parent.tag == "Ground")
            _AudioSource.PlayOneShot(_groundClip);

        Debug.Log(transform.parent.tag);
    }
}
