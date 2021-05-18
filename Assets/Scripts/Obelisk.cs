////////////////////////////////////////////////
//
// Obelisk
//
// The First puzzle of Stage_1
// 
// 20. 04. 20
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    #region 변수

    public Transform[] TFArray;

    public bool bAction;
    public bool bFirst;
    public bool bTurning;

    public float[] fDest;
    public Material[] mtBasic;
    public Material[] mtLight;

    public Obelisk_All ParentObelisk;

    //private float fTime;

    public Quaternion[] qDestArray;

    private Vector3 v360;
    private Transform TFParent;
    public Transform TFButton;
    private Transform TFWall;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void SettingDest()
    {
        //qDest1 = Quaternion.Euler(0, TFBox1.eulerAngles.y + 90, 0);
        //qDest2 = Quaternion.Euler(0, TFBox2.eulerAngles.y + 90, 0);

        for(int i = 0; i < qDestArray.Length; i++)
        {
            qDestArray[i] = Quaternion.Euler(0, TFArray[i].eulerAngles.y + 90, 0);
        }

        bFirst = false;
    }

    public void Turning()
    {
        //fTime += Time.deltaTime;

        int temp = 0;

        for(int i = 0; i < TFArray.Length; i++)
        {
            if (qDestArray[i].eulerAngles == v360 ?
                (TFArray[i].eulerAngles.y <= 359) : 
                (TFArray[i].eulerAngles.y < qDestArray[i].eulerAngles.y))
                temp++;
        }

        if(temp == TFArray.Length)
        //if (qDest1.eulerAngles == v360 ? (TFBox1.eulerAngles.y < 359) :
        //    (TFBox1.eulerAngles.y < qDest1.eulerAngles.y)
        //    &&
        //    qDest2.eulerAngles == v360 ? (TFBox2.eulerAngles.y < 359) :
        //    (TFBox2.eulerAngles.y < qDest2.eulerAngles.y))
        {
            //TFBox1.Rotate(Vector3.up);
            //TFBox2.Rotate(Vector3.up);
            
            for(int i = 0; i < TFArray.Length; i++)
            {
                TFArray[i].Rotate(Vector3.forward);
            }
        }
        else
        {            
            for (int i = 0; i < TFArray.Length; i++)
            {
                //TFArray[i].eulerAngles = qDestArray[i].eulerAngles;
                TFArray[i].eulerAngles = new Vector3(
                    TFArray[i].eulerAngles.x,
                    qDestArray[i].eulerAngles.y,
                    TFArray[i].eulerAngles.z);

                if(TFArray[i].eulerAngles.y % 90 != 0)
                {
                    TFArray[i].eulerAngles = new Vector3(
                        TFArray[i].eulerAngles.x,
                        (TFArray[i].eulerAngles.y / 90) * 90, 
                        TFArray[i].eulerAngles.z);
                }
            }

            if (CheckClear())
            {
                ParentObelisk.transform.Find("ClearCam").gameObject.SetActive(true);

                IngameManager.instance.ClearStage();                
            }

            bAction = false;
            bTurning = false;
        }
    }

    public void ActiveCollider()
    {
        bFirst = true;
        bAction = true;
        bTurning = true;
    }

    public bool CheckTurning()
    {
        if(TFButton.name == "ButtonBox")
        {
            for (int i = 0; i < TFButton.childCount; i++)
            {
                if (TFButton.GetChild(i).GetChild(0).GetComponent<Obelisk>().bTurning)
                    return true;
            }
        }
        else
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (TFButton.GetChild(i).GetComponent<Obelisk>().bTurning)
                    return true;
            }
        }

        return false;
    }

    public bool CheckClear()
    {
        int result = 0;

        for(int i = 0; i < TFParent.childCount; i++)
        {
            //if (TFParent.GetChild(i).eulerAngles.y == 90)
            if (TFParent.GetChild(i).eulerAngles.y == fDest[i])
            {
                result++;
                TFWall.GetChild(i).GetComponent<Renderer>().material = mtLight[i];
            }
            else
            {
                TFWall.GetChild(i).GetComponent<Renderer>().material = mtBasic[i];
            }


            if (i + 1 == TFParent.childCount && result == TFParent.childCount)
                return true;               
        }

        return false;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        bAction = false;

        //fTime = 0;

        //qDest1 = new Quaternion();
        //qDest2 = new Quaternion();

        qDestArray = new Quaternion[TFArray.Length];

        v360 = Quaternion.Euler(0, 360, 0).eulerAngles;

        //TFParent = transform.parent.Find("TurningBox");
        //TFWall = transform.parent.Find("Light");
        //fDest = transform.parent.parent.GetComponent<Obelisk_All>().fDest;
        //mtBasic = transform.parent.parent.GetComponent<Obelisk_All>().mtBasic;
        //mtLight = transform.parent.parent.GetComponent<Obelisk_All>().mtLight;

        TFParent = ParentObelisk.transform.Find("TurningBox");
        TFWall = ParentObelisk.transform.Find("Light");
        fDest = ParentObelisk.fDest;
        mtBasic = ParentObelisk.mtBasic;
        mtLight = ParentObelisk.mtLight;
    }

    private void FixedUpdate()
    {
        if (bFirst && bAction)
        {
            SettingDest();
        }
        else if (!bFirst && bAction)
        {
            Turning();
        }
    }

    #endregion
}
