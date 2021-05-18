using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    #region 변수

    byte[] data;

    public GameObject gCreateWindow;
    public GameObject gJoinWindow;

    public Image MainImage;
    public Sprite sTitle2;

    #endregion


    #region 함수

    public void OnClick_Start()
    {
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        MainImage.sprite = sTitle2;
    }

    public void OnClick_Create()
    {
        char cType = 'c';
        int nNum = -1;

        //string jsonData = cType.ToString() + nNum.ToString() + '\0';
        //data = System.Text.Encoding.UTF8.GetBytes(jsonData);
        data = AsyncClient.StructToByte(new PKT_REQ_ROOM(cType, nNum));

        AsyncClient.instance.Send(data, Constant.REQ_ROOM);

        gCreateWindow.SetActive(true);
    }

    public void OnClick_Join()
    {
        gJoinWindow.SetActive(true);
    }

    #endregion


    #region 실행

    #endregion
}
