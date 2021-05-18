////////////////////////////////////////////////
//
// Camdolph_LockUI
//
// This script is activate in "Camdolph_Lock_UI"
// 
// 20. 05. 28
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camdolph_LockUI : MonoBehaviour
{
    #region 변수

    public InputField tRed;
    public InputField tGreen;
    public InputField tBlue;

    public InputField tBlock1;
    public InputField tBlock2;
    public InputField tBlock3;

    public GameObject ClearCam;
    public Transform TFCamera;

    public Animator aniLock;
    public GameObject gLocker;
    public GameObject gParticle;

    #endregion


    #region 함수

    public void OpenClick()
    {
        //Camera.main.GetComponent<FollowCam>().CursorStateChange();
        Camera.main.GetComponent<FollowCam>().bPuzzleUI = false;

        if (tRed.text == "3" && tGreen.text == "3" && tBlue.text == "4")
        {
            // Clear
            print("Clear");

            ClearCam.SetActive(true);
            gLocker.SetActive(false);
            gParticle.SetActive(false);
            aniLock.SetBool("Open", true);
        }
        else
        {
            // UnClear
            print("Fail");
        }

        gameObject.SetActive(false);
    }

    #endregion


    #region 실행

    private void OnEnable()
    {
        tRed.text = null;
        tGreen.text = string.Empty;
        tBlue.text = "";

        tBlock1.text = string.Empty;
        tBlock2.text = string.Empty;
        tBlock3.text = string.Empty;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {            
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Camera.main.gameObject.GetComponent<FollowCam>().CursorStateChange();
    }

    #endregion
}
