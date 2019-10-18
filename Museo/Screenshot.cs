using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Screenshot : Interactable {

    [SerializeField] private GameObject Canvas_User;
    [SerializeField] private GameObject Line_Render;

    private int N_Photo = 0;
    private string Path_Directory = "";
    private Task Task_Salvataggio_Img = null;
    private Task Task_Scaricamento_Img = null;

    // Use this for initialization
	private void Start ()
    {
        Path_Directory = Application.persistentDataPath + "/Screenshot";

        if (Directory.Exists( Path_Directory ))
            Directory.Delete( Path_Directory , true);

        Directory.CreateDirectory( Path_Directory );
    }

    public override void Click()
    {
        Canvas_User.SetActive(false);
        Line_Render.SetActive(false);
        this.GetComponent<AudioSource>().Play();

        N_Photo++;
        Task_Salvataggio_Img = Task.Run(() => {
                                                Debug.Log("Task Salvataggio Foto");

                                                string NomeFile = N_Photo + ".png";

                                                ScreenCapture.CaptureScreenshot("/Screenshot/" + NomeFile);

                                                string PathImage = Path_Directory + "/" + NomeFile;

                                                Thread.Sleep(1500);

                                                if (File.Exists(PathImage))
                                                {
                                                    Debug.Log("Foto Esistente");

                                                    NetworkManager.instance.CommandSendImage(PathImage, NomeFile);
                                                }
                                                else
                                                    Debug.Log("Errore Salvataggio Foto");

                                            }
                                        );

        Canvas_User.SetActive(true);
        Line_Render.SetActive(true);

    }

    public override void Touch()
    {   }
}
