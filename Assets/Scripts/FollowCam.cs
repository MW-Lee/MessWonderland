////////////////////////////////////////////////
//
// FollowCam
//
// 플레이어 캐릭터를 따라다니는 3인칭 카메라 스크립트
// 
// 20. 04. 02
// MWLee
////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    #region 변수

    // 추적할 대상
    public Transform target;

    // 카메라와의 거리
    public float dist = 2.5f;

    // 카메라 회전 속도
    public float xSpeed = 200.0f;
    public float ySpeed = 100.0f;

    // 카메라 초기 위치
    private float x = 0.0f;
    private float y = 0.0f;

    // y값 제한 (위 아래 제한)
    public float yMinLimit = -30f;
    public float yMaxLimit = 70f;

    // esc키를 이용하여 마우스 커서 출현 / 고정
    private bool bShowMouse;

    private Transform TF;
    private RaycastHit hit;

    public GameObject goInteraction_F;

    public Material mtOutline;
    public Material[] mtrls;

    private GameObject goOld;
    //private Material mtOld;
    private Material[] mtOld;

    public LayerMask InteractionLM;

    private IngameManager _ingame;
    public bool bPuzzleUI;

    public GameObject gInventory;
    public GameObject gDialogWindow;
    public Queue<DialogStruct> qSentences;
    public Animator aniDialog;
    public Collider cOldDialog;
    public bool bDialog;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// 회전 각도가 너무 커지거나 작아지면 더이상 돌아가지 않게 제한
    /// </summary>
    /// <param name="angle">현재 각도</param>
    /// <param name="min">최소 각도</param>
    /// <param name="max">최대 각도</param>
    /// <returns></returns>
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    public void CursorStateChange()
    {
        bShowMouse ^= true;
        goInteraction_F.SetActive(false);
        Cursor.visible = bShowMouse;
        PlayerController.bShowMouse = bShowMouse;
        Cursor.lockState = bShowMouse ? CursorLockMode.None : CursorLockMode.Locked;
    }


    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    void Start()
    {
        // 커서 고정
        Cursor.lockState = CursorLockMode.None;

        Vector3 angles = transform.eulerAngles;

        x = angles.y;
        y = angles.x;

        bShowMouse = true;

        TF = transform;

        mtrls = new Material[2];
        _ingame = IngameManager.instance;

        bPuzzleUI = false;

        qSentences = new Queue<DialogStruct>();

        CursorStateChange();
    }

    void Update()
    {
        if (!bShowMouse && !bDialog)
        {
            // Caculate camera's rotate speed
            x += Input.GetAxis("Mouse X") * xSpeed * 0.015f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.015f;

            // Limited angle 'Y'
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Caculated camera's position
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position =
                rotation * new Vector3(0, 0.9f, -dist) + target.position + Vector3.up;

            transform.rotation = rotation;
            //target.rotation = Quaternion.Euler(0, x, 0);
            transform.position = position;


            //
            // Shooting Ray for interaction with object & make outline
            //
            if (Physics.Raycast(TF.position, TF.forward, out hit, 15, InteractionLM))
            {
                if (goOld != hit.transform.gameObject)
                {
                    if (goOld != null)
                    {
                        //TryGetRenderer(goOld).sharedMaterials = mtOld;
                        if (!goOld.transform.CompareTag("Player"))
                            goOld.GetComponent<ActivateOutline>().DisActiveOutline();

                        //mtrls[0] = mtOld;
                        //mtrls[1] = null;
                        //TryGetRenderer(goOld).sharedMaterials = mtrls;
                        ////goOld.GetComponent<MeshRenderer>().sharedMaterials = mtrls;
                    }

                    if (!hit.transform.CompareTag("Player"))
                        hit.collider.GetComponent<ActivateOutline>().ActiveOutline();

                    goInteraction_F.SetActive(true);
                    goOld = hit.transform.gameObject;
                    //mtOld = new Material[TryGetRenderer(hit.transform.gameObject).sharedMaterials.Length];
                    //mtOld = TryGetRenderer(hit.transform.gameObject).sharedMaterials;
                    //mtrls = new Material[mtOld.Length + 1];
                    //mtrls[0] = mtOutline;
                    //for(int i = 0; i < mtOld.Length; i++)
                    //{
                    //    mtrls[i + 1] = mtOld[i];
                    //}
                    //TryGetRenderer(goOld).sharedMaterials = mtrls;

                    //mtrls[0] = TryGetRenderer(hit.transform.gameObject).sharedMaterial;
                    ////mtrls[0] = hit.transform.GetComponent<MeshRenderer>().sharedMaterial;
                    //mtOld = mtrls[0];
                    //mtrls[1] = mtrls[0];
                    //mtrls[0] = mtOutline;
                    //TryGetRenderer(goOld).sharedMaterials = mtrls;
                    ////goOld.GetComponent<MeshRenderer>().sharedMaterials = mtrls;
                }

            }
            else
            {
                if(goOld != null)
                {
                    //if (!goOld.CompareTag("Player"));
                    if(goOld.tag != "Player")
                        goOld.GetComponent<ActivateOutline>().DisActiveOutline();
                    goInteraction_F.SetActive(false);
                    //TryGetRenderer(goOld).sharedMaterials = mtOld;
                    ////mtrls[0] = mtOld;
                    ////mtrls[1] = null;
                    ////TryGetRenderer(goOld).sharedMaterials = mtrls;
                    ////goOld.GetComponent<MeshRenderer>().sharedMaterials = mtrls;
                    goOld = null;
                }
            }


            if (Input.GetKeyUp(KeyCode.F))
            {
                //if (Physics.Raycast(TF.position + (Vector3.up * 1.5f), TFCamera.forward, out hit, 50, InteractionLM))
                if (Physics.Raycast(TF.position, TF.forward, out hit,15, InteractionLM))
                {
                    switch (hit.transform.tag)
                    {
                        case "Player":
                            byte[] data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(100, DontDestroy.instance.iRoomNum));
                            AsyncClient.instance.Send(data, Constant.NOTICE_PUZZLE);

                            IngameManager.instance.ShowHint();
                            return;

                        case "Camdolph":
                            hit.transform.parent.parent.parent.parent.GetComponent<Camdolph>().ShowBubble();
                            return;

                        case "CamdolphLock":
                            hit.transform.GetComponent<Camdolph_Lock>().Interact();
                            CursorStateChange();
                            bPuzzleUI = true;
                            return;

                        case "Dialog":                            
                            //qSentences = hit.transform.GetComponent<Dialog>().sentences;
                            qSentences = new Queue<DialogStruct>(hit.transform.GetComponent<Dialog>().sentences);
                            aniDialog = hit.transform.GetComponent<Dialog>()._ani;
                            cOldDialog = hit.transform.GetComponent<Collider>();
                            gDialogWindow.SetActive(true);
                            //gDialogWindow.GetComponent<DialogWindow>().sentences
                            //    = hit.transform.GetComponent<Dialog>().sentences;
                            //gDialogWindow.GetComponent<DialogWindow>().StartDialog();
                            return;

                        case "Lever":
                            hit.transform.GetComponent<Lever>().bIsUser = true;
                            hit.transform.GetComponent<Lever>().enabled = true;

                            return;

                        case "Lever_Updown":
                            hit.transform.GetComponent<Lever_Updown>().ActiveStair();

                            data = AsyncClient.StructToByte(new PKT_NOTICE_PUZZLE(
                                int.Parse(hit.transform.parent.parent.name) + 3, DontDestroy.instance.iRoomNum));

                            AsyncClient.instance.Send(data, Constant.NOTICE_PUZZLE);
                            return;
                    }

                    if (hit.transform.parent.parent.name == "ButtonBox")
                    {
                        hit.transform.parent.GetChild(1).gameObject.SetActive(true);

                        if (!hit.transform.GetComponent<Obelisk>().CheckTurning())
                        {
                            hit.transform.GetComponent<Obelisk>().ActiveCollider();
                        }

                        if (!IngameManager.instance.bSinglePlay)
                            _ingame.NoticeInteraction(hit.transform.gameObject);
                    }
                }
                else
                {
                    // Nothing in hit, Open inventory
                    gInventory.SetActive(true);
                    CursorStateChange();
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.R) && !bPuzzleUI)
        {
            CursorStateChange();
        }
    }

    #endregion
}
