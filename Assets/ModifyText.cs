using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ModifyText : MonoBehaviour
{
    public String expression;
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
        if (s.Equals("back"))
        {
            expression.Remove(expression.Length-1);
        }
        expression += s;
    }

    public string getExpression()
    {
        return this.expression;
    }
}
