using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

public class NetworkManager : MonoBehaviour {

    #region Variabili Locali

    public static NetworkManager instance = null;

    #region Variabili SerializeField   
    [SerializeField] private GameObject Utente_Locale;
    [SerializeField] private GameObject Altro_Utente;
    [SerializeField] private GameObject Oculus_Go;
    [SerializeField] private GameObject Oculus_Quest;
    [SerializeField] private SocketIOComponent socket;
    #endregion

    #region Variabili Private
    private string NomePlayer;
    private bool connectionMultiplayer = true;
    private string playerName;
    private string PathImage;
    private string NomeFile;
    private string full_name_player;
    private bool ConnessioneEffettuata = false;

    private Task Invio_Server = null;
    private Task Scaricamento_Img = null;
    private List<KeyValuePair<string, string>> List_Img_To_Send = null;
    private Semaphore Sem_Elementi_Lista = null;
    private Semaphore Sem_InvioServer = null;
    #endregion
    #endregion

    #region Metodi Unity
    private void Awake()
    {
        // Se non è stata già creata una Istanza del NetworkManager
        if( instance == null )
            instance = this;
        else if( instance != null )
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        playerName = GetNomePlayer();
    }

	private void Start ()
    {
        // Aggiunta di tutti i Listener della Socket con il WebServer NodeJS
        socket.On("play", OnPlay);
        socket.On("other player connected", otherPlayerConnected);
        socket.On("player move", OnPlayerMove);
        socket.On("player rotation", OnPlayerRotation);
        socket.On("player rotation head", OnPlayerRotationHead);
        socket.On("other player disconnect", OnOtherPlayerDisconnect);

        StartCoroutine(ControlloConnessioneServer());

        // Comando Creazione Cartella Foto
        StartCoroutine(CommandCreateFolder());
        StartCoroutine(SetImageSend());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator SetImageSend()
    {
        yield return new WaitForSeconds(1.0f);
        Sem_Elementi_Lista = new Semaphore(initialCount: 0, maximumCount: 100);
    
        Sem_InvioServer = new Semaphore(initialCount: 1, maximumCount: 1);
        Invio_Server = new Task(() => Invio_Img_Server());

        List_Img_To_Send = new List<KeyValuePair<string, string>>();
        Invio_Server.Start();
    }

    public void setConnectionMultiplayer( bool Esito )
    {
        connectionMultiplayer = Esito;
    }

    private IEnumerator ControlloConnessioneServer()
    {
        do
        {
            if (connectionMultiplayer)
                StartCoroutine(ConnectToServer());

            yield return new WaitForSeconds(2.0f);
        } while (ConnessioneEffettuata == false);
    }

    private void Invio_Img_Server()
    {
        while (true)
        {
            Debug.Log("Pronto a ricevere Img");

            Sem_Elementi_Lista.WaitOne();

            PathImage = List_Img_To_Send[0].Key;
            NomeFile = List_Img_To_Send[0].Value;
            List_Img_To_Send.RemoveAt(0);

            Sem_InvioServer.WaitOne();

            Scaricamento_Img = Task.Run(() => SendToServer());
        }
    }

    private void SendToServer()
    {
        Debug.Log("Invio Immagine al Server");

        byte[] imageBytes = File.ReadAllBytes(PathImage);
        string ImageBase64 = System.Convert.ToBase64String(imageBytes);

        string data = JsonUtility.ToJson(new ImageJSON(ImageBase64, NomeFile));
        socket.Emit("salvataggio foto", new JSONObject(data));

        Debug.Log("Img Inviata");
        Sem_InvioServer.Release();
    }
    private string GetNomePlayer()
    {
        string Path = Application.persistentDataPath + "/Username.txt";

        if (File.Exists(Path))
        {
            string Dati = File.ReadAllText(Path);
            string[] AllData = Dati.Split(char.Parse(" "));

            Debug.Log("Nome Utente = " + AllData[0]);

            full_name_player = AllData[1];

            for (int i = 2; i < AllData.Length; i++)
                full_name_player += " " + AllData[i];

            return AllData[0];
        }
        else
        {
            Debug.Log("Username = Prova");
            full_name_player = "Unknown";
            return "prova";
        }
    }
    #endregion

    #region Commands
    private IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerJSON playerJSON = new PlayerJSON("Android_Smartphone",playerName,"Hand_Dx",full_name_player);

        string data = JsonUtility.ToJson(playerJSON);
        socket.Emit("visit", new JSONObject(data));

        yield return new WaitForSeconds(0.6f);
        socket.Emit("player connect");
    }

    private IEnumerator CommandCreateFolder()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Creazione Cartella Utente");

        string data = JsonUtility.ToJson(new NameJSON(playerName));
        socket.Emit("creazione cartella utente", new JSONObject(data));
    }

    public void CommandMove( string name, Vector3 move )
    {
        string data = JsonUtility.ToJson(new PositionJSON(name,move));
        socket.Emit("player move", new JSONObject(data));
    }

    public void CommandRotation( string name, Quaternion rot)
    {
        string data = JsonUtility.ToJson(new RotationJSON(name,rot));
        socket.Emit("player rotation", new JSONObject(data));
    }

    public void CommandRotationHead( string name, Quaternion rot)
    {
        string data = JsonUtility.ToJson(new RotationJSON(name,rot));
        socket.Emit("player rotation head", new JSONObject(data));
    }

    public void CommandRotationHand(string name, Vector3 pos, Quaternion rot)
    {
        string data = JsonUtility.ToJson(new HandJSON(name, pos,rot));
        socket.Emit("player rotation hand", new JSONObject(data));
    }

    public void CommandSendImage( string _pathImage, string _nomeFile )
    {
        List_Img_To_Send.Add(new KeyValuePair<string, string>(_pathImage, _nomeFile));
        Sem_Elementi_Lista.Release();
    }
    #endregion

    #region JSONMessageClasses

    [System.Serializable]
    public class NameJSON
    {
        public string name;

        // Costruttore
        public NameJSON(string _name)
        {
            name = _name;
        }
    }

    [System.Serializable]
    public class ImageJSON
    {
        public string nomeFile;
        public string image;

        public ImageJSON( string _image, string _nomeFile )
        {
            image = _image;
            nomeFile = _nomeFile;
        }
    }


    [System.Serializable]
    public class PlayerJSON
    {
        public string device;
        public string name;
        public string full_name;
        public string[] position;
        public string[] rotation;
        public string[] rotationHead;
        public string hand = "";
        public string[] positionHand;
        public string[] rotationHand;
        
        // Costruttore
        public PlayerJSON(string _device,string _name,string _hand,string _full_name)
        {
            float X = Random.Range(-10f, 14f);
            float Z = Random.Range(-4.0f, 2f);

            device = _device;
            name = _name;
            hand = _hand;
            full_name = _full_name;
            position = new string[3] { "" + (int)X, "2.176", "" + (int)Z };
            rotation = new string[3] { "0.0", "0.0", "0.0" };
            rotationHead = new string[3] { "0.0", "0.0", "0.0" };

            if (hand == "Hand_Sx")
            {
                rotationHand = new string[3] { "1.16", "-2.012", "0.16" };
                positionHand = new string[3] { "0.0", "0.0", "90.0" };
            }
            else if (hand == "Hand_Dx")
            {
                rotationHand = new string[3] { "-0.18", "-2.17", "0.0" };
                positionHand = new string[3] { "0.0", "0.0", "90.0" };
            }
        }
    }

    [System.Serializable]
    public class PointJSON
    {
        public string[] position;
        public string[] rotation;

        public PointJSON(Movimento_Utente player)
        {
            float[] pos = new float[3];
            float[] rot = new float[3];

            pos = player.getPositionUser();
            rot = player.getRotationUser();

            position = new string[3] { "" + pos[0], "" + pos[1], "" + pos[2] };
            rotation = new string[3] { "" + rot[0], "" + rot[1], "" + rot[2] };
        }
    }

    [System.Serializable]
    public class PositionJSON
    {
        public string[] position;
        public string name;

        public PositionJSON( string _name, Vector3 _position)
        {
            name = _name;
            position = new string[3] { "" + _position.x, "" + _position.y, "" + _position.z };
        }
    }

    [System.Serializable]
    public class RotationJSON
    {
        public string[] rotation;
        public string name;

        public RotationJSON( string _name, Quaternion _rotation )
        {
            name = _name;
            float[] rot = new float[3]
            {
                _rotation.eulerAngles.x,
                _rotation.eulerAngles.y,
                _rotation.eulerAngles.z,
            };

            rotation = new string[3] { "" + rot[0], "" + rot[1], "" + rot[2] };
        }
    }

    [System.Serializable]
    public class RotationHeadJSON
    {
        public string[] rotation;
        public string name;
        public RotationHeadJSON( string _name, Quaternion _rotation)
        {
            name = _name;
            float[] rot = new float[3]
            {
                _rotation.eulerAngles.x,
                _rotation.eulerAngles.y,
                _rotation.eulerAngles.z,
            };

            rotation = new string[3] { "" + rot[0], "" + rot[1], "" + rot[2] };
        }
    }

    [System.Serializable]
    public class HandJSON
    {
        public string[] position;
        public string[] rotation;
        public string name;

        public HandJSON(string _name, Vector3 _position, Quaternion _rotation)
        {
            name = _name;
            position = new string[3] { "" + _position.x, "" + _position.y, "" + _position.z };

            float[] rot = new float[3]
            {
                _rotation.eulerAngles.x,
                _rotation.eulerAngles.y,
                _rotation.eulerAngles.z,
            };

            rotation = new string[3] { "" + rot[0], "" + rot[1], "" + rot[2] };
        }
    }

    [System.Serializable]
    public class UserJSON
    {
        public string device;
        public string name;
        public string full_name;
        public string[] position;
        public string[] rotation;
        public string[] rotationHead;
        public string hand;
        public string[] positionHand;
        public string[] rotationHand;

        // Converte la stringa ricevuta --> in un JSON
        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }
    #endregion

    #region Listening

    private void otherPlayerConnected( SocketIOEvent socketIOEvent )
    {
        Debug.Log("Nuovo Utente Connesso");

        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        GameObject Check_User = GameObject.Find(userJSON.name) as GameObject;

        if (Check_User != null)
            return;

        Vector3 position = new Vector3( float.Parse(userJSON.position[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.position[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.position[2], CultureInfo.InvariantCulture.NumberFormat));
        Quaternion rotation = Quaternion.Euler(float.Parse(userJSON.rotation[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.rotation[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.rotation[2], CultureInfo.InvariantCulture.NumberFormat));

        GameObject Nuovo_Utente = null;

        if ( userJSON.device == "Android_Smartphone" )
            Nuovo_Utente = Instantiate(Altro_Utente, position, rotation) as GameObject;
        else if( userJSON.device == "Oculus Go" )
            Nuovo_Utente = Instantiate(Oculus_Go, position, rotation) as GameObject;
        else if( userJSON.device == "Oculus Quest" )
            Nuovo_Utente = Instantiate(Oculus_Quest, position, rotation) as GameObject;

        Nuovo_Utente.GetComponent<Movimento_Utente>().setName(userJSON.name);
        Nuovo_Utente.GetComponent<Movimento_Utente>().setIsLocalPlayer(false);
        Nuovo_Utente.GetComponent<Movimento_Utente>().setNomeCompleto(userJSON.full_name);
    
        ConnessioneEffettuata = true;
    }

    private void OnPlay( SocketIOEvent socketIOEvent )
    {
        Debug.Log("Ti sei unito alla Visita Museale");

        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(float.Parse(userJSON.position[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.position[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.position[2], CultureInfo.InvariantCulture.NumberFormat));
        Quaternion rotation = Quaternion.Euler(float.Parse(userJSON.rotation[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.rotation[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(userJSON.rotation[2], CultureInfo.InvariantCulture.NumberFormat));

        Utente_Locale.transform.position = position;
        Utente_Locale.transform.rotation = rotation;
        Utente_Locale.GetComponent<Movimento_Utente>().setName(userJSON.name);
        Utente_Locale.GetComponent<Movimento_Utente>().setNomeCompleto(userJSON.full_name);
        Utente_Locale.GetComponent<Movimento_Utente>().setIsLocalPlayer(true);

        ConnessioneEffettuata = true;
    }

    private void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();

        UserJSON currentUser = UserJSON.CreateFromJSON(data);
        GameObject Utente = GameObject.Find(currentUser.name) as GameObject;

        if (Utente == null)
            return;

        // Se è il nostro Utente Locale...
        if (Utente.GetComponent<Movimento_Utente>().statusLocal())
            return;

        Vector3 position = new Vector3(float.Parse(currentUser.position[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.position[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.position[2], CultureInfo.InvariantCulture.NumberFormat));
        Utente.GetComponent<Movimento_Utente>().setPositionUser(position);
    }

    private void OnPlayerRotation(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();

        UserJSON currentUser = UserJSON.CreateFromJSON(data);
        GameObject Utente = GameObject.Find(currentUser.name) as GameObject;

        if (Utente == null)
            return;

        // Se è il nostro Utente Locale...
        if (Utente.GetComponent<Movimento_Utente>().statusLocal())
            return;

        Quaternion rotation = Quaternion.Euler(float.Parse(currentUser.rotation[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.rotation[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.rotation[2], CultureInfo.InvariantCulture.NumberFormat));
        Utente.GetComponent<Movimento_Utente>().setRotationUser(rotation);
    }

    private void OnPlayerRotationHead(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();

        UserJSON currentUser = UserJSON.CreateFromJSON(data);
        GameObject Utente = GameObject.Find(currentUser.name) as GameObject;

        if (Utente == null)
            return;

        // Se è il nostro Utente Locale...
        if (Utente.GetComponent<Movimento_Utente>().statusLocal())
            return;

        Quaternion rotation = Quaternion.Euler(float.Parse(currentUser.rotationHead[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.rotationHead[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.rotationHead[2], CultureInfo.InvariantCulture.NumberFormat));
        Utente.GetComponent<Movimento_Utente>().setRotationHead(rotation);
    }
    void OnPlayerRotationHand(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();

        UserJSON currentUser = UserJSON.CreateFromJSON(data);
        GameObject Utente = GameObject.Find(currentUser.name) as GameObject;

        if (Utente == null)
            return;

        // Se è il nostro Utente Locale...
        if (Utente.GetComponent<Movimento_Utente>().statusLocal())
            return;

        Vector3 position = new Vector3(float.Parse(currentUser.positionHand[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.positionHand[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.positionHand[2], CultureInfo.InvariantCulture.NumberFormat));

        Quaternion rotation = Quaternion.Euler(float.Parse(currentUser.rotationHand[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.rotationHand[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(currentUser.rotationHand[2], CultureInfo.InvariantCulture.NumberFormat));
        Utente.GetComponent<Movimento_Utente>().setRotationHand(position, rotation);
    }


    void OnOtherPlayerDisconnect(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON currentUser = UserJSON.CreateFromJSON(data);

        Destroy(GameObject.Find(currentUser.name) as GameObject);
    }
    #endregion
}
