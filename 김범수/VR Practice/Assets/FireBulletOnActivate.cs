using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject bullet; // 총알
    public Transform spawnPoint; // 총알 스폰 위치
    public float fireSpeed = 20f; // 발사 속도

    // Start is called before the first frame update
    void Start()
    {
        // XRGrabInteractable 오브젝트에 activated 상태로 전환 시 발생시킬 콜백함수 등록
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 총알 발사
    void FireBullet(ActivateEventArgs args)
    {
        // 스폰 포인트에 총알 스폰
        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        // 지정된 속도로 총알 발사
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        // 5초 후 파괴
        Destroy(spawnedBullet, 5);
    }
}
