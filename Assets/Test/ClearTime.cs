////////////////////////////////////////////////
//
// ClearTime
//
// Administrate Clear TImeline
// 
// 20. 05. 08
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearTime : MonoBehaviour
{
    public PlayableDirector PD;

    public void Update()
    {
        if(PD.state != PlayState.Playing)
        {
            gameObject.SetActive(false);
        }
    }
}
