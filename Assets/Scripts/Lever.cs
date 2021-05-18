////////////////////////////////////////////////
//
// Lever
//
// Activate in Turning lever
// 
// 20. 06. 14
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    #region 변수

    public GameObject gCamera;
    public Transform TFPillar;
    public Transform TFPlayer;
    public Transform TFDisPlayer;

    public bool bIsUser = false;

    private Animator _ani;

    private AsyncClient _async;

    #endregion


    #region 함수

    /// <summary>
    /// Rotate the pillar what connect with lever
    /// </summary>
    /// <param name="Isup">true : straight / false : back</param>
    public void Turning(bool Isup)
    {
        if (Isup)
        {
            TFPillar.Rotate(Vector3.up);
        }
        else
        {
            TFPillar.Rotate(-Vector3.up);
        }
    }

    public void ActiveLever(int input)
    {
        switch(input % 4)
        {
            // Go Straight
            case 0:
                Turning(true);
                break;

            // Go Back
            case 1:
                Turning(false);
                break;

            // Off lever
            case 2:
                GetComponent<Lever>().enabled = false;
                break;
        }
    }

    #endregion


    #region 실행

    private void Awake()
    {
        _async = AsyncClient.instance;
    }

    private void OnEnable()
    {
        if (bIsUser)
        {
            PlayerController.bDialog = true;
            Camera.main.GetComponent<FollowCam>().bDialog = true;
            TFPlayer.GetChild(TFPlayer.childCount - 1).SetParent(transform);

            //byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(
            //    5 * int.Parse(transform.parent.parent.parent.name), DontDestroy.instance.iRoomNum)
            //    );
            //_async.Send(data, Constant.NOTICE_PUZZLE);

            gCamera.SetActive(true);
        }
        else
        {
            TFDisPlayer.GetChild(TFDisPlayer.childCount - 1).SetParent(transform);            
        }

        transform.GetChild(0).localPosition = new Vector3(0.67f, 0.3f, -0.8f);
        //transform.GetChild(0).localRotation = new Quaternion(
        //    -60,
        //    -90,
        //    -90,
        //    transform.localRotation.w
        //    );
        //transform.rotation = Quaternion.LookRotation(point);
        transform.GetChild(0).rotation = Quaternion.identity;
        transform.GetChild(0).Rotate(Vector3.up, -135);
        _ani = transform.GetComponentInChildren<Animator>();
        _ani.SetBool("Lever", true);        
    }

    private void FixedUpdate()
    {
        if (bIsUser)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                _ani.SetBool("Move", true);

            if (Input.GetKey(KeyCode.W))
            {
                transform.Rotate(Vector3.forward, Space.Self);
                Turning(true);

                byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(
                4 * int.Parse(transform.parent.parent.name), DontDestroy.instance.iRoomNum)
                );
                _async.Send(data, Constant.NOTICE_PUZZLE);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Rotate(-Vector3.forward, Space.Self);
                Turning(false);

                byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(
                4 * int.Parse(transform.parent.parent.name) + 1, DontDestroy.instance.iRoomNum)
                );
                _async.Send(data, Constant.NOTICE_PUZZLE);
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                _ani.SetBool("Move", false);
            

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GetComponent<Lever>().enabled = false;

                byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(
                4 * int.Parse(transform.parent.parent.name) + 2, DontDestroy.instance.iRoomNum)
                );
                _async.Send(data, Constant.NOTICE_PUZZLE);
            }
        }
    }

    private void OnDisable()
    {
        _ani.SetBool("Lever", false);

        if (transform.childCount != 0)
        {
            if (bIsUser)
            {
                transform.GetChild(0).SetParent(TFPlayer);
                TFPlayer.GetChild(TFPlayer.childCount - 1).localPosition = Vector3.zero;
                TFPlayer.GetChild(TFPlayer.childCount - 1).localRotation = Quaternion.identity;

                PlayerController.bDialog = false;
                Camera.main.GetComponent<FollowCam>().bDialog = false;
                gCamera.SetActive(false);
            }
            else
            {
                transform.GetChild(0).SetParent(TFDisPlayer);
                TFDisPlayer.GetChild(TFDisPlayer.childCount - 1).localPosition = Vector3.zero;
                TFDisPlayer.GetChild(TFDisPlayer.childCount - 1).localRotation = Quaternion.identity;
            }
        }
    }

    #endregion
}
