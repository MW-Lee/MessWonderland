////////////////////////////////////////////////
//
// MessageWindow
//
// Activate in Message UI
// 
// 20. 06. 13
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    #region 변수

    public Text text;
    private bool bIsusing = false;

    #endregion


    #region 함수

    public void UseMessage(string desc)
    {
        if (!bIsusing)
        {
            bIsusing = true;
            text.text = desc;
            StartCoroutine(ShowMessage());
        }
        else
        {
            print("Message Using now");
        }
    }

    public IEnumerator ShowMessage()
    {
        while (true)
        {
            if(transform.localPosition.x <= 710)
            {
                break;
            }

            transform.Translate(-10, 0, 0);
            yield return null;
        }

        yield return new WaitForSeconds(1.7f);

        while (true)
        {
            if (transform.localPosition.x >= 1210)
            {
                break;
            }

            transform.Translate(10, 0, 0);
            yield return null;
        }

        text.text = "";
        bIsusing = false;
        yield return null;
    }

    #endregion


    #region 실행

    #endregion
}
