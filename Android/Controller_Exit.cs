using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Controller_Exit : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Canvas Canvas_Exit;
    [SerializeField] private Exit_Visita Exit;
    #endregion

    #region Variabili Private
    private string Path = "";
    private Mapping_Input_Controller.Controller controller = Mapping_Input_Controller.Controller.PS4;
    private bool Input_Enable = true;
    #endregion

    #endregion

    #region Metodi Unity
    private void Awake()
    {
        Path = Application.persistentDataPath + "/Controller.txt";

        if (File.Exists(Path))
        {
            string Dati = File.ReadAllText(Path);

            if (Dati.Equals("PS4"))
                controller = Mapping_Input_Controller.Controller.PS4;
            else if (Dati.Equals("Xbox_One"))
                controller = Mapping_Input_Controller.Controller.XBOX_ONE;
        }
        else
            throw new System.Exception("Errore Lettura Scelta Controller");
    }

    private void Update () 
    {
        if (Input_Enable)
        {
            if (controller == Mapping_Input_Controller.Controller.PS4)
            {
                if (Mapping_Input_Controller.GetKey(controller, "Triangle"))
                {
                    Input_Enable = false;
                    Canvas_Exit.gameObject.SetActive(true);

                    StartCoroutine(Attivazione_Input_Exit());
                }
                
            }
            else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
            {
                if (Mapping_Input_Controller.GetKey(controller, "Y"))
                {
                    Input_Enable = false;
                    Canvas_Exit.gameObject.SetActive(true);

                    StartCoroutine(Attivazione_Input_Exit());
                }
            }
        }
    }
    #endregion

    #region Metodi Pubblici
    public void Change_Status_Exit(bool Esito)
    {
        Input_Enable = Esito;
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Attivazione_Input_Exit()
    {
        yield return new WaitForSeconds(1.0f);

        Exit.setInput(true);
    }
    #endregion
}
