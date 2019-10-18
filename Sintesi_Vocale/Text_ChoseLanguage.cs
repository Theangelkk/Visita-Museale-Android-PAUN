using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Text_ChoseLanguage : MonoBehaviour {

    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Text Testo;
    [SerializeField] private string Testo_Inglese;
    [SerializeField] private string Testo_Italiano;
    #endregion

    #region Variabili Private
    private string Dati = "";
    #endregion

    #endregion

    #region Metodi Unity
    private void Start ()
    {
        string Path = Application.persistentDataPath + "/Lingua.txt";

        if (File.Exists(Path))
        {
            Dati = File.ReadAllText(Path);

            if (Dati == "Italiano")
                Testo.text = Testo_Italiano;
            else
                Testo.text = Testo_Inglese;
        }
        else
            Testo.text = Testo_Inglese;
    }
    #endregion

    #region Metodi Pubblici
    public void setTextItaliano(string testo)
    {
        Testo_Italiano = testo;
        setText();
    }

    public void setTextInglese(string testo)
    {
        Testo_Inglese = testo;
        setText();
    }

    public void setText()
    {
        if (Dati == "Italiano")
            Testo.text = Testo_Italiano;
        else
            Testo.text = Testo_Inglese;
    }
    #endregion
}
