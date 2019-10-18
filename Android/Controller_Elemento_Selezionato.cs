using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Controller_Elemento_Selezionato : MonoBehaviour
{
    #region Variabili Private
    private GameObject obj = null;
    private string Path = "";
    private Mapping_Input_Controller.Controller controller = Mapping_Input_Controller.Controller.PS4;
    private bool Input_Enable = true;
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

    private void Update()
    {
        if (Input_Enable)
        {
            if (controller == Mapping_Input_Controller.Controller.PS4)
            {
                if (Mapping_Input_Controller.GetKey(controller, "X"))
                {
                    if (obj != null)
                    {
                        Input_Enable = false;
                        StartCoroutine(Abilita_Input());
                        obj.GetComponent<Interactable>().Click();
                    }
                }
            }
            else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
            {
                if (Mapping_Input_Controller.GetKey(controller, "A"))
                    if (obj != null)
                    {
                        Input_Enable = false;
                        StartCoroutine(Abilita_Input());
                        obj.GetComponent<Interactable>().Click();
                    }
            }
        }
    }
    #endregion

    #region Metodi Pubblici
    public void setOggetto(GameObject elem)
    {
        obj = elem;
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Abilita_Input()
    {
        yield return new WaitForSeconds(1.0f);

        Input_Enable = true;
    }
    #endregion
}

