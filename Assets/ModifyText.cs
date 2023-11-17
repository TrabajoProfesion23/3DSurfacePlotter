using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ModifyText : MonoBehaviour
{
    private String expression;
    // Start is called before the first frame update
    void Start()
    {
        expression = "";
    }

    // Update is called once per frame
    void Update()
    {
        displayExpression();
    }

    private void displayExpression()
    {
        TextMeshProUGUI tmp = this.gameObject.GetComponent<TextMeshProUGUI>();
        String newexpr = "f(x,y)=" + expression.Replace("[y]", "y").Replace("[x]", "x");
        tmp.SetText(newexpr);
    }

    public void addChar(string s)
    {
        if (s == "back")
        {
            if (expression[^1] == ']')
            {
                expression = expression[..^3];
            }
            else 
            {
                expression = expression[..^1];
            }
        } else
        {
            expression += s;
        }
        
    }

    public string getExpression()
    {
        return this.expression;
    }
}
