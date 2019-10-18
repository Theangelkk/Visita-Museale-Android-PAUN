using System.Collections;

namespace GoogleVR.HelloVR
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;

    [RequireComponent(typeof(Collider))]
    public class Selezione_Oggetto : MonoBehaviour
    {
        #region Variabili Locali

        #region Variabili SerializeField
        [SerializeField] private GameObject Object;
        #endregion

        #region Variabili Private
        private GameObject Controller;
        #endregion

        #endregion

        #region Metodi Unity
        private void Start()
        {
            Controller = GameObject.FindWithTag("ElementoSelezionato");
        }
        #endregion

        #region Metodi Pubblici
        public void OnPointerEnter()
        {
            Controller.GetComponent<Controller_Elemento_Selezionato>().setOggetto(Object);
        }

        public void OnPointerExit()
        {
            Controller.GetComponent<Controller_Elemento_Selezionato>().setOggetto(null);
        }
        #endregion
    }
}
