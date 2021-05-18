using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using System.IO;
using System.Text;
//using TMPro;
using System.Security.Cryptography;
using UnityEditor;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    #region 변수

    // ------------------------------------- //
    // Player Setting
    // ------------------------------------- //
    public Transform tfTony;
    public Transform tfEphi;

    // ------------------------------------- //
    // 기본 이동 관련 변수
    // ------------------------------------- //

    // (보낼 데이터) 움직이기 위한 방향벡터
    public Vector3 vDir;

    // 움직이는 속도
    public float fMoveSpeed;

    // 회전 속도
    public float fRotSpeed;

    // Transform을 미리 파싱해놓음 (최적화)
    private Transform TF;

    // (보낼 데이터) 캐릭터의 현재 상태
    private State sState;

    // (보낼 데이터) 캐릭터의 X축 이동량
    private float fHorizontal;

    // (보낼 데이터) 캐릭터의 Y축 이동량 
    [SerializeField]
    private float fVertical;

    // (보낼 데이터) 캐릭터의 Y축 기준 회전량
    //private float fRotateX;
    //private float fRotateY;

    private Vector3 vMoveDir;
    private Vector3 vFoward;
    private Vector3 vRight;
    private RaycastHit RHFoot;
    private Transform TFCamera;
    private Rigidbody RB;
    private float fSpeed;
    private float fState;

    private Animator animator;

    public bool bBCGroundCheck;
    public LayerMask GroundLM;

    public GameObject gGoggleTest;
    public static bool goggle = false;

    public GameObject gTorch;
    public static bool bTorch = false;

    private IngameManager _ingame;

    public Material[] mtSnows;
    public ParticleSystemRenderer psSnow;

    public Camera _mainCam;
    public static bool bDialog = false;

    public GameObject KeyInfo;
    private bool bKeyinfo;

    // ------------------------------------- //
    // 시간 관련 변수
    // ------------------------------------- //

    // 0.2초로 고정
    private short sOldTime;

    // DeltaTime을 더하여 0.2초 마다 자르기 위해 시간을 담을 변수
    private short sCurTime;

    // ------------------------------------- //
    // 서버 관련 변수
    // ------------------------------------- //

    // 상대쪽으로 데이터를 보내기 위한 JsonManager
    private JsonMgr jsonMgr;

    // 상대쪽에 Json파일을 만들 위치
    private string sDataPath;

    // Find player connect with server.
    // If connect success, player can move their Character.
    public static bool bIsOnline;
    public static bool bShowMouse;

    //public static int iUserNum;

    public MeshRenderer testMR;

    public Transform TFCamdolph;

    //public Transform goTony;
    //public Transform goEphi;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// 상대쪽으로 데이터를 보내는 함수
    /// </summary>
    private void JsonOverWirte()
    {
        // 보낼 내용을 정리
        //JsonClass json = new JsonClass(this.gameObject.transform, sState, vDir);

        object json = new JsonClass(this.gameObject.transform, sState, vDir);

        // 정리된 내용을 Json으로 변경
        string jsonData = jsonMgr.ObjectToJson(json);

        jsonData += '\0';

        // (Test) Json파일 생성
        //jsonMgr.CreateJsonFile(sDataPath, "DisPlayer", jsonData);

        // 변경된 Json을 치환
        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        // 쏘세요!
        AsyncClient.instance.Send(data, 6);
    }


    private bool UpdateHitPoint()
    {
        //if (Physics.Raycast((TF.position + Vector3.up), Vector3.down, out RHFoot, 1.1f))
        if (Physics.Raycast(TF.position + Vector3.up, Vector3.down, out RHFoot, 2, GroundLM))
        //if (Physics.Raycast(TF.position + Vector3.up, Vector3.down, out RHFoot, 1.1f, 8, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        return false;
    }

    private bool CaculateDir()
    {
        if (RHFoot.collider == null)
        {
            vFoward = TFCamera.forward;
            vRight = TFCamera.right;
        }
        else
        {
            //if (goggle)
            //{
            //    vFoward = Vector3.Cross(RHFoot.normal, Vector3.Normalize(-new Vector3(_goggleCam.transform.right.x, 0, _goggleCam.transform.right.z)));
            //}
            //else
            //{
                              
            //}

            vFoward = Vector3.Cross(RHFoot.normal, Vector3.Normalize(-new Vector3(TFCamera.right.x, 0, TFCamera.right.z)));
            vRight = Vector3.Cross(vFoward, -RHFoot.normal);
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            //    return;
            //if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            //    return;

            float fHorizontalDest = 0;
            float fVerticalDest = 0;


            if (Input.GetKey(KeyCode.W))
                fVerticalDest = 1;
            if (Input.GetKey(KeyCode.S))
                fVerticalDest = -1;
            if (Input.GetKey(KeyCode.D))
                fHorizontalDest = 1;
            if (Input.GetKey(KeyCode.A))
                fHorizontalDest = -1;

            fVertical = Mathf.Lerp(fVertical, fVerticalDest, Time.deltaTime * 9);
            fHorizontal = Mathf.Lerp(fHorizontal, fHorizontalDest, Time.deltaTime * 9);
            fSpeed = Mathf.Lerp(fSpeed, 1, Time.deltaTime * 9);
        }
        else
        {
            fVertical = Mathf.Lerp(fVertical, 0, Time.deltaTime * 20);
            fHorizontal = Mathf.Lerp(fHorizontal, 0, Time.deltaTime * 20);
            fSpeed = 0;
            vDir = Vector3.zero;

            if ((fVertical + fHorizontal) < 0.1f)
                return false;
            //fVertical = 0;
            //fHorizontal = 0;
        }

        vMoveDir = (vFoward * fVertical + vRight * fHorizontal).normalized;
        
        float temp = Vector3.Dot(TF.forward, vMoveDir);

        if(Vector3.Cross(TF.forward, vMoveDir).y > 0)
        {
            TF.Rotate(TF.up, (1 - temp) * fRotSpeed, Space.World);
        }
        else
        {
            TF.Rotate(-TF.up, (1 - temp) * fRotSpeed, Space.World);
        }

        vDir = vMoveDir * fSpeed;

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            _ingame.NoticeInteraction(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            _ingame.NoticeInteraction(other.gameObject);
        }
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        //초기 세팅
        if (IngameManager.instance.bSinglePlay)
        {
            tfTony.SetParent(transform);
            tfTony.localPosition = Vector3.zero;
            gTorch =
                tfTony.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).
                GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
        }
        else
        {
            if (DontDestroy.instance.iSeq == 0)
            {
                tfTony.SetParent(transform);
                tfTony.localPosition = Vector3.zero;
                gTorch =
                    tfTony.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).
                    GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
            }
            else
            {
                tfEphi.SetParent(transform);
                tfEphi.localPosition = Vector3.zero;
                gTorch =
            tfEphi.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).
            GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
            }
        }

        //테스트용
        //tfTony.SetParent(transform);
        //tfTony.localPosition = Vector3.zero;
        //gTorch =
        //    tfTony.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).
        //    GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;

        //tfEphi.SetParent(transform);
        //tfEphi.localPosition = Vector3.zero;
        //gTorch =
        //    tfEphi.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).
        //    GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;

        TF = GetComponent<Transform>();
        vDir = Vector3.zero;
        fHorizontal = .0f;
        fVertical = .0f;
        //fRotateX = .0f;
        //fRotateY = .0f;
        //fMoveSpeed = 1.0f;
        //fRotSpeed = 30.0f;

        //sDataPath = Application.dataPath + "/Resources";
        //sDataPath = Application.streamingAssetsPath;
        sDataPath = Application.persistentDataPath;

        sOldTime = 0;
        sCurTime = 10;

        jsonMgr = new JsonMgr();
        sState = State.Idle;

        bIsOnline = true;

        fState = 0;

        vMoveDir = Vector3.zero;
        vFoward = TF.forward;
        vRight = TF.right;

        TFCamera = Camera.main.transform;
        TFCamera.GetComponent<FollowCam>().target = TF;


        RB = GetComponent<Rigidbody>();


        _ingame = IngameManager.instance;

        bBCGroundCheck = false;

        animator = GetComponentInChildren<Animator>();
    }
    Vector3 velocity;


    private void Update()
    {
        Debug.DrawRay(TF.position + Vector3.up , Vector3.down * 2f);

        //iSnow = UnityEngine.Random.Range(0, 4);
        //psSnow.materials = mtSnows;

        if (!bShowMouse && !bDialog)
        {
            if (UpdateHitPoint() && bBCGroundCheck)
            {
                if (CaculateDir())
                {
                    animator.SetBool("Move", true);

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        sState = State.Run;
                        velocity = vDir * (fMoveSpeed + 5);

                        if (fState >= 0.95f)
                            fState = 1;
                        else
                            fState = Mathf.Lerp(fState, 1, 4 * Time.deltaTime);
                    }
                    else
                    {
                        sState = State.Walk;
                        velocity = vDir * fMoveSpeed;

                        if (fState <= 0.05f)
                            fState = 0;
                        else
                            fState = Mathf.Lerp(fState, 0, 4 * Time.deltaTime);
                    }

                    animator.SetFloat("State", fState);
                }
                else
                {
                    sState = State.Idle;
                    velocity = Vector3.zero;
                    animator.SetBool("Move", false);


                    if (fState <= 0.05f)
                        fState = 0;
                    else
                        fState = Mathf.Lerp(fState, 0, 6 * Time.deltaTime);
                    //print(fState);
                    animator.SetFloat("State", fState);
                }
            }
            else
            {
                velocity += Vector3.down * 20 * Time.deltaTime;
                print("Fall");

                //animator.SetBool("Move", false);
                fState = Mathf.Lerp(fState, 0, 4 * Time.deltaTime);
                //print(fState);
                animator.SetFloat("State", fState);
                // 공중에 떠있을때 구현
            }
            RB.velocity = velocity;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (goggle)
                {
                    for (int i = 0; i < TFCamdolph.childCount; i++)
                    {
                        if (IngameManager.instance.bSinglePlay)
                        {
                            TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(false, false);
                        }
                        else
                        {
                            if (DontDestroy.instance.iSeq == 0)
                                TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(true, false);
                            else
                                TFCamdolph.GetChild(i).GetComponentInChildren<Camdolph>().ChangeMaterial(false, false);
                        }
                    }

                    TFCamera.GetComponent<FPCamera>().enabled = false;
                    TFCamera.GetComponent<FollowCam>().enabled = true;
                    goggle = false;
                }

                if(KeyInfo)
                {
                    KeyInfo.SetActive(false);
                    bKeyinfo = false;
                }

                //_ingame.UseMessage("esc키가\n눌렸습니다.");
            }
            //if (Input.GetKeyUp(KeyCode.Escape))
            //{
            //    if (goggle)
            //    {
            //        for (int i = 0; i < TFCamdolph.childCount; i++)
            //        {
            //            TFCamdolph.GetChild(i).GetComponent<Camdolph>().ChangeMaterial(false, false);
            //        }

            //        goggle = false;
            //        TFCamera.GetComponent<FollowCam>().enabled = true;
            //        TFCamera.GetComponent<FollowCam>().CursorStateChange();
            //        TFCamera.GetComponent<FPCamera>().enabled = false;
                    
            //    }                
            //}
        }
        else
        {
            sState = State.Idle;
            RB.velocity = Vector3.zero;
            animator.SetBool("Move", false);


            if (fState <= 0.05f)
                fState = 0;
            else
                fState = Mathf.Lerp(fState, 0, 6 * Time.deltaTime);
            animator.SetFloat("State", fState);
        }

        if (bTorch)
        {
            gTorch.SetActive(true);
            animator.SetBool("Torch", true);
        }
        else
        {
            gTorch.SetActive(false);
            animator.SetBool("Torch", false);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            KeyInfo.SetActive(true);
            bKeyinfo = true;
        }
    }

    private void FixedUpdate()
    {
        if (sOldTime < sCurTime)
            sOldTime++;
        else
        { 
            //JsonOverWirte();
            sOldTime = 0;
        }
    }

    #endregion
}