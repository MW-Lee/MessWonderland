////////////////////////////////////////////////
//
// DontDestroy
//
// Scene이 전환되어도 없어지면 안되는 정보를 모음
// 
// 20. 03. 06
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // 싱글톤 사용을 위한 instance
    public static DontDestroy instance;

    // 다음 Scene의 이름을 저장하는 변수
    public string sNextScene;

    // 현재 클라이언트에서 플레이어가 몇번째 플레이어인지 저장해주는 변수
    public int iSeq;
    // 현재 클라이언트에서 플레이어가 속해있는 방 번호를 저장하는 번호
    public int iRoomNum;

    /// <summary>
    /// 현재 클라이언트가 SinglePlay 전용으로 진행되는지 저장하는 변수
    /// </summary>
    public bool bSinglePlay;


    /// <summary>
    /// 다음 Scene을 호출하는 함수
    /// </summary>
    /// <param name="_input">다음 Scene의 이름</param>
    public void LoadNextScene(string _input)
    {
        sNextScene = _input;
        SceneManager.LoadScene("LoadingScene");
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this);
    }

    
}
