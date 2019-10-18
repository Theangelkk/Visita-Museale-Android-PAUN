using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_Mesh : MonoBehaviour {

    #region Metodi Unity
    private void Awake()
    {
        foreach( var Obj in GetComponentsInChildren<MeshRenderer>() )
            Obj.enabled = false;
    }

    #region Metodi Trigger Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("FOV_Statua"))
            foreach (var Obj in GetComponentsInChildren<MeshRenderer>())
                Obj.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("FOV_Statua"))
            foreach (var Obj in GetComponentsInChildren<MeshRenderer>())
                Obj.enabled = false;
    }
    #endregion
    #endregion
}
