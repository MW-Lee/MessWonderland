////////////////////////////////////////////////
//
// FPCamera
//
// 고글을 사용했을 때 1인칭 시점으로 바꿔주는 스크립트
// 
// 20. 04. 02
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FPCamera : MonoBehaviour
{
    Transform TF;
    public Transform TFCamdolph;

    public GameObject gToolTip;
    public GameObject gGoggleTip;

    public float camSpeed = 1.0f; // 화면이 움직이는 속도 변수

    private float yaw = 0.0f; // 
    private float pitch = 0.0f;

    private bool bColor;

    private void Start()
    {
        TF = transform;
        bColor = false;

        TF.position = GetComponent<FollowCam>().target.position + new Vector3(0, 1.65f, 0);
        TF.rotation = GetComponent<FollowCam>().target.rotation;
    }

    private void OnEnable()
    {
        gGoggleTip.SetActive(true);
        gToolTip.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Cursor.visible)
        {
            yaw += camSpeed * Input.GetAxis("Mouse X"); // 마우스X값을 지속적으로 받을 변수
            pitch += camSpeed * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수

            // Mathf.Clamp(x, 최소값, 최댓값) - x값을 최소,최대값 사이에서만 변하게 해줌
            //yaw = Mathf.Clamp(yaw, -150f, -30f); // yaw값을 한정시켜줌
            pitch = Mathf.Clamp(pitch, -30f, 10f); // pitch값을 한정시켜줌

            TF.position = GetComponent<FollowCam>().target.position + new Vector3(0, 1.65f, 0);
            TF.eulerAngles = new Vector3(-pitch, yaw, 0.0f); // 앵글각에 만들어놓은 값을 넣어줌

            //if (Input.GetKeyUp(KeyCode.L))
            //{
            //    bShowMouse ^= true;
            //    Cursor.visible = bShowMouse;
            //    PlayerController.bShowMouse = bShowMouse;
            //    Cursor.lockState = bShowMouse ? CursorLockMode.None : CursorLockMode.Locked;
            //}
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            bColor ^= true;
            for (int i = 0; i < TFCamdolph.childCount; i++)
                TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(bColor, true);
        }
    }

    private void OnDisable()
    {
        gGoggleTip.SetActive(false);
        gToolTip.SetActive(true);
    }
}
