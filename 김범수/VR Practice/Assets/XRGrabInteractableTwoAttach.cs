using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Custom XRGrabInteractable 생성
public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    // 왼손의 부착 위치
    public Transform leftAttachTransform;
    // 오른손의 부착 위치
    public Transform rightAttachTransform;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 왼손에 부착해야 한다면 leftAttachTransform 위치에, 오른손에 부착해야 한다면 rightAttachTransform 위치에 부착
        if(args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if(args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
        }
    }
}
