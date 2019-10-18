using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Button_Language : Interactable {

    public enum Language
    {
        inglese,
        italiano
    };

    #region Variabili SerializeField
    [SerializeField] private Language Lingua;
    [SerializeField] private float TempoAttesaIniziale = 1.0f;
    [SerializeField] private string Messaggio_Vocale;
    [SerializeField] private Text Text_Input;
    [SerializeField] private string Nome_Scena_Next;
    #endregion

    #region Variabili Private
    private bool Eseguito = false;
    #endregion

    #region Metodi Pubblici
    public override void Touch()
    {   }

    public override void Click()
    {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(Attivazione_Evento());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Attivazione_Evento()
    {
        Attiva_Audio();

        yield return new WaitForSeconds(5.0f);

        string Path = Application.persistentDataPath + "/Lingua.txt";

        if (File.Exists(Path))
            File.Delete(Path);
      
        // Andiamo a creare il File
        File.WriteAllText(Path, this.name );
        SceneManager.LoadScene(Nome_Scena_Next);
    }

    private void Attiva_Audio()
    {
        if (Eseguito == false)
            StartCoroutine(DonwloadAudio());
    }

    private IEnumerator DonwloadAudio()
    {
        yield return new WaitForSeconds(TempoAttesaIniziale);

        Regex rgx = new Regex("\\s+");
        string result = rgx.Replace(Messaggio_Vocale, "+");

        string url = "";

        if (Lingua == Language.inglese)
            url = "http://api.voicerss.org/?key=62adb0e343344397b1af44da5422a798&hl=en-us&src=" + result;
        else if (Lingua == Language.italiano)
            url = "http://api.voicerss.org/?key=62adb0e343344397b1af44da5422a798&hl=it-it&src=" + result;
        

        WWW sito = new WWW(url);
        yield return sito;

        Text_Input.GetComponent<AudioSource>().clip = sito.GetAudioClip(false, true, AudioType.WAV);
        Text_Input.GetComponent<AudioSource>().Play();
    
        Eseguito = true;
    }
    #endregion
}
