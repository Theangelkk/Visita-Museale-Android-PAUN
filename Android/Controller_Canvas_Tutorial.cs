using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller_Canvas_Tutorial : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private List<Canvas> Canvas_Tutorial;
    #endregion

    #region Variabili Private
    private string Path = "";
    private Mapping_Input_Controller.Controller controller = Mapping_Input_Controller.Controller.PS4;
    private bool Enable_Input = true;
    private int i = 0;
    private Mapping_Input_Controller.Controller pp = new Mapping_Input_Controller.Controller();
    #endregion

    #endregion

    #region Metodi Unity
    private void Awake()
    { 
        Path = Application.persistentDataPath + "/Controller.txt";

        if (File.Exists(Path))
        {
            string Dati = File.ReadAllText(Path);

            Debug.Log(Dati);

            if (Dati.Equals("PS4"))
                controller = Mapping_Input_Controller.Controller.PS4;
            else if (Dati.Equals("Xbox_One"))
                controller = Mapping_Input_Controller.Controller.XBOX_ONE;

            Canvas_Tutorial[0].gameObject.SetActive(true);
            Canvas_Tutorial[0].gameObject.GetComponent<Animator>().SetTrigger("Fade_In");

        }
        else
            throw new System.Exception("Errore Lettura Scelta Controller");
    }

    private void Update()
    {
        if( Enable_Input )
        {
            if (controller == Mapping_Input_Controller.Controller.PS4)
            {
                if (Mapping_Input_Controller.GetKey(controller, "X"))
                {
                    Cambia_Canvas();
                    StartCoroutine(Disabilita_Input());
                }
            }
            else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
            {
                if (Mapping_Input_Controller.GetKey(controller, "A"))
                {
                    Cambia_Canvas();
                    StartCoroutine(Disabilita_Input());
                }
            }
        }
    }
    #endregion

    #region Metodi Privati
    private void Cambia_Canvas()
    {
        if( i+1 < Canvas_Tutorial.Count )
        {
            Canvas_Tutorial[i].gameObject.GetComponent<Animator>().SetTrigger("Fade_Out");
            Canvas_Tutorial[i].gameObject.SetActive(false);

            Canvas_Tutorial[i + 1].gameObject.SetActive(true);
            Canvas_Tutorial[i + 1].gameObject.GetComponent<Animator>().SetTrigger("Fade_In");

            i++;
        }
        else
            SceneManager.LoadScene("Caricamento_Apprendimento");
        
    }

    private IEnumerator Disabilita_Input()
    {
        Enable_Input = false;

        yield return new WaitForSeconds(2.0f);

        Enable_Input = true;
    }
    #endregion
}
