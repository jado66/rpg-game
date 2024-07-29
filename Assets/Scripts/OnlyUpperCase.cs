using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyUpperCase : MonoBehaviour
{
    public InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
    }
    
    char Val(char c)
    {
        c =  char.ToUpper(c);
        return char.IsLetter(c) ? c : '\0';
    }

}
