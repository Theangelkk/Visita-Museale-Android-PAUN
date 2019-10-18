using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Setting_Scene : MonoBehaviour {

    private enum Device
    {
        None,
        Cardboard
    };

    private enum Orientazione
    {
        Verticale,
        Orizzontale
    };

    #region Variabili SerializeField
    [SerializeField] private Device Nome_Device = Device.None;
    [SerializeField] private Orientazione Orient = Orientazione.Verticale;
    #endregion

    #region Metodi Unity
    private void Start()
    {
        StartCoroutine(LoadDevice());
    }
    #endregion

    #region Metodi Privati
    private IEnumerator LoadDevice()
    {
        if (Nome_Device == Device.None)
        {
            XRSettings.LoadDeviceByName("None");
            yield return null;
            XRSettings.enabled = false;
        }
        else if (Nome_Device == Device.Cardboard)
        {
            
            XRSettings.LoadDeviceByName("Cardboard");
            yield return null;
            XRSettings.enabled = true;
        }

        if( Orient == Orientazione.Verticale )
            Screen.orientation = ScreenOrientation.Portrait;
        else if( Orient == Orientazione.Orizzontale )
            Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    #endregion
}
