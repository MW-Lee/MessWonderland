////////////////////////////////////////////////
//
// LoadManager
//
// 다음 Scene을 불러오는걸 담당하는 Manager
// 
// 20. 04.06
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    #region 변수

    public static LoadManager instance;

    private AsyncOperation async;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        // DataManager에 저장된 불러올 다음 Scene을 호출
        async = SceneManager.LoadSceneAsync(DontDestroy.instance.sNextScene);

        // 로딩이 완료되어도 바로 전환하진 않는다
        async.allowSceneActivation = false;
    }

    private void Update()
    {
        // 로딩의 90%가 되면 화면을 전환한다.
        if (async.progress >= 0.9f)
            async.allowSceneActivation = true;
    }

    #endregion
}
