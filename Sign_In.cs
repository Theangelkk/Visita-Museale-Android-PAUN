using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Sign_In : Interactable
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private InputField Username;
    [SerializeField] private InputField Password;
    [SerializeField] private Button LoginButton;
    [SerializeField] private GameObject Canvas_Input_Non_Corretto;
    [SerializeField] private GameObject Canvas_Input_Corretto;
    [SerializeField] private GameObject Canvas_Utente_Exist;
    [SerializeField] private Setting_Server Server_Info;
    #endregion

    #region Variabili Private
    private Transform Original;
    private string Path;
    private string Path_Username;
    private WWW PaginaPHP = null;
    private string[] AllData = null;
    #endregion

    #endregion

    #region Metodi Unity
    private void Start()
    {
        Path = Application.persistentDataPath + "/Dati.txt";
        Path_Username = Application.persistentDataPath + "/Username.txt";

        if ( File.Exists(Path) )
            File.Delete(Path);

        if (File.Exists(Path_Username))
            File.Delete(Path_Username);

        // Andiamo a creare il File
        File.WriteAllText(Path, "");

        // Andiamo a creare il File
        File.WriteAllText(Path_Username, "");
    }
    #endregion

    #region Metodi Pubblici
    public override void Touch() { }

    public override void Click()
    {
        if( Verifica_Campi() )
            CallLogin();
    }

    public void Login_Avvenuto()
    {
        Debug.Log("Login avvenuto con Successo");

        string NomeCompletoUtente = AllData[2];

        for (int i = 3; i < AllData.Length; i++)
            NomeCompletoUtente += " " + AllData[i];

        Debug.Log("Nome Completo Utente = " + NomeCompletoUtente);

        File.WriteAllText(Path, PaginaPHP.text);
        File.WriteAllText(Path_Username, Username.text + " " + NomeCompletoUtente);

        // Attivazione di un Canvas per Conferma del Login
        Canvas_Input_Corretto.SetActive(true);
        Canvas_Input_Corretto.GetComponent<Audio_Script>().setTextInglese("Login Correctly, Welcome " + NomeCompletoUtente);
        Canvas_Input_Corretto.GetComponent<Audio_Script>().setTextItaliano("Login effettuato Correttamente, Benvenuto " + NomeCompletoUtente);
        Canvas_Input_Corretto.GetComponent<AudioSource>().Play();
    }

    public void Login_Fallito(int Id_Number)
    {
        if (Id_Number == 0)
        {
            Debug.Log("Utente e/o Password Non Corretti");

            // Attivazione di un Canvas per Errore di Immissione Dati...
            Canvas_Input_Non_Corretto.SetActive(true);
            Canvas_Input_Non_Corretto.GetComponent<Animator>().SetTrigger("Fade_In");
            Canvas_Input_Non_Corretto.GetComponent<AudioSource>().Play();

            PulisciCampi();
        }
        // Utente già presente all'interno della Scena
        else if (Id_Number == -10)
        {
            Debug.Log("Utente già Presente all'interno della Scena");

            // Attivazione di un Canvas per Utente già all'interno della Scena
            Canvas_Utente_Exist.SetActive(true);
            Canvas_Utente_Exist.GetComponent<AudioSource>().Play();
        }
        else
            Debug.Log("Errore Query");
    }

    public bool Verifica_Campi()
    {
        // L'attivazione del Bottone... avviene solo se sono rispettate delle condizioni
        // imposte ( es. i Campi Non sono Vuoti )
        bool Esito = (Password.text.Length >= 4 && Username.text.Length >= 5);

        return Esito;
    }
    #endregion

    #region Metodi Privati
    private void CallLogin()
    {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        bool Esito = false;
        int Id_Number = -1;

        do
        {
            // Passiamo alla Pagina PHP i dati immessi dall'utente in maniera POST
            WWWForm Post_Dati = new WWWForm();

            Post_Dati.AddField("Username", Username.text);
            Post_Dati.AddField("Password", Password.text);

            // Andiamo ad aprire la pagina PHP per la richiesta di Login
            PaginaPHP = new WWW(Server_Info.getURL_PHPServer() + "/Login.php", Post_Dati);

            yield return PaginaPHP;

            Debug.Log(PaginaPHP.text);

            // Se viene Restituito un Valore > 0 --> ID Utente
            AllData = PaginaPHP.text.Split(char.Parse(" "));
            Esito = int.TryParse(AllData[0], out Id_Number);

        } while (Esito == false);

        Debug.Log("Id = " + Id_Number);

        if (Id_Number > 0)
            // Controllo Utente se è al'interno della Scena...
            Network_AWS_Manager.instance.CommandControllerUser(Username.text);
        else
            Login_Fallito(Id_Number);
    }

    private void PulisciCampi()
    {
        Username.text = "";
        Password.text = "";
    }
    #endregion
}
