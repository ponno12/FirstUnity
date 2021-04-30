using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionVisualizor : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnGUI()
    {
        void Show(string txt, TextAnchor align)
        {
            var rect = new Rect(x: 0, y: 0, Screen.width, height: Screen.height * 2 / 100);
            var style = new GUIStyle
            {
                alignment = align, fontSize = (int) rect.height, normal = { textColor = Color.red}
            };
            GUI.Label(rect, txt ,style);
            
        }
        Show(txt: $"{transform.position}", align: TextAnchor.UpperLeft);
    }
}
