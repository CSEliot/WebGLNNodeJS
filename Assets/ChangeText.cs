using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeText : MonoBehaviour 
{
    public Text ClientText;
    public Text ServerText;
    private string retrievedText;
   
	// Use this for initialization
	/*void Start () 
    {
	    BeginDownloading = false;
	}*/
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void POST()
    {
        string TextToSend = ClientText.text;
        GameObject.Find("PretendServer").GetComponent<ServerPretend>().POST(TextToSend);
    }

    public void GET()
    {
        GameObject.Find("PretendServer").GetComponent<ServerPretend>().GET();
    }

    private void FARTS()
    {
        Debug.Log("Farts");
    }
}