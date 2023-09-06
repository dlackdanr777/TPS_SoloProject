using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rigging : MonoBehaviour
{
    [SerializeField] private Transform _rigTarget; //������� �ٶ� Ÿ��
    [Header("�ִϸ��̼� ����")]
    [SerializeField] private MultiAimConstraint SpineAim1;
    [SerializeField] private MultiAimConstraint SpineAim2;
    [SerializeField] private MultiAimConstraint SpineAim3;
    [SerializeField] private MultiAimConstraint HeadAim;


    public void SetUpperRigWeight(float weight) //��ǥ�� �����ϴ� ������ ����ġ�� �����ϴ� �Լ�
    {
        if (SpineAim1.weight != weight)
        {
            float weightLerp = Mathf.Lerp(SpineAim1.weight, weight, Time.deltaTime * 10);
            SpineAim1.weight = weightLerp;
            SpineAim2.weight = weightLerp;
            SpineAim3.weight = weightLerp;
            HeadAim.weight = weightLerp;
        }
    }
}
