using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOutline : MonoBehaviour
{
    public Material[] mtrls_Main;
    public Material[] mtrls_Outline;

    private Renderer MRMain;

    public void ActiveOutline()
    {
        MRMain.sharedMaterials = mtrls_Outline;
    }

    public void DisActiveOutline()
    {
        MRMain.sharedMaterials = mtrls_Main;
    }

    private void Start()
    {
        if(GetComponent<MeshRenderer>() != null)
        {
            MRMain = GetComponent<MeshRenderer>();
        }
        else if(GetComponent<SkinnedMeshRenderer>() != null)
        {
            MRMain = GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            Debug.Log("There is no Renderer");
        }
    }
}
