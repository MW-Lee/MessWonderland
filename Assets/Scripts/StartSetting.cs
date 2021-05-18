////////////////////////////////////////////////
//
// StartSetting
//
// Setting the world when start
// 
// 20. 03. 06
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartSetting : MonoBehaviour
{
    #region 변수

    public Transform Player;
    public Transform DisPlayer;

    public GameObject Tooltip;

    private PlayableDirector PD;
    #endregion


    #region 함수

    public void SetPosition()
    {
        if(DontDestroy.instance.iSeq == 0)
        {
            Player.position = new Vector3(306, 3.4f, -220);
            DisPlayer.position = new Vector3(345, 20, -290);
        }
        else
        {
            Player.position = new Vector3(345, 20, -290);
            DisPlayer.position = new Vector3(306, 3.4f, -220);
        }
    }

    #endregion


    #region 실행

    private void Start()
    {
        PD = GetComponent<PlayableDirector>();
        Camera.main.GetComponent<FollowCam>().CursorStateChange();

        SetPosition();
    }

    private void OnDisable()
    {
        Tooltip.SetActive(true);
    }

    #endregion

}
