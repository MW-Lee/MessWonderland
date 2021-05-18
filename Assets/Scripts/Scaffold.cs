////////////////////////////////////////////////
//
// Scaffold
//
// 발판에서 작동하는 함수
// 
// 20. 04. 27
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : MonoBehaviour
{
    #region 변수

    enum HowToUse
    {
        ButtonEnable,
        BasicScaffold,

        MAX
    }

    [SerializeField]
    private HowToUse _howToUse;

    public GameObject gButton;
    public bool bIsTrigger;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    private void OnTriggerEnter(Collider other)
    {
        switch (_howToUse)
        {
            case HowToUse.ButtonEnable:
                gButton.GetComponent<BoxCollider>().enabled = true;
                break;

            case HowToUse.BasicScaffold:
                if (!GetComponent<Obelisk>().CheckTurning())
                    GetComponent<Obelisk>().ActiveCollider();
                break;
        }
    }
    
    public void ColliderTriggerEnter()
    {
        gButton.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        switch (_howToUse)
        {
            case HowToUse.ButtonEnable:
                break;

            case HowToUse.BasicScaffold:
                if (!GetComponent<Obelisk>().CheckTurning())
                    GetComponent<Obelisk>().ActiveCollider();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (_howToUse)
        {
            case HowToUse.ButtonEnable:
                gButton.GetComponent<BoxCollider>().enabled = false;
                break;

            case HowToUse.BasicScaffold:
                break;
        }
    }

    public void ColliderTriggerExit()
    {
        gButton.GetComponent<BoxCollider>().enabled = false;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        bIsTrigger = false;
    }

    #endregion
}
