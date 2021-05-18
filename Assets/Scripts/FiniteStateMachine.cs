////////////////////////////////////////////////
//
// FiniteStateMachine
//
// 객체를 움직일 때 사용하는 FSM 관련 스크립트
// 
// 20. 05. 03
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFFiniteStateMachine<T>
{
    void StateEnter(T _object);
    void StateUpdate();
    void StateExit();
}

public enum State_Character
{
    _idle,
    _move,


    _MAX
}

public class FiniteStateMachine<T>
{
    #region 변수

    IFFiniteStateMachine<T> currentState;

    #endregion


    #region 함수

    public void ChangeState(T _object, IFFiniteStateMachine<T> _state)
    {
        if(currentState != null)
        {
            currentState.StateExit();
        }

        currentState = _state;
        currentState.StateEnter(_object);
    }

    public void UpdateState()
    {
        currentState.StateUpdate();
    }
    #endregion
}

//////////////////////////////////////////////////////////////////////////////////////////////

public class CharacterState_Idle : IFFiniteStateMachine<CharacterStateController>
{
    #region 변수

    CharacterStateController csc;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void StateEnter(CharacterStateController _object)
    {
        csc = _object;
    }

    public void StateUpdate()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            csc.ChangeState(State_Character._move);
        }
    }

    public void StateExit()
    {

    }

    #endregion
}

public class CharacterState_Move : IFFiniteStateMachine<CharacterStateController>
{
    #region 변수

    CharacterStateController csc;
    CharacterInfo cinfo;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void StateEnter(CharacterStateController _object)
    {
        csc = _object;
        cinfo = CharacterInfo.instance;
    }

    public void StateUpdate()
    {





        if (Input.GetKey(KeyCode.None))
        {
            csc.ChangeState(State_Character._idle);
        }
    }

    public void StateExit()
    {

    }

    #endregion
}