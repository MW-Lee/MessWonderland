////////////////////////////////////////////////
//
// CharacterStateController
//
// 플레이어를 움직이기 위한 스크립트
// 
// 20. 05. 03
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    #region 변수

    private FiniteStateMachine<CharacterStateController> fsm = new FiniteStateMachine<CharacterStateController>();

    private CharacterState_Idle _idle = new CharacterState_Idle();
    private CharacterState_Move _move = new CharacterState_Move();

    #endregion



    #region 함수

    public void ChangeState(IFFiniteStateMachine<CharacterStateController> _state)
    {
        fsm.ChangeState(this, _state);
    }

    public void ChangeState(State_Character _input)
    {
        switch (_input)
        {
            case State_Character._idle:
                ChangeState(_idle);
                return;

            case State_Character._move:
                ChangeState(_move);
                return;
        }
    }

    #endregion



    #region 실행

    private void Start()
    {
        ChangeState(_idle);
    }

    private void Update()
    {
        fsm.UpdateState();
    }

    #endregion
}
