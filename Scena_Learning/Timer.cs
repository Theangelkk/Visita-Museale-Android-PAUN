using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [Range(30.0f, 180.0f)][SerializeField] private float Tempo_Timer;
    [SerializeField] private Text Testo_TempoRim;
    [SerializeField] private Canvas Canvas_Conferma;
    #endregion

    #region Variabili Private
    private float Tempo_Rimanente = 0.0f;
    private bool canCount = true;
    private bool doOnce = false;
    private Color DefaultColor;
    private bool TimerAttivo = true;
    #endregion

    #endregion

    #region Metodi Unity
    private void Start ()
    {
        Tempo_Rimanente = Tempo_Timer;
        DefaultColor = Testo_TempoRim.color;
	}

	private void Update ()
    {
        if (TimerAttivo)
        {
            if (Tempo_Rimanente >= 0.0f && canCount)
            {
                Tempo_Rimanente -= Time.deltaTime;
                int Value_Show = (int)Tempo_Rimanente;
                Testo_TempoRim.text = "" + Value_Show;
            }
            else if (Tempo_Rimanente <= 0.0f && !doOnce)
            {
                canCount = false;
                doOnce = true;
                Testo_TempoRim.text = "0";
                Tempo_Rimanente = 0.0f;

                Canvas_Conferma.gameObject.SetActive(true);
            }

            if (Tempo_Rimanente <= 20.0f && Tempo_Rimanente >= 10.0f)
                Testo_TempoRim.color = new Color(0.0f, 0.0f, 255.0f, 1.0f);
            else if (Tempo_Rimanente < 10.0f)
                Testo_TempoRim.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
        }
    }
    #endregion

    #region Metodi Pubblici
    public void ResetTimer()
    {
        Tempo_Rimanente = Tempo_Timer;
        canCount = true;
        doOnce = false;
        Testo_TempoRim.color = DefaultColor;
    }

    public void Status_Timer( bool Status )
    {
        TimerAttivo = Status;
    }
    #endregion
}
