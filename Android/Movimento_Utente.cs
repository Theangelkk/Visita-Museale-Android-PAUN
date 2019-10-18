using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movimento_Utente : MonoBehaviour
{
    #region Variabili Locali
    #region Variabili SerializeField    
    [SerializeField] private GameObject Cam;
    [SerializeField] private GameObject Canvas_NomeCompleto;
    [SerializeField] private Text Text_NomeCompleto_Utente;

    [SerializeField] private bool isLocal = false;
    [SerializeField] private bool canMovement = true;
    [SerializeField] private bool isMultiplayer = true;

    [SerializeField] private GameObject HandLeft = null;
    [SerializeField] private GameObject HandRight = null;
    #endregion

    #region Variabili Private
    private GameObject RealHand = null;
    private bool Change = false;
    private int Speed = 6;

    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private Quaternion oldRotationUser;
    private Quaternion currentRotationUser;
    private Quaternion oldRotationHead;
    private Quaternion currentRotationHead;
    #endregion
    #endregion

    #region Metodi Unity
    private void Start()
    {
        currentPosition = oldPosition = this.GetComponent<CharacterController>().transform.position;
        currentRotationUser = oldRotationUser = this.GetComponent<CharacterController>().transform.rotation;
        currentRotationHead = oldRotationHead = Cam.transform.rotation;
    }

    private void Update()
    {
        if (canMovement)
        {
            UpdateRotationHead();

            if (    Input.GetKey(KeyCode.UpArrow) ||
                    Input.GetKey(KeyCode.LeftArrow) ||
                    Input.GetKey(KeyCode.DownArrow) ||
                    Input.GetKey(KeyCode.RightArrow) 
               )
                Movement();

            if( Input.GetAxis("Mouse X") >= 0.1f  ||
                Input.GetAxis("Mouse X") <= -0.1f ||
                Input.GetAxis("Mouse Y") >= 0.1f  ||
                Input.GetAxis("Mouse Y") <= -0.1f     )
                Movement();

            Canvas_NomeCompleto.transform.rotation = Quaternion.Euler(0.0f, Cam.transform.rotation.eulerAngles.y, 0.0f);
        }
    }
    #endregion

    //---------------------------------------------------- METODI PUBBLICI ------------------------------------------------------------------
    #region Metodi Pubblici
    public void Attiva_Movimento()
    {
        canMovement = true;
    }

    public void Disattiva_Movimento()
    {
        canMovement = false;
    }

    public void setIsLocalPlayer(bool Esito)
    {
        if (Change == false)
        {
            isLocal = Esito;
            Change = true;

            StartCoroutine(Attivazione());
        }
    }
    #endregion

    //************************************************** Metodi Setter ********************************************************************
    #region Metodi Setter
    public void setPositionUser( Vector3 position )
    {
        this.GetComponent<CharacterController>().transform.position = position;
    }

    public void setRotationUser(Quaternion rotation)
    {
        this.GetComponent<CharacterController>().transform.rotation = rotation;
    }

    public void setRotationHand(Vector3 position, Quaternion rotation)
    {
        RealHand.transform.position = position;
        RealHand.transform.rotation = rotation;
    }

    public void setName(string _name)
    {
        this.gameObject.name = _name;
    }

    public void setRotationHead(Quaternion rotation)
    {
        Cam.transform.rotation = rotation;
    }

    public void setNomeCompleto(string NomeCompleto)
    {
        Text_NomeCompleto_Utente.text = NomeCompleto;
    }

    public void setMultiplayer(bool Scelta)
    {
        isMultiplayer = Scelta;
    }

    public void setHand(string Hand)
    {
        if (Hand == "Hand_Sx" && HandLeft != null)
        {
            HandLeft.SetActive(true);
            RealHand = HandLeft;
        }
        else if (Hand == "Hand_Dx" && HandRight != null)
        {
            HandRight.SetActive(true);
            RealHand = HandRight;
        }
    }

    public void setSpeed(int speed)
    {
        Speed = speed;
    }
    #endregion

    //************************************************** Metodi Getter ********************************************************************
    #region Metodi Getter
    public float[] getPositionUser()
    {
        Transform t = this.GetComponent<CharacterController>().transform;

        return new float[3]
        {
            t.position.x,
            t.position.y,
            t.position.z
        };
    }
    public float[] getRotationUser()
    {
        Transform t = this.GetComponent<CharacterController>().transform;

        return new float[3]
        {
            t.eulerAngles.x,
            t.eulerAngles.y,
            t.eulerAngles.z
        };
    }

    public float[] getRotationUserHead()
    {
        Transform t = Cam.transform;

        return new float[3]
        {
            t.eulerAngles.x,
            t.eulerAngles.y,
            t.eulerAngles.z
        };
    }

    public bool statusLocal()
    {
        return isLocal;
    }
    #endregion

    //---------------------------------------------------- METODI PRIVATI ------------------------------------------------------------------
    #region Metodi Privati
    private IEnumerator Attivazione()
    {
        yield return new WaitForSeconds(0.2f);

        if (isLocal)
        {
            setName(name);
            setMultiplayer(true);
            Attiva_Movimento();
        }
    }

    //************************************************** MOVIMENTI UTENTE ********************************************************************
    #region Metodi Movimento Utente
    private void Movement()
    {
        Vector3 moveDirection = Vector3.forward;
        moveDirection *= Speed;

        moveDirection = Cam.transform.TransformDirection(moveDirection);

        moveDirection.y -= 1000.0f * Time.deltaTime;

        // Move the controller
        this.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);

        if (isMultiplayer)
        {
            currentPosition = this.GetComponent<CharacterController>().transform.position;
            currentRotationUser = this.GetComponent<CharacterController>().transform.rotation;

            if (oldPosition != currentPosition)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(this.name, currentPosition);
                oldPosition = currentPosition;
            }

            if (oldRotationUser != currentRotationUser)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandRotation(this.name, currentRotationUser);
                oldRotationUser = currentRotationUser;
            }
        }
    }

    private void UpdateRotationHead()
    {
        if (isMultiplayer)
        {
            currentRotationHead = Cam.transform.rotation;

            if (oldRotationHead != currentRotationHead)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandRotationHead(this.name, currentRotationHead);
                oldRotationHead = currentRotationHead;
            }
        }
    }
    #endregion
    #endregion
}
