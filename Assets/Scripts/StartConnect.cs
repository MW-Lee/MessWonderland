using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartConnect : MonoBehaviour
{
    public GameObject _btn;
    public GameObject gLogo;

    Text txt;

    bool send = false;
    bool receive = false;

    float fTime = 0;

    private void Start()
    {
        txt = GetComponent<Text>();        
    }

    private void Update()
    {
        if (!send)
        {
            AsyncClient.instance.Send(AsyncClient.instance.socket, AsyncClient.instance.sendBuffer);
            send = true;
        }
        else
            fTime += Time.deltaTime;

        if (fTime > 0.7f && !receive)
        {
            AsyncClient.instance.Receive(AsyncClient.instance.socket);
            receive = true;
        }

        if (send && receive)
        {
            //txt.text = "MessWonderland";
            //gLogo.SetActive(true);
            _btn.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(fTime > 0.7f)
        {
            send = false;
            fTime = 0;
        }
    }
}
