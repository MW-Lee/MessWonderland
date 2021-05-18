using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SandAni
{
    Greet,
    Communicate,
    FakeRun,
    Surprise,
    EndDialog
}

[Serializable]
public struct DialogStruct
{
    public string sName;
    public string sDesc;
    public SandAni _ani;

    public DialogStruct(string name, string desc, int ani)
    {
        sName = name;
        sDesc = desc;
        _ani = (SandAni)ani;
    }
}

public class Dialog : MonoBehaviour
{
    #region 변수

    [SerializeField]
    public DialogStruct[] InputSentence;

    public Queue<DialogStruct> sentences = new Queue<DialogStruct>();

    public Animator _ani;

    #endregion

    #region 실행

    private void Start()
    {
        for(int i = 0; i < InputSentence.Length; i++)
        {
            sentences.Enqueue(InputSentence[i]);
        }
    }

    #endregion
}
