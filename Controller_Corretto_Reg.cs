using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Corretto_Reg : MonoBehaviour {

    #region Variabili SerializeField
    [SerializeField] private GameObject Canvas_Login;
    [SerializeField] private GameObject Canvas_Reg;
    [Range(3.0f, 6.0f)][SerializeField] private float TempoAttesa = 5.0f;
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
 
        this.gameObject.SetActive(false);
        Canvas_Login.SetActive(true);
        Canvas_Reg.SetActive(false);
        Canvas_Login.GetComponent<Animator>().SetTrigger("Login_FadeIn");
    }
    #endregion
}
