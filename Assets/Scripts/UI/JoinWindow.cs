using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoinWindow : MonoBehaviour
{
    #region 변수

    public static JoinWindow instance;

    public GameObject gRoomNumText;
    public InputField ifRoomNum;
    public GameObject gConnectingText;

    public bool bJoin = false;

    #endregion


    #region 함수

    public void OnClick_Join()
    {
        DontDestroy.instance.iSeq = 1;
        DontDestroy.instance.iRoomNum = int.Parse(ifRoomNum.text);

        char cType = 'j';
        int nNum = int.Parse(ifRoomNum.text);

        //string jsonData = cType.ToString() + nNum.ToString() + "000" + '\0';
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);

        byte[] data = AsyncClient.StructToByte(new PKT_REQ_ROOM(cType, nNum));

        AsyncClient.instance.Send(data, Constant.REQ_ROOM);

        gRoomNumText.SetActive(false);
        ifRoomNum.gameObject.SetActive(false);

        gConnectingText.SetActive(true);
        EventSystem.current.currentSelectedGameObject.SetActive(false);
    }

    #endregion


    #region 실행

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Update()
    {
        if (bJoin)
        {
            
            DontDestroy.instance.LoadNextScene("Stage_1");
        }
    }

    #endregion
}
