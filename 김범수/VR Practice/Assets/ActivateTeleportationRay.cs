using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    // 텔레포트관련 오브젝트
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    // Activate Input Parameter
    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    // Cancel Input Parameter
    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;

    // Update is called once per frame
    void Update()
    {
        // select 입력이 발생하지 않으면서 activate 입력이 일정 수준 이상 들어오면 오브젝트 활성화
        // 총을 쥐고 있는 상태에서는 텔레포트 오브젝트를 활성화 시키지 않게 하기 위함
        leftTeleportation.SetActive(leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleportation.SetActive(rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
