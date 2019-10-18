using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Controller_Canvas_Conferma : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Timer tempo;
    [SerializeField] private Change_Speed Speed;
    #endregion

    #region Variabili Private
    private string Path = "";
    private Mapping_Input_Controller.Controller controller = Mapping_Input_Controller.Controller.PS4;
    private string Path_Dati = "";
    private int ID_User = 0;
    #endregion

    #endregion

    #region Metodi Unity
    private void Awake()
    {
        Path = Application.persistentDataPath + "/Controller.txt";
        Path_Dati = Application.persistentDataPath + "/Dati.txt";

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

        Dati = File.ReadAllText(Path_Dati);

        string[] AllData = Dati.Split(char.Parse(" "));

        if (int.TryParse(AllData[0], out ID_User))
            Debug.Log("User ID = " + AllData[0]);
    }

    void Update()
    {
        if (controller == Mapping_Input_Controller.Controller.PS4)
        {
            if (Mapping_Input_Controller.GetKey(controller, "Triangle"))
                StartCoroutine(Salvataggio_Dati());
            else if (Mapping_Input_Controller.GetKey(controller, "Circle"))
            {
                tempo.ResetTimer();
                this.gameObject.SetActive(false);
            }
            
        }
        else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
        {
            if (Mapping_Input_Controller.GetKey(controller, "Y"))
                StartCoroutine(Salvataggio_Dati());
            else if (Mapping_Input_Controller.GetKey(controller, "B"))
            {
                tempo.ResetTimer();
                this.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Salvataggio_Dati()
    {
        // Passiamo alla Pagina PHP per scaricare i dati dello Speed
        WWWForm Post_Dati = new WWWForm();

        string Dati = "" + (int)Speed.getSpeed();
        Post_Dati.AddField("ID", "" + ID_User);
        Post_Dati.AddField("Speed", Dati);

        // Andiamo ad aprire la pagina PHP per la richiesta di Salvataggio Dati
        WWW PaginaPHP = new WWW("https://vistamuseooculus.000webhostapp.com/Salvataggio_Dati.php", Post_Dati);
        yield return PaginaPHP;

        // Se viene Restituito un Valore > 0 --> Salvataggio Avvenuto con successo
        int Valore_Rest = int.Parse(PaginaPHP.text);

        if (Valore_Rest == 0)
        {
            Debug.Log("Salvataggio Dati avvenuto con Successo");

            if (File.Exists(Path))
                File.Delete(Path);

            // Andiamo a creare il File
            File.WriteAllText(Path, "");
            File.WriteAllText(Path, "" + ID_User + " " + Speed.getSpeed());

            // Attivazione Scena della Visita Museale
            SceneManager.LoadScene("Caricamento_Intermedio_1");
        }
        else
        {
            if (Valore_Rest < 0)
                Debug.Log("Salvataggio Dati fallito");
            else
                Debug.Log("Errore Query");
        }
    }
    #endregion
}
