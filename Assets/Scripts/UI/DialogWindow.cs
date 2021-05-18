using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour
{
    #region 변수

    public Text tName;
    public Text tDesc;
    public GameObject gF;
    public Queue<DialogStruct> sentences = new Queue<DialogStruct>();

    private Animator _ani;
    private DialogStruct sentence;
    private bool bTyping = false;

    #endregion


    #region 함수

    public void StartDialog()
    {
        gF.SetActive(false);
        //_ani.SetInteger("Ani", -1);

        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        
        sentence = sentences.Dequeue();
        _ani.SetInteger("Ani", (int)sentence._ani);
        StopAllCoroutines();
        StartCoroutine(TypeSentence());
    }

    public void EndDialog()
    {
        sentences.Clear();
        PlayerController.bDialog = false;
        Camera.main.GetComponent<FollowCam>().bDialog = false;
        _ani.SetInteger("Ani", -1);
        Camera.main.GetComponent<FollowCam>().cOldDialog.enabled = false;
        Camera.main.GetComponent<FollowCam>().cOldDialog = null;
        gameObject.SetActive(false);
    }

    IEnumerator TypeSentence()
    {
        tName.text = sentence.sName;
        tDesc.text = "";
        bTyping = true;
        foreach(char letter in sentence.sDesc.ToCharArray())
        {
            tDesc.text += letter;
            yield return null;
        }

        bTyping = false;
        _ani.SetInteger("Ani", -1);
        gF.SetActive(true);
        yield return null;
    }

    #endregion


    #region 실행

    private void OnEnable()
    {
        tName.text = "";
        tDesc.text = "";

        sentence = new DialogStruct();
        //sentences = Camera.main.GetComponent<FollowCam>().qSentences;
        sentences = new Queue<DialogStruct>(Camera.main.GetComponent<FollowCam>().qSentences);
        _ani = Camera.main.GetComponent<FollowCam>().aniDialog;

        PlayerController.bDialog = true;
        Camera.main.GetComponent<FollowCam>().bDialog = true;
        StartDialog();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (!bTyping)
            {
                StartDialog();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            EndDialog();
        }
    }

    #endregion
}
