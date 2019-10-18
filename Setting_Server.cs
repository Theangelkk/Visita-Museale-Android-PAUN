using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.IO;

public class Setting_Server : MonoBehaviour {

    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private SocketIOComponent socket;
    #endregion

    #region Variabili Private
    private string IP_Server_Multiplayer;
    private string Porta_Server_Multiplayer;
    private string PHP_Server;
    #endregion

    #endregion

    #region Metodi Unity
    private void Awake()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("Server_IP");

        if ( mytxtData == null )
            throw new System.Exception("Errore nessun file di Settaggio Server Caricato");

        string Dati = mytxtData.text;

        string[] AllData = Dati.Split(char.Parse(" "));

        if (AllData.Length == 3)
        {
            IP_Server_Multiplayer = AllData[0];
            Porta_Server_Multiplayer = AllData[1];
            PHP_Server = AllData[2];
        }
        else
            throw new System.Exception("Errore parametri mancanti settaggio Server");

        socket.url = "ws://" + IP_Server_Multiplayer + ":" + Porta_Server_Multiplayer + "/socket.io/?EIO=4&transport=websocket";

        socket.gameObject.SetActive(true);
        socket.Connect();
    }
    #endregion

    #region Metodi Pubblici
    public string getURL_PHPServer()
    {
        return PHP_Server;
    }
    #endregion
}
