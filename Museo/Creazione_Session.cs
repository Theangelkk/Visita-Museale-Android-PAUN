using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System;

public class Creazione_Session : MonoBehaviour 
{
    #region  Variabili Locali

    #region Variabili SerializeField    
    [SerializeField] private Setting_Server Server_Info;
    #endregion

    #region Variabili Private
    private string Path;
    private string ID_User = "-1";
    private int ID_Sessione = 0;
    #endregion
    #endregion

    #region Metodi Unity 
    private void Start()
    {
        Path = Application.persistentDataPath + "/Dati.txt";

        if (!File.Exists(Path))
            throw new Exception("Nessun File Utente trovato");

        string Data = File.ReadAllText(Path);
        string[] AllData = Data.Split(char.Parse(" "));

        if (AllData.Length >= 1)
            ID_User = AllData[0];
        else
            throw new Exception("Dati File Login Mancanti");

        StartCoroutine(Scaricamento_Dati());
    }
    #endregion

    #region Metodi Getter
    public int getID_Utente()
    {
        return int.Parse(ID_User);
    }

    public int getID_Sessione()
    {
        return ID_Sessione;
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Scaricamento_Dati()
    {
        bool Esito_PHP = false;
        int Valore_Esito = 0;

        do
        {
            WWW PaginaPHP = null;

            // Passiamo alla Pagina PHP i dati immessi dall'utente in maniera POST
            WWWForm Post_Dati = new WWWForm();

            Post_Dati.AddField("ID_USER", ID_User);

            // Andiamo ad aprire la pagina PHP per la richiesta di Login
            PaginaPHP = new WWW(Server_Info.getURL_PHPServer() + "/Creazione_Sessione.php", Post_Dati);

            yield return PaginaPHP;

            Esito_PHP = int.TryParse(PaginaPHP.text, out Valore_Esito);

        } while ( Esito_PHP == false );

        Debug.Log("Esito Creazione Sessione = " + Valore_Esito);

        if (Valore_Esito >= 0)
        {
            Debug.Log("Creazione Sessione Avvenuta con Successo");
            ID_Sessione = Valore_Esito;
        }
        else
            throw new Exception("Errore Creazione Sessione DataBase");
    }
    #endregion
}
