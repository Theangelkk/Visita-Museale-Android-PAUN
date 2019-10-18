using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign_In_Register : Interactable
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private GameObject Canvas_Registrazione;
    [SerializeField] private GameObject Canvas_Login;
    #endregion

    #region Variabili Private
    private Transform Original;
    #endregion

    #endregion

    #region Metodi Unity
    private void Start()
    {
        Canvas_Registrazione.SetActive(false);
    }
    #endregion

    #region Metodi Pubblici
    public override void Touch() { }

    public override void Click()
    {
        this.GetComponent<AudioSource>().Play();

        Canvas_Login.SetActive(false);
        Canvas_Registrazione.SetActive(true);
    }
    #endregion
}
