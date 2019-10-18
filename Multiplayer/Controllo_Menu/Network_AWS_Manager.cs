using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.IO;
using System.Globalization;

public class Network_AWS_Manager : MonoBehaviour {

    public static Network_AWS_Manager instance = null;

    #region Variabili SerializeField
    [SerializeField] private SocketIOComponent socket;
    [SerializeField] private Sign_In Login;
    #endregion

    #region Metodi Unity
    private void Awake()
    {
        // Se non è stata già creata una Istanza del NetworkManager
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
      
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Listener per il controllo Utente
        socket.On("Esito_check_user", OnCheck);
    }
    #endregion

    #region Commands
    public void CommandControllerUser(string email)
    {
        string data = JsonUtility.ToJson(new UserJSON(email));
        socket.Emit("check_user", new JSONObject(data));
    }
    #endregion

    #region JSONMessageClasses
    [System.Serializable]
    private class UserJSON
    {
        public string email;

        // Costruttore
        public UserJSON(string _email)
        {
            email = _email;
        }
    }

    [System.Serializable]
    private class EsitoRequestJSON
    {
        public bool Esito;

        // Costruttore
        public EsitoRequestJSON(bool _esito)
        {
            Esito = _esito;
        }
    }
    #endregion

    #region Listening
    private void OnCheck(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();

        Debug.Log("Esito JSON = " + data);

        EsitoRequestJSON Risposta = JsonUtility.FromJson<EsitoRequestJSON>(data);

        Debug.Log("Valore Esito = " + Risposta.Esito);

        if( Risposta.Esito )
            // Dobbiamo dire al Login che l'utente è già presente nella Scena
            Login.Login_Fallito(-10);
        
        else
            // Richiama il metodo di Login della Scena
            Login.Login_Avvenuto();
        
    }
    #endregion
}
