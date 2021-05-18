using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateWindow : MonoBehaviour
{
    #region 변수

    public static CreateWindow instance;

    public bool bSuccess = false;
    public bool bJoin = false;
    public int iRoomNum = -1;

    public Text txt;

    #endregion


    #region 함수

    #endregion


    #region 실행

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Update()
    {
        if(bSuccess)
        {
            txt.enabled = true;
            txt.text = iRoomNum.ToString();
            DontDestroy.instance.iSeq = 0;
            DontDestroy.instance.iRoomNum = iRoomNum;

            bSuccess = false;
        }

        if (bJoin)
        {
            DontDestroy.instance.LoadNextScene("Stage_1");
        }
    }

    #endregion
}
