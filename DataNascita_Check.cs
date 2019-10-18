using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataNascita_Check : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private InputField DataField;
    #endregion

    #region Variabili Private
    private string Anno = "";
    private string Mese = "";
    private string Giorno = "";
    #endregion

    #endregion

    #region Metodi Pubblici
    public bool ControlloData()
    {
        // Andiamo a prendere i 3 Valori ( Anno - Mese - Giorno )
        string[] Data = DataField.text.Split(char.Parse("/"));

        if (Data.Length < 3)
            return false;

        // Andiamo a verificare la presenza di eventuali lettere...
        System.DateTime DataClass;

        if (System.DateTime.TryParse(DataField.text, out DataClass))
        {
            Anno = Data[0];
            Mese = Data[1];
            Giorno = Data[2];

            return true;
        }
        else
            return false;
    }

    public string GetAnno()
    {
        return Anno;
    }

    public string GetMese()
    {
        return Mese;
    }

    public string GetGiorno()
    {
        return Giorno;
    }
    #endregion
}
