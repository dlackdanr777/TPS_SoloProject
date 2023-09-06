using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rigging : MonoBehaviour
{
    [SerializeField] private Transform _rigTarget; //뼈대들이 바라볼 타겟
    [Header("애니메이션 리깅")]
    [SerializeField] private MultiAimConstraint SpineAim1;
    [SerializeField] private MultiAimConstraint SpineAim2;
    [SerializeField] private MultiAimConstraint SpineAim3;
    [SerializeField] private MultiAimConstraint HeadAim;


    public void SetUpperRigWeight(float weight) //목표를 추적하는 뼈대의 가중치를 설정하는 함수
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
