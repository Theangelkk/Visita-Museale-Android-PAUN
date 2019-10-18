using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Controller_Speed : MonoBehaviour
{
    #region Variabili Private
    private string Path = "";
    private int Speed = 6;
    #endregion

    #region Metodi Unity
    private void Start()
    {
        Speed = 6;
    }
    #endregion

    #region Metodi Pubblici
    public int getSpeed()
    {
        return Speed;
    }

    public bool increaseSpeed()
    {
        if (Speed <= 10)
        {
            Speed++;
            return true;
        }

        return false;
    }

    public bool decreaseSpeed()
    {
        if (Speed >= 4)
        {
            Speed--;
            return true;
        }

        return false;
    }
    #endregion
}
