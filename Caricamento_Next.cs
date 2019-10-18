using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Caricamento_Next : MonoBehaviour
{
    #region Variabili SerializeField
    [Range(3.0f,9.0f)][SerializeField]
    private float TempoAttesa = 5.0f;
    #endregion

    #region Metodi Unity
    private void Start()
    {
        StartCoroutine(Caricamento_Scena());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Caricamento_Scena()
    {
        yield return new WaitForSeconds(TempoAttesa);
        SceneManager.LoadScene("Scelta_Controller");
    }
    #endregion
}
