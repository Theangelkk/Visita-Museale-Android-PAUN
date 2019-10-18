using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sesso_Check : MonoBehaviour {

    #region Variabili SerializeField
    [SerializeField] private InputField SessoField;
    #endregion

    #region Metodi Pubblici
    public void CheckSesso()
    {
        if (SessoField.text != "M" && SessoField.text != "F")
            SessoField.text = "";
    }

    public bool Status_Sesso()
    {
        if (SessoField.text != "M" && SessoField.text != "F")
            return false;
        else
            return true;
    }
    #endregion
}
