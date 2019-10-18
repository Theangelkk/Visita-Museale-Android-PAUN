using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Vocal : MonoBehaviour {

    public enum Language
    {
        Inglese,
        Italiano
    };

    [SerializeField] private float TempoAttesaIniziale = 1.0f;
    [SerializeField] private string Testo_Italiano;
    [SerializeField] private string Testo_Inglese;
    [SerializeField] private Language Linguaggio = Language.Inglese;
    [SerializeField] private TextToSpeechTest3 Speak;

    private bool Stato = false;

	// Use this for initialization
	private void Start ()
    {   }

    public void StartSpeak()
    {
        StartCoroutine(NowSpeak());
    }

    private IEnumerator NowSpeak()
    {
        yield return new WaitForSeconds(TempoAttesaIniziale);

        if (Linguaggio == Language.Inglese)
            Speak.StartTTS(Testo_Inglese);
        else
            Speak.StartTTS(Testo_Italiano);

        Stato = true;
    }

    public void setTesto( Controller_Vocal.Language Lingua, string Testo )
    {
        if (Lingua == Controller_Vocal.Language.Inglese)
            Testo_Inglese = Testo;
        else if (Lingua == Controller_Vocal.Language.Italiano)
            Testo_Italiano = Testo;
    }

    public void setTempoAttesaIniziale( float Tempo )
    {
        TempoAttesaIniziale = Tempo;
    }

    public void Messaggio_Terminato()
    {
        Stato = false;
    }

    public bool Stato_Vocale()
    {
        return Stato;
    }

    public void setLanguage( Controller_Vocal.Language Lingua )
    {
        Linguaggio = Lingua;

        if (Linguaggio == Controller_Vocal.Language.Inglese)
            Speak.ChangeLanguage("en");
        else if (Linguaggio == Controller_Vocal.Language.Italiano)
            Speak.ChangeLanguage("it");
    }
}
