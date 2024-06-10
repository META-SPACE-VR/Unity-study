using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
    public float posetransitionDuration = 0.2f;
    // 총을 쥘 때의 오른손 포즈
    public HandData rightHandPose;
    public HandData leftHandPose;

    // 쥐기 전/후의 손의 상대적 위치, 회전, 관절의 회전
    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        if(args.interactorObject is XRDirectInteractor)
        {
            // 상호작용하는 interactorObject(오른손)을 얻은 후 애니메이터 비 활성화
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if(handData.handType == HandData.HandModelType.Right)
            {
                // 쥐기 전과 후의 HandData을 저장한 뒤 rightHandPose에 저장된 손 모양으로 바꿔준다.
                SetHandDataValues(handData, rightHandPose);
            }
            else
            {
                SetHandDataValues(handData, leftHandPose);
            }

            
            StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));
        }
    }

    public void UnSetPose(BaseInteractionEventArgs args)
    {
        if(args.interactorObject is XRDirectInteractor)
        {
            // 상호작용하는 interactorObject(오른손)을 얻은 후 애니메이터 활성화
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            // 쥐기 전 손 모양으로 변경
            StartCoroutine(SetHandDataRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));
        }
    }

    // 쥐기 전과 후의 HandData의 값을 보정을 거쳐서 변수에 저장
    public void SetHandDataValues(HandData h1, HandData h2)
    {
        // 쥐기 전에는 원래 부모 오브젝트를 기준으로, 쥔 후에는 Attach Point를 기준으로 위치가 정해짐 (아마도?)
        // 위치 저장 (단, 지정한 스케일에 따라 같은 Vector3값이어도 실제 적용되는 위치가 다르므로 보정해준다.)
        startingHandPosition = new Vector3(
            h1.root.localPosition.x / h1.root.localScale.x,
            h1.root.localPosition.y / h1.root.localScale.y,
            h1.root.localPosition.z / h1.root.localScale.z
        );
        finalHandPosition = new Vector3(
            h2.root.localPosition.x / h2.root.localScale.x,
            h2.root.localPosition.y / h2.root.localScale.y,
            h2.root.localPosition.z / h2.root.localScale.z
        );

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for(int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    // HandData에 새로운 값 반영
    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for(int i = 0; i < h.fingerBones.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0;

        while(timer < posetransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer/posetransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer/posetransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for(int i = 0; i < h.fingerBones.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer/posetransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR

    [MenuItem("Tools/Mirror Selected Right Grab Pose")]
    public static void MirrorRightPose()
    {
        Debug.Log("MIRROR RIGHT POSE");
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
    }

#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        for(int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
        {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}
