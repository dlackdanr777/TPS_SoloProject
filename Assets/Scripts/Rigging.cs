using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rigging : MonoBehaviour
{
    [Header("애니메이션 리깅")]
    [SerializeField] private MultiAimConstraint _spineAim1;
    [SerializeField] private MultiAimConstraint _spineAim2;
    [SerializeField] private MultiAimConstraint _headAim;
    [SerializeField] private MultiAimConstraint _headCamera;
    [SerializeField] private MultiAimConstraint _handAim;
    [SerializeField] private TwoBoneIKConstraint _secondHandGrabWeapon;



    public void SetUpperRigWeight(float weight) //목표를 추적하는 뼈대의 가중치를 설정하는 함수
    {
        if (_spineAim1.weight != weight)
        {
            float weightLerp = Mathf.Lerp(_spineAim1.weight, weight, Time.deltaTime * 10);
            _spineAim1.weight = weightLerp;
            _spineAim2.weight = weightLerp;
            _headAim.weight = weightLerp;
            _handAim.weight = weightLerp;
            _secondHandGrabWeapon.weight = weightLerp;
            _headCamera.weight = Mathf.Abs(weightLerp - 1);
        }
    }

    public void SetHeadRigWeight(float weight)
    {
        if (_headAim.weight != weight)
        {
            float weightLerp = Mathf.Lerp(_headAim.weight, weight, Time.deltaTime * 10);
            _headCamera.weight = weightLerp;
            _headAim.weight = Mathf.Abs(weightLerp - 1);
        }
    }
}
