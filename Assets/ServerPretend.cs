using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;


public class ServerPretend : MonoBehaviour {

	private string url = "http://52.11.71.22:8082";
	private bool isDownloadFinished;
	
	public Text ServerText;

    [Serializable]
    public struct DataTable 
    {
        public string textFile;
    }

    private DataTable MyTable;
	// Use this for initialization
	void Start () 
    {
		isDownloadFinished = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
		MyTable.textFile = GameObject.Find("Display_Client").GetComponent<Text>().text;
	}

    public void GET()
    {
		isDownloadFinished = false;
        StartCoroutine(GetHelper()); //calls load_values
    }

    public void POST(string file)
    {
		Debug.Log("MyTable.text says: " + MyTable.textFile);
		Debug.Log("Saving to Database Attempting . . .");
		byte[] bytesToUpload = Save_Values();
		StartCoroutine(PostHelper(bytesToUpload));
        //MyTable.textFile = file;
        //Save_Values();
    }

    private byte[] Save_Values()
    {
        Debug.Log("Attempting to save ");
        IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream("Table.bin", FileMode.Create, FileAccess.Write, FileShare.None);
        //formatter.Serialize(stream, MyTable);
        //stream.Close();
		byte[] bytes;
		using(MemoryStream stream = new MemoryStream())
		{
			formatter.Serialize(stream, MyTable);
			bytes = new byte[stream.Length];
			stream.Position = 0;
			stream.Read(bytes, 0, (int)stream.Length);
		}
		Debug.Log("Saving Database likely successful");
		return bytes;
    }

    private void Load_Values(WWW download)
    {
		//convert the downloaded text back into bytes
		byte[] bytes = Convert.FromBase64String(download.text);
		
        IFormatter formatter = new BinaryFormatter();
		try
		{
			Byte[] content = bytes;
			using (MemoryStream ms = new MemoryStream(content))
			{
				MyTable = (DataTable)formatter.Deserialize(ms); 
				Debug.Log("Uploaded file is: " + MyTable.textFile);                       
			}
			ServerText.text = MyTable.textFile;
		}
		catch (Exception ex)
		{
			Debug.Log("Exception: " + ex.Message);
			Debug.Log("Stack Trace: " + ex.StackTrace);
		}
		Debug.Log("Loading Database likely successful");
    }

	public bool getDownloadStatus()
	{
		return isDownloadFinished;
	}

    IEnumerator GetHelper()
    {
        // Create a download object
        WWW download = new WWW(url+"/bar");
        
		Debug.Log("Downloading . . ." + download.progress);
        // Wait until the download is done
        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            Load_Values(download);
            // show the highscores
            Debug.Log("Downloaded data is done? " + download.isDone);
            //MyTable.textFile = (string)download.text;
        }
    }
    
    IEnumerator PostHelper(byte[] bytesToUpload)
    {
		WWWForm form = new WWWForm();
		
		string base64 = Convert.ToBase64String(bytesToUpload);
		
		form.AddField("foo", base64);
		//form.AddBinaryData("foo", bytesToUpload);
		
		WWW upload = new WWW(url+"/foo", form);
	
		yield return upload;
		if (!string.IsNullOrEmpty(upload.error)) {
			Debug.Log("Error Uploading: " + upload.error);
		}
		else {
			Debug.Log("Finished Uploading File.");
		}
    }
}
