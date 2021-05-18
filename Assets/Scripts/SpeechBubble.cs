using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    #region 변수

    public Transform TFTarget;

    public GameObject[] goBubbles;

    private Transform TF;

    #endregion


    #region 함수

    private void RefreshTarget()
    {
        TF.LookAt(new Vector3(TFTarget.position.x, transform.position.y, TFTarget.position.z));
        //TF.Rotate(Vector3.up, 90);
    }

    public IEnumerator ShowBubble()
    {
        goBubbles[0].SetActive(true);
        yield return new WaitForSeconds(.2f);

        goBubbles[1].SetActive(true);
        yield return new WaitForSeconds(.2f);

        goBubbles[2].SetActive(true);
        yield return new WaitForSeconds(3);

        for (int i = 0; i < 3; i++)
            goBubbles[i].SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }

    #endregion


    #region 실행

    private void Start()
    {
        TF = this.transform;        
    }

    private void OnEnable()
    {
        StartCoroutine("ShowBubble");
    }

    private void Update()
    {
        RefreshTarget();
    }

    #endregion
}
