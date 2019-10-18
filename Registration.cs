using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Registration : Interactable
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private InputField NomeCompletoField;
    [SerializeField] private InputField UsernameField;
    [SerializeField] private InputField PasswordField;
    [SerializeField] private InputField DataField;
    [SerializeField] private InputField SessoField;
    [SerializeField] private GameObject Canvas_Successo;
    [SerializeField] private GameObject Canvas_Fallimento;
    [SerializeField] private Canvas Canvas_LogIn;
    [SerializeField] private Setting_Server Server_Info;
    #endregion

    #region Variabili Private
    private Transform Original;
    #endregion

    #endregion

    #region Metodi Pubblici
    public override void Touch() { }

    public override void Click()
    {
        if (Verifica_Campi())
            CallRegister();
    }

    public bool Verifica_Campi()
    {
        bool EsitoData = DataField.GetComponent<DataNascita_Check>().ControlloData();
        bool EsitoSesso = SessoField.GetComponent<Sesso_Check>().Status_Sesso();

        // L'attivazione del Bottone... avviene solo se sono rispettate delle condizioni
        // imposte ( es. i Campi Non sono Vuoti )
        bool Esito = (NomeCompletoField.text.Length >= 5 && PasswordField.text.Length >= 4 &&
                                       UsernameField.text.Length >= 5 && EsitoSesso && EsitoData);

        return Esito;
    }
    #endregion

    #region Metodi Privati
    private void CallRegister()
    {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(Registrazione());
    }

    // Funzione di Registrazione...
    private IEnumerator Registrazione()
    {
        Debug.Log("Registrazione...");

        bool Esito = false;
        int Valore_Rest = -1;

        do
        {
            // Passiamo alla Pagina PHP i dati immessi dall'utente in maniera POST
            WWWForm Post_Dati = new WWWForm();

            Post_Dati.AddField("Username", UsernameField.text);
            Post_Dati.AddField("Password", PasswordField.text);
            Post_Dati.AddField("NomeCompleto", NomeCompletoField.text);
            Post_Dati.AddField("Sesso", SessoField.text);
            Post_Dati.AddField("Anno", DataField.GetComponent<DataNascita_Check>().GetAnno());
            Post_Dati.AddField("Mese", DataField.GetComponent<DataNascita_Check>().GetMese());
            Post_Dati.AddField("Giorno", DataField.GetComponent<DataNascita_Check>().GetGiorno());

            // Andiamo ad aprire la pagina PHP per la richiesta di Registrazione
            WWW PaginaPHP = new WWW(Server_Info.getURL_PHPServer() + "/Register.php", Post_Dati);

            yield return PaginaPHP;

            Debug.Log(PaginaPHP.text);
            Esito = int.TryParse(PaginaPHP.text, out Valore_Rest);

        } while (Esito == false);

        // Andiamo ora a verificare l'esito restituito dalla Pagina PHP per la Registrazione
        if ( Valore_Rest == 0 )
        {
            // Messaggio sulla Console di Debug
            Debug.Log("Registrazione avvenuta con Successo !!!");

            // Attivazione del Canvas di Conferma Corretta Registrazione
            Canvas_Successo.SetActive(true);
            Canvas_Successo.GetComponent<AudioSource>().Play();

        }
        // In caso di Registrazione non avvenuta con Successo...
        else
        {
            // Messaggio sulla Console di Debug
            Debug.Log("Registrazione Fallita... Numero di Errore = " + Valore_Rest);

            // Altre azioni per l'utente.... ( Magari Grafiche e di pulizia dei Campi... )
            
            // Attivazione del Canvas di Fallimento Registrazione 
            Canvas_Fallimento.SetActive(true);
            Canvas_Fallimento.GetComponent<AudioSource>().Play();
        } 
    }
    
    private void PulisciCampi()
    {
        UsernameField.text = "";
        PasswordField.text = "";
        NomeCompletoField.text = "";
        SessoField.text = "";
        DataField.text = "";
    }
    #endregion
}
