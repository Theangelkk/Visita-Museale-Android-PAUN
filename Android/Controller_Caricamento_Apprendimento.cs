using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Caricamento_Apprendimento : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private string NomeScena;
    #endregion

    #region Variabili Private
    private AsyncOperation async;
    #endregion
    #endregion

    #region Metodi Unity
    private void Start()
    {
        StartCoroutine(LoadingScene());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(1.0f);

        async = SceneManager.LoadSceneAsync(NomeScena);
        async.allowSceneActivation = false;

        while (async.isDone == false)
        {
            if (async.progress == 0.9f)
                async.allowSceneActivation = true;

            yield return null;
        }
    }
    #endregion
}
