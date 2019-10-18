using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Change_Speed : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Controller_Speed contSpeed;
    [SerializeField] private Movimento_Utente mov;
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

        string Dati = "";

        if (File.Exists(Path))
        {
            Dati = File.ReadAllText(Path);

            if (Dati.Equals("PS4"))
                controller = Mapping_Input_Controller.Controller.PS4;
            else if (Dati.Equals("Xbox_One"))
                controller = Mapping_Input_Controller.Controller.XBOX_ONE;
        }
        else
            throw new System.Exception("Errore Lettura Scelta Controller");
    }

    private void Start()
    {
        this.GetComponent<Text>().text = "" + contSpeed.getSpeed();
    }

    private void Update()
    {
        if (Input_Enable)
        {
            if (controller == Mapping_Input_Controller.Controller.PS4)
            {
                if (Mapping_Input_Controller.GetKey(controller, "L2"))
                {
                    if( contSpeed.increaseSpeed() )
                    {
                        this.GetComponent<Text>().text = "" + contSpeed.getSpeed();
                        mov.setSpeed(contSpeed.getSpeed());
                        StartCoroutine(Attivazione_Input());
                    }   
                }
                else if (Mapping_Input_Controller.GetKey(controller, "R2"))
                {
                    if( contSpeed.decreaseSpeed() )
                    {
                        this.GetComponent<Text>().text = "" + contSpeed.getSpeed();
                        mov.setSpeed(contSpeed.getSpeed());
                        StartCoroutine(Attivazione_Input());
                    }
                }
            }
            else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
            {
                if (Mapping_Input_Controller.GetKey(controller, "LB"))
                {
                    if (contSpeed.increaseSpeed())
                    {
                        this.GetComponent<Text>().text = "" + contSpeed.getSpeed();
                        StartCoroutine(Attivazione_Input());
                    }
                }
                else if (Mapping_Input_Controller.GetKey(controller, "RB"))
                {
                    if (contSpeed.decreaseSpeed())
                    {
                        this.GetComponent<Text>().text = "" + contSpeed.getSpeed();
                        StartCoroutine(Attivazione_Input());
                    }
                }
            }
        }
    }
    #endregion

    #region Metodi Pubblici
    public int getSpeed()
    {
        return int.Parse(this.GetComponent<Text>().text);
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Attivazione_Input()
    {
        Input_Enable = false;

        yield return new WaitForSeconds(0.5f);

        Input_Enable = true;
    }
    #endregion
}
