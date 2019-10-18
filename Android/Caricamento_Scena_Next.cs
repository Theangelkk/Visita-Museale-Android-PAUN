using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Caricamento_Scena_Next : MonoBehaviour
{
    #region Variabili 
    [SerializeField] private string NomeScena;
    #endregion

    #region Metodi Unity
    private void Start()
    {
        StartCoroutine(Attivazione());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Attivazione()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(NomeScena);
    }
    #endregion
}
