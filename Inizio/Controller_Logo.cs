using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Logo : MonoBehaviour {

    #region Variabili SerializeField
    [SerializeField] private GameObject Angelo_Casolaro;
    [SerializeField] private GameObject Canvas_Logo;
    #endregion

    #region Metodi Unity
    private void Start ()
    {
        Angelo_Casolaro.SetActive(false);
        Canvas_Logo.SetActive(false);
        StartCoroutine(Inizio_Animazione());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator Inizio_Animazione()
    {
        this.GetComponent<Animator>().SetTrigger("Fade_In");
        yield return new WaitForSeconds(4.0f);

        this.GetComponent<Animator>().SetTrigger("Fade_Out");
        yield return new WaitForSeconds(1.0f);

        Angelo_Casolaro.SetActive(true);
        Angelo_Casolaro.GetComponent<Animator>().SetTrigger("Fade_In");

        yield return new WaitForSeconds(4.0f);
        Angelo_Casolaro.GetComponent<Animator>().SetTrigger("Fade_Out");

        yield return new WaitForSeconds(4.0f);

        Canvas_Logo.SetActive(true);
        Canvas_Logo.GetComponent<Animator>().SetTrigger("Fade_In");

        yield return new WaitForSeconds(4.0f);
        Canvas_Logo.GetComponent<Animator>().SetTrigger("Fade_Out");

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Choose_Language");
    }
    #endregion
}
