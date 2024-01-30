using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonColorChanger : MonoBehaviour
{
    public int maxThrough = 10;
    public Image[] buttons = new Image[9];
    


    public void ButtonColorChange(Station[] stations){
        for(int i = 0; i < 9; i++){
            buttons[i].color = CalculateColor(stations[i].through);
        }
    }

    Color CalculateColor(int value)
    {
        Debug.Log("ColorValue = " + value);
        Color color;        
        if(value > maxThrough){
            color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }else if(value > 0 && value <= maxThrough){
            float ratio = 1.0f - ((float)value / (float)maxThrough);
            color = new Color(1.0f, ratio, ratio, 1.0f);
        }else{
            color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        return color;
    }
}
