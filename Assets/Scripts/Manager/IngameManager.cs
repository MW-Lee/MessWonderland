////////////////////////////////////////////////
//
// IngameManager
//
// Administrate stage puzzle
// 
// 20. 05. 08
// MWLee
////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    #region 변수

    public static IngameManager instance;

    [SerializeField]
    public Puzzles[] puzzles;

    /// <summary>
    /// 나침반을 사용할 때 나오는 다음 퍼즐 안내
    /// </summary>
    public List<GameObject> gHint;
    /// <summary>
    /// 현재 진행도에 비례하여 나침반 사용시 보여줄 힌트 번호를 저장
    /// </summary>
    public int iHint = -1;

    // 서버를 통해 어떤 동작이 들어왔는지 번호를 저장하는 변수
    public int iActivate;
    // 멀티플레이에서 나침반 사용에 관련해서 작동하는 변수
    public bool bCompass = false;
    // 우측 하단에 작게 뜨는 알림창
    public MessageWindow gMessageWindow;

    // 인벤토리의 보이는 버튼의 정보를 저장한 배열
    public bool[] bInventory;

    // 현재 진행중인 퍼즐의 순서를 저장하고 있는 변수
    private int iCurrent;

    // 현재 클라이언트가 싱글플레이인지 저장하는 변수
    public bool bSinglePlay;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void Activate_Compass()
    {
        if(!bCompass)
        {
            // Change Layer to Interaction
            DisPlayerController.instance.gameObject.layer = 30;
            // Activate Outline
            UseMessage("다른 플레이어가 나침반\n사용을 희망하고 있습니다");
        }
        else
        {
            // show hint
            ShowHint();
            bCompass = false;
        }
    }

    public void ShowHint()
    {
        gHint[iHint].SetActive(true);
    }

    public void UseMessage(string desc)
    {
        gMessageWindow.UseMessage(desc);
    }
    
    public void NoticeInteraction(GameObject _obj)
    {
        Puzzle[] temp = puzzles[iCurrent].puzzle;

        for(int i = 0; i < temp.Length; i++)
        {
            if(temp[i].GO == _obj)
            {
                JsonOverWrite(i);
                return;
            }
        }
    }

    private void JsonOverWrite(int _i)
    {
        //string jsonData = _i.ToString() + DontDestroy.instance.iRoomNum.ToString() + "000" + '\0';
        //byte[] data = Encoding.UTF8.GetBytes(jsonData);

        byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(_i, DontDestroy.instance.iRoomNum));

        AsyncClient.instance.Send(data, Constant.NOTICE_PUZZLE);
    }

    public void JsonReceive(char _input)
    {
        Debug.Log("Receive : " + _input);

        puzzles[iCurrent].puzzle[(int)char.GetNumericValue(_input)].Activate();
    }

    public void ClearStage()
    {
        for(int i = 0; i < puzzles[iCurrent].puzzle.Length; i++)
        {
            puzzles[iCurrent].puzzle[i].GO.GetComponent<Collider>().enabled = false;
        }

        iCurrent++;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        // 싱글플레이 제작을 위해서 임시로 지정 ★
        //bSinglePlay = DontDestroy.instance.bSinglePlay;
        bSinglePlay = true;
    }

    private void Start()
    {
        iCurrent = 0;
        iActivate = -1;

        bInventory = new bool[6] { false, false, false, false, false, false };
        //AsyncClient.instance._ingame = this;
    }

    private void Update()
    {
        if (iActivate >= 0)
        {
            // Compass
            if (iActivate == 100)
            {
                Activate_Compass();
            }
            else
            {
                if (puzzles[iCurrent].puzzle[iActivate].type == InteractionType.Lever_Turn)
                    puzzles[iCurrent].puzzle[iActivate].Activate(iActivate);
                else
                    puzzles[iCurrent].puzzle[iActivate].Activate();
            }

            iActivate = -1;
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            iCurrent++;
        }
    }

    #endregion
}

public enum InteractionType
{
    Button,
    Scaffold,
    Lever_Turn,
    Lever_Updown,

    MAX
}

[Serializable]
public struct Puzzle
{
    public GameObject GO;
    public InteractionType type;

    public void Activate()
    {
        switch(type)
        {
            case InteractionType.Button:
                Obelisk tempObelisk = GO.GetComponent<Obelisk>();

                if (!tempObelisk.CheckTurning())
                    tempObelisk.ActiveCollider();

                return;

            case InteractionType.Scaffold:
                Scaffold tempScaffold = GO.GetComponent<Scaffold>();

                if (tempScaffold.bIsTrigger)
                    tempScaffold.ColliderTriggerExit();
                else
                    tempScaffold.ColliderTriggerEnter();

                return;

            case InteractionType.Lever_Updown:
                Lever_Updown tempUDLever = GO.GetComponent<Lever_Updown>();

                tempUDLever.ActiveStair();
                return;
        }
    }

    public void Activate(int input)
    {
        Lever tempLever = GO.GetComponent<Lever>();

        if (tempLever.enabled)
        {
            tempLever.ActiveLever(input);
        }
        else
        {
            tempLever.bIsUser = false;
            GO.GetComponent<Lever>().enabled = true;

            tempLever.ActiveLever(input);
        }
    }
}

[Serializable]
public struct Puzzles
{
    public Puzzle[] puzzle;
}