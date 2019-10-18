using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class Audio_Script : MonoBehaviour {

    public enum Language
    {
        inglese,
        italiano
    };

    #region Variabili SerializeField
    [SerializeField] private Language Lingua;
    [SerializeField] private float TempoAttesaIniziale = 1.0f;
    [SerializeField] private string Messaggio_Vocale_Inglese;
    [SerializeField] private string Messaggio_Vocale_Italiano;
    [SerializeField] private Text Text_Input;
    [SerializeField] private bool Repeat = true;
    #endregion

    #region Variabili Private
    private bool Eseguito = false;
    #endregion

    #region Metodi Unity
    private void Start () 
    {
        string Path = Application.persistentDataPath + "/Lingua.txt";

        if (File.Exists(Path))
        {
            string Dati = File.ReadAllText(Path);

            if (Dati == "Italiano")
                Lingua = Language.italiano;
            else if (Dati == "Inglese")
                Lingua = Language.inglese;
        }
        else
            Lingua = Language.inglese;

        Attiva_Audio();
    }

    private void OnEnable()
    {
        if( Repeat )
            Attiva_Audio();
    }
    #endregion

    #region Metodi Pubblici
    public void setTextItaliano(string testo)
    {
        Messaggio_Vocale_Italiano = testo;
    }

    public void setTextInglese(string testo)
    {
        Messaggio_Vocale_Inglese = testo;
    }
    #endregion

    #region Metodi Privati
    private void Attiva_Audio()
    {
        if (Eseguito == false)
            StartCoroutine(DonwloadAudio());
        else
            Text_Input.GetComponent<AudioSource>().Play();
    }
    
    private IEnumerator DonwloadAudio()
    {
        yield return new WaitForSeconds(TempoAttesaIniziale);

        Regex rgx = new Regex("\\s+");
        string result;

        string url = "";

        if (Lingua == Language.inglese)
        {
            result = rgx.Replace(Messaggio_Vocale_Inglese, "+");
            url = "http://api.voicerss.org/?key=62adb0e343344397b1af44da5422a798&hl=en-us&src=" + result;
        }
        else if (Lingua == Language.italiano)
        {
            result = rgx.Replace(Messaggio_Vocale_Italiano, "+");
            url = "http://api.voicerss.org/?key=62adb0e343344397b1af44da5422a798&hl=it-it&src=" + result;
        }

        WWW sito = new WWW(url);
        yield return sito;

        Text_Input.GetComponent<AudioSource>().clip = sito.GetAudioClip(false, true, AudioType.WAV);
        Text_Input.GetComponent<AudioSource>().Play();

        Eseguito = true;
    }
    #endregion
}
