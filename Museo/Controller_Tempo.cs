using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Tempo : MonoBehaviour {

    #region  Variabili Locali
    private float Tempo_Visita = 0.0f;
    #endregion

    #region Metodi Unity
    private void Start ()
    {
        Tempo_Visita = 0.0f;
	}
	
	private void Update ()
    {
        Tempo_Visita += Time.deltaTime;
	}
    #endregion

    #region Metodi Getter
    public float getTempo()
    {
        return Tempo_Visita;
    }
    #endregion
}
