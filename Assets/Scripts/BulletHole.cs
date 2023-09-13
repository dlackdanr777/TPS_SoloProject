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

    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private ParticleSystem _bloodCloudParticle;

    private void OnEnable()
    {
        StartCoroutine(BulletHoleDisable(_disableTime));
        StartCoroutine(TargetHit());
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

    private IEnumerator TargetHit()
    {
        yield return YieldCache.WaitForSeconds(0.01f);

        if (transform.parent.tag == "HitPart" || transform.parent.tag == "Enemy")
        {
            _AudioSource.PlayOneShot(_hitPartClip);
            PlayParticle(2);
            yield return YieldCache.WaitForSeconds(0.2f);
            PlayParticle(1);
        }
            
        else if (transform.parent.tag == "Ground")
            _AudioSource.PlayOneShot(_groundClip);

        Debug.Log(transform.parent.tag);
    }

    private void PlayParticle(int amount)
    {
        _bloodParticle.Emit(amount);
        _bloodCloudParticle.Emit(amount);
    }
}
