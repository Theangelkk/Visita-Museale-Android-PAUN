using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapping_Input_Controller : MonoBehaviour {

    public enum Controller
    {
        None,
        PS4,
        XBOX_ONE
    };

    static Dictionary<string, KeyCode> buttonMapping_PS4;
    static Dictionary<string, KeyCode> buttonMapping_Xbox_One;

    #if UNITY_EDITOR
    static string[] keyMaps_PS4 = new string[14]
    {
        "Square",
        "X",
        "Circle",
        "Triangle",
        "L1",
        "R1",
        "L2",
        "R2",
        "Share",
        "Options",
        "L3",
        "R3",
        "PS",
        "PadPress"
    };
    #else
    // Android --> Wireless Controller
    static string[] keyMaps_PS4 = new string[14]
    {
        
        "X",
        "Circle",
        "Square",
        "Triangle",
        "L1",
        "R1",
        "L2",
        "R2",
        "L3",
        "R3",
        "Options",
        "Share",
        "PS",
        "PadPress"
    };
    #endif

    static string[] keyMaps_Xbox_One = new string[8]
    {
        "A",
        "B",
        "X",
        "Y",
        "LB",
        "RB",
        "Share",
        "Start"
    };

    static KeyCode[] defaults = new KeyCode[14]
    {
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton1,
        KeyCode.JoystickButton2,
        KeyCode.JoystickButton3,
        KeyCode.JoystickButton4,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton6,
        KeyCode.JoystickButton7,
        KeyCode.JoystickButton8,
        KeyCode.JoystickButton9,
        KeyCode.JoystickButton10,
        KeyCode.JoystickButton11,
        KeyCode.JoystickButton12,
        KeyCode.JoystickButton13,
    };

    // Costruttore
    static Mapping_Input_Controller()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        Debug.Log("Mi inizializzo");

        buttonMapping_PS4 = new Dictionary<string, KeyCode>();

        for (int i = 0; i < keyMaps_PS4.Length; i++)
        {
            buttonMapping_PS4.Add(keyMaps_PS4[i], defaults[i]);
        }

        buttonMapping_Xbox_One = new Dictionary<string, KeyCode>();

        for (int i = 0; i < keyMaps_Xbox_One.Length; i++)
        {
            buttonMapping_Xbox_One.Add(keyMaps_Xbox_One[i], defaults[i]);
        }
    }

    public static bool GetKey(Mapping_Input_Controller.Controller controller, string keyMap )
    {
        if (controller == Mapping_Input_Controller.Controller.PS4)
            return Input.GetKey(buttonMapping_PS4[keyMap]);
        else if (controller == Mapping_Input_Controller.Controller.XBOX_ONE)
            return Input.GetKey(buttonMapping_Xbox_One[keyMap]);

        return false;
    }
}
