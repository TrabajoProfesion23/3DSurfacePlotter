using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        tmp.SetText(getDisplayString());
    }

    private String getDisplayString()
    {
        String newexpr = "f(x,y)=" + expression.Replace("[y]", "y").Replace("[x]", "x");
        
        return newexpr;
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
        } else if (s == "^")
        {
            expression += "^(";
        } 
        else
        {
            expression += s;
        }
        
    }

    public string getExpression()
    {
        string ret = applyRegex();
        return ret;
    }
    public String applyRegex()
    {
        String pat1 = @"\[([xy])\]";
        String ret = Regex.Replace(expression, pat1, "([$1])");
        String pattern = @"\(([\d+\-*/x\[\]y]+)\)\^\(([\d+\-*/x\[\]y]+)\)";
        ret = Regex.Replace(ret, pattern, "Pow($1, $2)");
        Debug.Log(ret);
        return ret;
    }
}
