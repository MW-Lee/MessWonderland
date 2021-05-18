////////////////////////////////////////////////
//
// DisPlayerController
//
// 상대쪽 캐릭터를 움직이기 위한 스크립트
// 
// 20. 03. 06
// MWLee
////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DisPlayerController : MonoBehaviour
{
    #region 변수

    // ------------------------------------- //
    // Player Setting
    // ------------------------------------- //
    public Transform tfTony;
    public Transform tfEphi;

    public static DisPlayerController instance;

    // ------------------------------------- //
    // 기본 이동 관련 변수
    // ------------------------------------- //

    // (받은 데이터) 움직이기 위한 방향벡터
    public Vector3 vDir;

    // 움직이는 속도
    public float fMoveSpeed;

    // 회전 속도
    public float fRotSpeed;

    // Transform을 미리 파싱해놓음 (최적화)
    private Transform TF;

    // (받은 데이터) 캐릭터의 현재 상태
    private State sState;

    // (임시 데이터) 0.2초마다 움직일 목적지
    private Vector3 vDestination;

    // (받은 데이터) 회전하기 위한 Quaternion
    private Quaternion vRot;

    private Animator animator;

    private Rigidbody RB;

    private float fState;

    public bool bBCGroundCheck;

    // ------------------------------------- //
    // 시간 관련 변수
    // ------------------------------------- //

    // 0.2초 고정
    private short sOldTime;

    // DeltaTime을 더하여 0.2초마다 자르기 위해 시간을 담을 변수
    private short sCurTime;


    // ------------------------------------- //
    // 서버 관련 변수
    // ------------------------------------- //

    // 로드할 파일 위치
    private string sDataPath;

    public static bool bIsOnline;

    JsonMgr jsonmgr = new JsonMgr();

    Vector3 vFirstPos;

    //public Transform goTony;
    //public Transform goEphi;
    

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// 상대 쪽에서 보낸 데이터를 받아서 적용시키기 위한 함수
    /// </summary>
    private void JsonRead()
    {
        // 만들어진 Json파일을 JsonClass형식으로 로드
        //jcReceiveData = new JsonMgr().LoadJsonFile<JsonClass>(sDataPath, "DisPlayer");

        //string temp = jsonmgr.LoadJsonFile<string>(sDataPath, "DisPlayer");
        //JsonClass _json = jsonmgr.JsonToObject<JsonClass>(temp);

        if (File.Exists(sDataPath + "/DisPlayer.json"))
        {
            JsonClass _json = jsonmgr.LoadJsonFile<JsonClass>(sDataPath, "DisPlayer");

            // 받은 데이터로 캐릭터를 움직이기
            vDestination = _json.vPos;
            vRot = _json.vRot;
            TF.localScale = _json.vScale;
            sState = _json.state;
            vDir = _json.vDir;

            if (!bIsOnline) bIsOnline = true;
        }
        else
        {
            TF.position = vFirstPos;
        }
    }

    private void RefreshPosition()
    {
        //TF.Translate(vDir * fMoveSpeed * Time.deltaTime);

        TF.position = Vector3.Lerp(TF.position, vDestination, 5f * Time.deltaTime);

        TF.rotation = Quaternion.Slerp(TF.rotation, vRot, 15 * Time.deltaTime);
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // 초기 세팅
        if (DontDestroy.instance.iSeq == 0)
        {
            tfEphi.SetParent(transform);
            tfEphi.localPosition = Vector3.zero;
        }
        else
        {
            tfTony.SetParent(transform);
            tfTony.localPosition = Vector3.zero;
        }

        TF = GetComponent<Transform>();
        vFirstPos = TF.position;
        vDir = Vector3.zero;
        fMoveSpeed = 1.0f;
        fRotSpeed = 30.0f;

        //sDataPath = Application.dataPath + "/Resources/Json";
        //sDataPath = Application.streamingAssetsPath + "/Json";
        sDataPath = Application.persistentDataPath;

        sOldTime = 0;
        sCurTime = 10;

        sState = State.Idle;

        //bIsOnline = true;


        RB = GetComponent<Rigidbody>();

        fState = 0;

        //if(PlayerController.iUserNum % 2 == 0)
        //{
        //    goEphi.SetParent(TF);
        //    animator = GetComponentInChildren<Animator>();
        //}
        //else
        //{
        //    goTony.SetParent(TF);
        //    animator = GetComponentInChildren<Animator>();
        //}

        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (bIsOnline)
        {
            switch(sState)
            {
                case State.Idle:
                    animator.SetBool("Move", false);
                    if (fState <= 0.05f)
                        fState = 0;
                    else
                        fState = Mathf.Lerp(fState, 0, 6 * Time.deltaTime);
                    animator.SetFloat("State", fState);
                    
                    break;

                case State.Walk:
                    animator.SetBool("Move", true);                    

                    if (fState <= 0.05f)
                        fState = 0;
                    else
                        fState = Mathf.Lerp(fState, 0, 4 * Time.deltaTime);
                    animator.SetFloat("State", fState);

                    //TF.Translate(vDir.normalized * fMoveSpeed * Time.deltaTime);
                    //TF.rotation = Quaternion.Slerp(TF.rotation, vRot,  2 * Time.deltaTime);
                    break;

                case State.Run:
                    animator.SetBool("Move", true);                    

                    if (fState >= 0.95f)
                        fState = 1;
                    else
                        fState = Mathf.Lerp(fState, 1, 4 * Time.deltaTime);
                    animator.SetFloat("State", fState);

                    //TF.Translate(vDir.normalized * (fMoveSpeed + 4) * Time.deltaTime);
                    //TF.rotation = Quaternion.Slerp(TF.rotation, vRot, 2 * Time.deltaTime);
                    break;
            }

            if (TF.position != vDestination)
                RefreshPosition();


            //// 현재 상태가 가만히 있지 않을 때만 작동
            //if (sState != State.Idle)
            //{
            //    animator.SetBool("Move", true);
            //    TF.Translate(vDir.normalized * fMoveSpeed * Time.deltaTime);

            //    TF.rotation = Quaternion.Slerp(TF.rotation, vRot, .2f);
            //}
            //// 현재 상태가 가만히 있는데 위치가 받은 위치와 다른 경우 보간이동
            //else if (TF.position != vDestination && sState == State.Idle)
            //{
            //    TF.Translate(vDir.normalized * fMoveSpeed * Time.deltaTime);
            //    TF.position = new Vector3(
            //        Mathf.Lerp(TF.position.x, vDestination.x, 2 * Time.deltaTime),
            //        Mathf.Lerp(TF.position.y, vDestination.y, 2 * Time.deltaTime),
            //        Mathf.Lerp(TF.position.z, vDestination.z, 2 * Time.deltaTime));
            //}
        }
    }

    private void FixedUpdate()
    {
        if (sOldTime < sCurTime)
        {
            sOldTime++;
        }
        else
        {
            JsonRead();
            sOldTime = 0;
        }
    }

    private void OnApplicationQuit()
    {
        File.Delete(sDataPath + "/DisPlayer.json");
    }

    #endregion
}
