using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Input_Errato : MonoBehaviour
{
    #region Variabili SerializeField
    [Range(3.0f, 6.0f)][SerializeField]
    private float TempoAttesa = 5.0f;
    #endregion

    #region Metodi Unity
    private void OnEnable()
    {
        StartCoroutine(Attivazione());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Attivazione()
    {
        yield return new WaitForSeconds(TempoAttesa);

        this.GetComponent<Animator>().SetTrigger("Fade_Out");
        this.gameObject.SetActive(false);
    }
    #endregion
}
