////////////////////////////////////////////////
//
// Connect Button
//
// 연결 버튼을 위한 스크립트
// 
// 20. 04. 03
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    private byte[] data;

    public void OnClick()
    {
        //AsyncClient.instance.Receive(AsyncClient.instance.socket);

        AsyncClient.instance.Send(data, Constant.REQ_ROOM);

        DontDestroy.instance.LoadNextScene("Stage_1");
    }

    private void Start()
    {
        string jsonData = string.Empty;

        char cType = 'c';
        int nNum = -1;

        jsonData = cType.ToString() + nNum.ToString() + '\0';

        data = Encoding.UTF8.GetBytes(jsonData);
    }
}
