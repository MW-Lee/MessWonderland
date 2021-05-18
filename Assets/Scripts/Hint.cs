////////////////////////////////////////////////
//
// Hint
//
// Activate in Hint gameobject
// 
// 20. 06. 13
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    #region 변수

    float fTime = 0;

    #endregion


    #region 함수

    #endregion


    #region 실행

    private void FixedUpdate()
    {
        fTime += 1;

        if (fTime > 250)
        {
            fTime = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (!IngameManager.instance.bSinglePlay)
            DisPlayerController.instance.gameObject.layer = 9;
        // outline disable
    }

    #endregion
}
