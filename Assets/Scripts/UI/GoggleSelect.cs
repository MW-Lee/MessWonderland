////////////////////////////////////////////////
//
// GoggleSelect
//
// 싱글플레이시 고글을 선택하는 창에서 작동하는 스크립트
// 
// 21. 04. 07
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoggleSelect : MonoBehaviour
{
    #region 변수

    public Transform TFCamera;
    public Transform TFCamdolph;

    #endregion

    ////////////////////////////////////////////////

    #region 함수

    // 빨강 고글을 선택했을 때 작동
    public void Click_Red()
    {
        for (int i = 0; i < TFCamdolph.childCount; i++)
        {
            TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(true, true);
        }

        this.gameObject.SetActive(false);
    }

    // 파란 고글을 선택했을 때 작동
    public void Click_Blue()
    {
        for (int i = 0; i < TFCamdolph.childCount; i++)
        {
            TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(false, true);
        }

        this.gameObject.SetActive(false);
    }

    #endregion

    ////////////////////////////////////////////////

    #region 실행

    private void OnDisable()
    {
        PlayerController.goggle = true;
        TFCamera.GetComponent<FollowCam>().enabled = false;
        TFCamera.GetComponent<FollowCam>().CursorStateChange();
        TFCamera.GetComponent<FPCamera>().enabled = true;
    }

    #endregion
}
