using UnityEngine;
using UnityEngine.Animations.Rigging;


/// <summary> ��ü�� �ִϸ��̼� ������ �����ϴ� �Լ� </summary>
public class Rigging : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MultiAimConstraint _spineAim1;
    [SerializeField] private MultiAimConstraint _spineAim2;
    [SerializeField] private MultiAimConstraint _headAim;
    [SerializeField] private MultiAimConstraint _headCamera;
    [SerializeField] private MultiAimConstraint _handAim;
    [SerializeField] private TwoBoneIKConstraint _secondHandGrabWeapon;


    /// <summary> ��ǥ�� �����ϴ� ������ ����ġ�� �����ϴ� �Լ� </summary>
    public void SetUpperRigWeight(float weight)
    {
        if (_spineAim1.weight == weight)
            return;

        float weightLerp = Mathf.Lerp(_spineAim1.weight, weight, Time.deltaTime * 10);
        _spineAim1.weight = weightLerp;
        _spineAim2.weight = weightLerp;
        _headAim.weight = weightLerp;
        _handAim.weight = weightLerp;
        _secondHandGrabWeapon.weight = weightLerp;
        _headCamera.weight = Mathf.Abs(weightLerp - 1);

    }


    /// <summary> ��ǥ�� �����ϴ� ������ ����ġ�� �Ӹ��� �����ϴ� �Լ� </summary>
    public void SetHeadRigWeight(float weight)
    {
        if (_headAim.weight == weight)
            return;

        float weightLerp = Mathf.Lerp(_headAim.weight, weight, Time.deltaTime * 10);
        _headCamera.weight = weightLerp;
        _headAim.weight = Mathf.Abs(weightLerp - 1);
    }
}
