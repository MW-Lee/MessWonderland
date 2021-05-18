using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearCam : MonoBehaviour
{
    private PlayableDirector PD;

    private void Start()
    {
        PD = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if(PD.state != PlayState.Playing)
        {
            gameObject.SetActive(false);
        }
    }
}
