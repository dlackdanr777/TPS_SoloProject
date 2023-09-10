using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider))]
public class BulletHole : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _hitPartClip;
    [SerializeField] private AudioClip _groundClip;
    [SerializeField] private float _disableTime;

    private void OnEnable()
    {
        StartCoroutine(BulletHoleDisable(_disableTime));
        _boxCollider.enabled = true;

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

    private void SoundEffect(Collider other)
    {
        if (other.tag == "HitPart" || other.tag == "Enemy")
            _AudioSource.PlayOneShot(_hitPartClip);

        else if (other.tag == "Ground")
            _AudioSource.PlayOneShot(_groundClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundEffect(other);
        _boxCollider.enabled = false;

        Debug.Log("ÃÑ¾Ë¼Ò¸®");
    }
}
