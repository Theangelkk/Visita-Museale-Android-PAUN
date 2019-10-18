using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Controller_Canvas_Exit : MonoBehaviour
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Text_ChoseLanguage Text_Spiegazione;
    [SerializeField] private Audio_Script Audio;
    #endregion

    #region Variabili Private
    private string Path = "";
    private Mapping_Input_Controller.Controller controller = Mapping_Input_Controller.Controller.PS4;
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

        if (controller == Mapping_Input_Controller.Controller.PS4)
        {
            Text_Spiegazione.setTextItaliano("Sei sicuro di voler uscire dalla Visita Museale ?\n\nSe si, premi il Tasto Trinagolo.\n\nSe invece intendi continuare premi il Tasto Cerchio.");
            Text_Spiegazione.setTextInglese("Are you sure you want to leave the Museum Visit?\n\nIf so, press the Triangolo button.\n\nIf you intend to continue, press the Circle button.");

            Audio.setTextItaliano("Sei sicuro di voler uscire dalla Visita Museale ? Se si, premi il Tasto Trinagolo. Se invece intendi continuare premi il Tasto Cerchio.");
            Audio.setTextInglese("Are you sure you want to leave the Museum Visit? If so, press the Triangolo button. If you intend to continue, press the Circle button.");
        }
        else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
        {
            Text_Spiegazione.setTextItaliano("Sei sicuro di voler uscire dalla Visita Museale ?\n\nSe si, premi il Tasto Y.\n\nSe invece intendi continuare premi il Tasto B.");
            Text_Spiegazione.setTextInglese("Are you sure you want to leave the Museum Visit?\n\nIf so, press the Y button.\n\nIf you intend to continue, press the B button.");

            Audio.setTextItaliano("Sei sicuro di voler uscire dalla Visita Museale ? Se si, premi il Tasto Y. Se invece intendi continuare premi il Tasto B.");
            Audio.setTextInglese("Are you sure you want to leave the Museum Visit? If so, press the Y button. If you intend to continue, press the B button.");
        }
    }
    #endregion
}
