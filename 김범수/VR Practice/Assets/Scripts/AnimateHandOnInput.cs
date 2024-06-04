using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    // VR기기의 입력을 받는다. 여기서는 오른쪽 컨트롤러의 트리거 버튼의 입력을 받는다.
    public InputActionProperty pinchAnimationAction;

    // 오른쪽 컨트롤러의 그립 버튼의 입력을 받는다.
    public InputActionProperty gripAnimationAction;

    // 손 애니메이터
    public Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 입력값 추출
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        // 입력값을 토대로 애니메이션 반영
        handAnimator.SetFloat("Trigger", triggerValue);
        
        // 입력값 추출
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        // 입력값을 토대로 애니메이션 반영
        handAnimator.SetFloat("Grip", gripValue);
    }
}
