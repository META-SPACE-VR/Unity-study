using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

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

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    // Update is called once per frame
    void Update()
    {
        // hover 상태가 아니고 select 입력이 발생하지 않으면서 activate 입력이 일정 수준 이상 들어오면 오브젝트 활성화
        // UI랑 상호작용 하거나 총을 쥐고 있는 상태에서는 텔레포트 오브젝트를 활성화 시키지 않게 하기 위함
        bool isLeftRayHovering = leftRay ? leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid) : false;
        leftTeleportation.SetActive(!isLeftRayHovering && leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);

        bool isRightRayHovering = rightRay ? rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid) : false;
        rightTeleportation.SetActive(!isRightRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
