using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public bool IsReload => _isReload;

    public event Action<IHp, float> OnTargetDamageHendler;
    public event Action OnFireHendler;
    public event Action OnReloadHendler;

    public Gun CurrentGun; //현재 들고있는 총

    [SerializeField] private CrossHair _crossHair;

    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private Camera _mainCamera;

    [SerializeField] private LayerMask _hitLayerMask;

    private AudioSource _audioSource;

    private float _currentFireRate; // 이값이 0이어야 총 발사 가능

    private bool _isReload = false;

    private float _nowRecoil; //현재 반동

    private float _recoilMul; //반동 배수


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _nowRecoil = CurrentGun.MinRecoil;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameEnd)
        {
            FireStabilization();
            GunFireRateCalc();
            TryReload();
        }
    }

    private void GunFireRateCalc()  
    {
        if(_currentFireRate > 0)
        {
            _currentFireRate -= Time.deltaTime;
        }
    }

    public void TryFire() //발사 입력을 받는 함수
    {
        if (Input.GetButton("Fire1") && _currentFireRate <= 0 && !_isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if(!_isReload)
        {
            if(CurrentGun.CurrentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(ReloadRoutine());
            }
        }
    }
    Vector3 fireDirection;
    public void Shoot() //총을 쏘는 함수
    {
        _currentFireRate = CurrentGun.FireRate;
        CurrentGun.CurrentBulletCount--; //탄약 감소
        PlaySound(CurrentGun.FireSound);
        CurrentGun.MuzzleFlash.Play();
        _playerAnimator.SetTrigger("Fire");

        float xError = GetRandomNormalDistribution(0f, _nowRecoil);
        float yError = GetRandomNormalDistribution(0f, _nowRecoil);
        fireDirection = _crossHair.transform.position - CurrentGun.MuzzleFlash.transform.position;

        float targetDistance = Vector3.Distance(CurrentGun.MuzzleFlash.transform.position, _crossHair.transform.position);
        float distanceScale = targetDistance / CurrentGun.Range;

        fireDirection = Quaternion.AngleAxis(yError * distanceScale, Vector3.up) * fireDirection;
        fireDirection = Quaternion.AngleAxis(xError * distanceScale, Vector3.right) * fireDirection;

        if(_nowRecoil < CurrentGun.MaxRecoil)
        {
            _nowRecoil += CurrentGun.Recoil;
            if (_nowRecoil > CurrentGun.MaxRecoil)
                _nowRecoil = CurrentGun.MaxRecoil;
        }

        RaycastHit hit;
        Ray ray = new Ray(CurrentGun.MuzzleFlash.transform.position, fireDirection);
        float distance = CurrentGun.Range;

        OnFireHendler?.Invoke();

        if (Physics.Raycast(ray, out hit, distance, _hitLayerMask))
        {
            Debug.Log("맞춤");
            Quaternion bulletHoleRotation = Quaternion.LookRotation(ray.direction);
            GameObject bulletHole = ObjectPoolManager.Instance.SpawnBulletHole(hit.point, bulletHoleRotation);
            bulletHole.transform.parent = hit.transform;
            if (hit.transform.GetComponent<IHp>() != null)
               OnTargetDamageHendler?.Invoke(hit.transform.GetComponent<IHp>(), CurrentGun.Damage);
        }
    }


    public void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReload &&CurrentGun.CurrentBulletCount < CurrentGun.ReloadBulletCount)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    public float GetRandomNormalDistribution(float mean, float standard)
    {
        var x1 = Random.Range(0, 1f);
        var x2 = Random.Range(0, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }

    private IEnumerator ReloadRoutine()
    {
        if(CurrentGun.CarryBulletCount > 0)
        {
            Debug.Log("재장전중");
            _playerAnimator.SetTrigger("Reload");
            _isReload = true;
            CurrentGun.CarryBulletCount += CurrentGun.CurrentBulletCount;
            CurrentGun.CurrentBulletCount = 0;

            yield return new WaitForSeconds(CurrentGun.ReloadTime);

            if(CurrentGun.CarryBulletCount >= CurrentGun.ReloadBulletCount)
            {
                CurrentGun.CurrentBulletCount = CurrentGun.ReloadBulletCount;
                CurrentGun.CarryBulletCount -= CurrentGun.ReloadBulletCount;
            }
            else
            {
                CurrentGun.CurrentBulletCount = CurrentGun.CarryBulletCount;
                CurrentGun.CarryBulletCount = 0;
            }
            _isReload = false;
            OnReloadHendler?.Invoke();
        }
        else
        {
            Debug.Log("보유한 총알수량이 부족합니다.");
        }
    }


    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.PlayOneShot(clip);
    }

    private void FireStabilization() //반동을 회복시키는 함수
    {
        if (_nowRecoil > CurrentGun.MinRecoil * _recoilMul)
            _nowRecoil -= CurrentGun.RecoilRecoveryAmount * Time.deltaTime;

        if (_nowRecoil < CurrentGun.MinRecoil * _recoilMul)
            _nowRecoil = CurrentGun.MinRecoil * _recoilMul;
    }

    public void FollowCrossHair()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //카메라 정중앙에 레이를 위치시킨다
        float distance = CurrentGun.Range;
        Debug.DrawRay(CurrentGun.MuzzleFlash.transform.position, CurrentGun.MuzzleFlash.transform.forward * distance, Color.red);
        if (Physics.Raycast(ray, out hit, distance, _hitLayerMask))
        {
            Vector3 hitPos = hit.point;
            float hitDistance = Vector3.Distance(_mainCamera.transform.position, hit.point);
            _crossHair.transform.position = hitPos;
            float distanceScale = hitDistance / distance;
            _crossHair.transform.localScale = Vector3.one * distanceScale;

            _crossHair.CrossHairAeraSize(_nowRecoil * distanceScale);
            _crossHair.transform.LookAt(_mainCamera.transform.position);
        }
        else
        {
            _crossHair.transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * distance;
            _crossHair.transform.localScale = Vector3.one;

            _crossHair.CrossHairAeraSize(_nowRecoil);
            _crossHair.transform.LookAt(_mainCamera.transform.position);
        }
    }

    public void EnableCrossHair() //크로스헤어를 활성화시키는 함수
    {
        _crossHair.Visibility();

       
    }

    public void DisableCrossHair() //크로스헤어를 비활성화 시키는 함수
    {
        _crossHair.Hidden();
    }

    public void SetRecoilMul(float value)
    {
        _recoilMul = value;
    }
}
