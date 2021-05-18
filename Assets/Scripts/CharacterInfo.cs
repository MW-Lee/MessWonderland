////////////////////////////////////////////////
//
// CharacterInfo
//
// 플레이어의 여러 정보를 담는 스크립트
// 
// 20. 05. 03
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    #region 변수

    // ------------------------------------- //
    // 기본 이동 관련 변수
    // ------------------------------------- //
    public static CharacterInfo instance;
    public static bool bIsOnline;
    public static bool bShowMouse;

    public Vector3 vDir;
    public Vector3 vMoveDir;
    public Vector3 vFoward;
    public Vector3 vRight;
    public Vector3 vVelocity;

    public RaycastHit RHEye;
    public RaycastHit RHFoot;

    public float fMoveSpeed;
    public float fRotSpeed;

    public Transform TF;
    public Transform TFCamera;
    public Rigidbody RB;
    public State sState;

    // ------------------------------------- //
    // 시간 관련 변수
    // ------------------------------------- //

    // 0.2초로 고정
    private float fOldTime;

    // DeltaTime을 더하여 0.2초 마다 자르기 위해 시간을 담을 변수
    private float fCurTime;

    #endregion



    #region 함수

    private bool UpdateHitPoint()
    {
        if (Physics.Raycast(TF.position, Vector3.down, out RHFoot, 0.1f))
        {
            return true;
        }

        return false;
    }

    private void CaculateDir()
    {
        vFoward = Vector3.Cross(RHFoot.normal, Vector3.Normalize(-new Vector3(TFCamera.right.x, 0, TFCamera.right.z)));
        vRight = Vector3.Cross(vFoward, -RHFoot.normal);


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
        if (UpdateHitPoint())
        {

        }
    }

    #endregion
}
