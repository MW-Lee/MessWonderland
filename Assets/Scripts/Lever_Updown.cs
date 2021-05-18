////////////////////////////////////////////////
//
// Lever_Updown
//
// Activate in Updown lever
// 
// 20. 06. 14
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Updown : MonoBehaviour
{
    #region 변수

    public Animator ani_Stair;

    #endregion


    #region 함수

    public void ActiveStair()
    {
        if (ani_Stair.GetBool("Up"))
        {
            ani_Stair.SetBool("Up", false);
        }
        else
        {
            ani_Stair.SetBool("Up", true);
        }
    }

    #endregion


    #region 실행



    #endregion
}
