using UnityEngine;
using TMPro;
using System;

public class ChatTextItem : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    //Modifies the textmeshpro text from a string parameter
    public void SetText(string playerName, string text)
    {
        DateTime date = DateTime.Now;
        int minutes = date.Minute;
        int seconds = date.Second;

        string timestamp = minutes + ":" + seconds + "|";

        textMesh.SetText(playerName + "|" + timestamp + text);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
