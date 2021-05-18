////////////////////////////////////////////////
//
// Camdolph_Lock
//
// This script is activate in "Camdolph lock"
// 
// 20. 03. 06
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camdolph_Lock : MonoBehaviour
{
    #region 변수

    public GameObject gLock;

    #endregion


    #region 함수

    public void Interact()
    {
        gLock.SetActive(true);
    }

    #endregion


    #region 실행

    #endregion
}
