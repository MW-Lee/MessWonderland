using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;

public class Camdolph : MonoBehaviour
{
    #region 변수

    public enum NoseColor
    {
        red,
        green,
        blue
    }

    public Material mtrl_basic;
    public Material[] mtrl_red;
    public Material[] mtrl_blue;

    public MeshRenderer MRNose;

    public GameObject goBubble;

    public NoseColor noseColor;

    Animator _animator;

    public GameObject gPoint1;
    public GameObject gPoint2;
    private Vector3 vDest;
    public Vector3 vDir;

    private bool bDestIsOne;

    public float fMoveSpeed;

    #endregion


    #region 함수

    /// <summary>
    /// Change camdolph's nose color
    /// </summary>
    /// <param name="_isred">Is this player's color is red? if blue input 'false'</param>
    /// <param name="_input">Is this player wear goggle?</param>
    public void ChangeMaterial(bool _isred ,bool _input)
    {
        if (_input)
        {
            if (_isred)
                MRNose.material = mtrl_red[(int)noseColor];
            else
                MRNose.material = mtrl_blue[(int)noseColor];
        }
        else
        {
            MRNose.material = mtrl_basic;
        }
    }

    public void ShowBubble()
    {
        goBubble.SetActive(true);
    }

    public void MoveToDest()
    {
        if (Vector3.Distance(transform.position, vDest) <= 1.5f)
        { 
            _animator.SetBool("Move", false);

            if (vDest == gPoint1.transform.position)
                vDest = gPoint2.transform.position;
            else
                vDest = gPoint1.transform.position;

            vDir = Vector3.Normalize(vDest - transform.position);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(vDest - transform.position), .05f);

            transform.Translate(Vector3.forward * Time.smoothDeltaTime);
        }
    }

    public void End_Idle()
    {
        _animator.SetBool("Move", true);
    }

    /// <summary>
    /// If MeshRenderer of object is 'skinned mesh renderer'
    /// bake each time and put that mesh in mesh collider.
    /// Not use : Frame down so serious
    /// </summary>
    //public void BakedBodyMesh()
    //{
    //    public SkinnedMeshRenderer SMRBody;
    //    public MeshCollider MCBody;
    //    private Mesh _mBody;
    //    _mBody = new Mesh();
    //    SMRBody.BakeMesh(_mBody);
    //    MCBody.sharedMesh = null;
    //    MCBody.sharedMesh = _mBody;
    //}

    #endregion


    #region 실행

    private void Start()
    {
        _animator = GetComponent<Animator>();

        vDest = gPoint2.transform.position;

        vDir = Vector3.Normalize(vDest - transform.position);

        //RefreshDest();
    }

    private void Update()
    {
        if (_animator.GetBool("Move"))
            MoveToDest();
    }

    #endregion
}
