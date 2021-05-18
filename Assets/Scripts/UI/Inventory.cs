////////////////////////////////////////////////
//
// Inventory
//
// This script is activate in inventory
// 
// 20. 06. 04
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    #region 변수

    public GameObject gAnimation;
    public GameObject gWindow;
    public Button Btn_Use;
    public Text tName;

    public Transform TFCamera;
    public Transform TFCamdolph;
    public Transform TFItem;

    public AudioSource clickSound;

    private int iSelect;

    #endregion


    #region 함수

    public void EndAnimation()
    {
        gAnimation.SetActive(false);
        gWindow.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.transform.parent.name == "Item")
        {
            // 선택한 애니메이션? 테두리? 추가
            Btn_Use.interactable = true;
            clickSound.PlayOneShot(clickSound.clip);
            tName.text = eventData.pointerCurrentRaycast.gameObject.name;
            iSelect = eventData.pointerCurrentRaycast.gameObject.transform.GetSiblingIndex();
        }
        else
        {
            // 선택된거 해제
            Btn_Use.interactable = false;
            tName.text = "-";
            iSelect = -1;
        }
    }

    public void OnClickUse()
    {
        switch (iSelect)
        {
            case 0:
                // Compass
                Activate_Compass();
                break;

            case 1:
                // Goggle
                Activate_Goggle();
                break;

            case 2:
                // DropWater
                break;

            case 3:
                // Toach
                Activate_Torch();
                break;

            case 4:
                // Flower of Cactus
                break;
        }

        gameObject.SetActive(false);
    }

    void Activate_Compass()
    {
        if (IngameManager.instance.bSinglePlay)
        {
            IngameManager.instance.ShowHint();
        }
        else
        {
            byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(100, DontDestroy.instance.iRoomNum));

            AsyncClient.instance.Send(data, Constant.NOTICE_PUZZLE);

            IngameManager.instance.bCompass = true;
        }
    }

    void Activate_Goggle()
    {
        for (int i = 0; i < TFCamdolph.childCount; i++)
        {
            // 싱글 플레이 전용 고글
            if (IngameManager.instance.bSinglePlay)
            {
                TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(false, true);
            }
            else
            {
                // 서버 연결을 통해 자신의 플레이어 번호를 확인하여 고글을 변경하는 시스템
                if (DontDestroy.instance.iSeq == 0)
                    TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(true, true);
                else
                    TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(false, true);
            }
        }

        // 화면 전환
        PlayerController.goggle = true;
        TFCamera.GetComponent<FollowCam>().enabled = false;
        TFCamera.GetComponent<FollowCam>().CursorStateChange();
        TFCamera.GetComponent<FPCamera>().enabled = true;
    }

    void Activate_Torch()
    {
        if (PlayerController.bTorch)
        {
            PlayerController.bTorch = false;
        }
        else
        {
            PlayerController.bTorch = true;
        }
    }

    #endregion


    #region 실행

    private void Start()
    {
        iSelect = -1;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (iSelect != 1 || !IngameManager.instance.bSinglePlay)
            TFCamera.GetComponent<FollowCam>().CursorStateChange();
        gAnimation.SetActive(true);
        gWindow.SetActive(false);
        iSelect = -1;
        tName.text = "-";
        Btn_Use.interactable = false;        
    }

    #endregion
}
