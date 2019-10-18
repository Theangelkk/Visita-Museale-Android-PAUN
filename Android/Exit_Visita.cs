using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Exit_Visita : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Setting_Server Server_Info;
    [SerializeField] private Controller_Tempo Controller;
    [SerializeField] private Creazione_Session Sessione;
    [SerializeField] private Controller_Exit cont;
    #endregion

    #region Variabili Private
    private string Path = "";
    private Mapping_Input_Controller.Controller controller = Mapping_Input_Controller.Controller.PS4;
    private bool Input_Enable = false;
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
                    Scaricamento_Sessione();
                else if (Mapping_Input_Controller.GetKey(controller, "Circle"))
                {
                    cont.Change_Status_Exit(true);
                    this.gameObject.SetActive(false);
                }
            }
            else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
            {
                if (Mapping_Input_Controller.GetKey(controller, "Y"))
                    Scaricamento_Sessione();
                else if (Mapping_Input_Controller.GetKey(controller, "B"))
                {
                    cont.Change_Status_Exit(true);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    #region Metodi Pubblici
    public void setInput( bool Esito )
    {
        Input_Enable = Esito;
    }
    #endregion

    #region Metodi Privati
    private void Scaricamento_Sessione()
    {
        StartCoroutine(Scaricamento());
    }

    private IEnumerator Scaricamento()
    {
        Debug.Log("Iniziamo il Salvataggio");

        yield return new WaitForSeconds(1.0f);

        int ID_User = 0;
        string Time = "";

        Debug.Log(Time);

        int Valore_Rest = 0;
        bool Esito_PHP = false;

        do
        {
            ID_User = Sessione.getID_Sessione();

            if (ID_User != -1)
            {
                Time = "" + (int)(Controller.getTempo() / 60);

                WWWForm Post_Dati = new WWWForm();
                Post_Dati.AddField("ID_Session", "" + ID_User);
                Post_Dati.AddField("Time", Time);

                WWW PaginaPHP = new WWW(Server_Info.getURL_PHPServer() + "/Salvataggio_Tempo.php", Post_Dati);
                yield return PaginaPHP;

                Debug.Log(PaginaPHP.text);

                Esito_PHP = int.TryParse(PaginaPHP.text, out Valore_Rest);
            }
            else
                Esito_PHP = false;

        } while (Esito_PHP == false);

        Debug.Log("Valore Restituito = " + Valore_Rest);

        if (Valore_Rest == 0)
        {
            Debug.Log("salvataggio Tempo Avvenuto con Successo");
            Chiusura();
        }
        else
            Debug.Log("Errore Scaricamento Sessione");
    }

    private void Chiusura()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    #endregion
}
