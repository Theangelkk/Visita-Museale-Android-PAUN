using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Choose_Controller : Interactable
{
    #region Variabili Locali

    #region Variabili SerializeField
    [SerializeField] private Controller Scelta_Controller = Controller.None;
    #endregion

    #region Variabili Private
    private enum Controller
    {
        None,
        PS4,
        Xbox_One
    };
    private string Path = "";
    #endregion

    #endregion

    #region Metodi Unity
    private void Start ()
    {
        Path = Application.persistentDataPath + "/Controller.txt";

        if (File.Exists(Path))
            File.Delete(Path);

        // Andiamo a creare il File
        File.WriteAllText(Path, "");
    }
    #endregion

    #region Metodi Pubblici
    public override void Touch() { }

    public override void Click()
    {
        this.GetComponent<AudioSource>().Play();

        if (Scelta_Controller == Controller.None || Scelta_Controller == Controller.PS4)
        {
            File.WriteAllText(Path, "PS4");
            StartCoroutine(CaricamentoScena("Tutorial_PS4"));
        }
        else if (Scelta_Controller == Controller.Xbox_One)
        {
            File.WriteAllText(Path, "Xbox_One");
            StartCoroutine(CaricamentoScena("Tutorial_Xbox_One"));
        }
    }
    #endregion

    #region Metodi Privati
    private IEnumerator CaricamentoScena(string Scena)
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(Scena);
    }
    #endregion
}
