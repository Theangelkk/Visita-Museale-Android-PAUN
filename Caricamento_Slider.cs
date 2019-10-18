using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Threading.Tasks;
using System.Threading;

public class Caricamento_Slider : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private string NomeScena;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private RawImage rawImage;
    #endregion

    #region Variabili Private
    private Task Caricamento_Scena = null;
    private bool Esito = false;
    private bool Blocco = false;
    private AsyncOperation async;
    #endregion

    #endregion

    #region Metodi Unity
    private void OnEnable()
    {
        Caricamento_Scena = new Task(() => Attesa());
        Caricamento_Scena.Start();
    }

    private void FixedUpdate()
    {
        if( Esito )
        {
            Esito = false;
            rawImage.color = Color.black;
            StartCoroutine(LoadingScene());
        }

        if( Blocco )
            rawImage.color = Color.black;
        else
            StartCoroutine(CaricamentoVideo());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator LoadingScene()
    {
        async = SceneManager.LoadSceneAsync(NomeScena);
        async.allowSceneActivation = false;

        yield return new WaitForSeconds(1.5f);

        Blocco = true;
        async.allowSceneActivation = true;
    }

    private void Attesa()
    {
        Thread.Sleep(13000);
        Esito = true;
    }

    private IEnumerator CaricamentoVideo()
    {
        video.Prepare();

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        while (!video.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        rawImage.color = Color.white;
        rawImage.texture = video.texture;
        video.Play();
    }
    #endregion
}
