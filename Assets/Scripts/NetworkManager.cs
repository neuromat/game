using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour 
{
	public string webProtocol = "http://";
	public enum Server 
	{
		productionGameServer, 
		remoteTestGameServer, 
		localTestGameServer
	}
	public Server server;
	
	private readonly Dictionary<Server, string> knownServers
		= new Dictionary<Server, string>
		{
			{ Server.productionGameServer, "bb.scipopulis.com" },
			{ Server.remoteTestGameServer, "bb.scipopulis.com:4201" },
			{ Server.localTestGameServer, "127.0.0.1:4200"}
		};

	static private NetworkManager _instance;
	static public NetworkManager instance
	{
		get
		{
			if(_instance == null)
			{	
				_instance = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
			}
			
			return _instance;
		}
	}
	
	public delegate void DataSent (bool status, string msg);
	
	public void SendPlayDataToServer (DataSent callback, Dictionary<string, string> param)
	{
		
		WWW www = null;
		
		if(param == null)
		{
			Debug.Log("Please enter a parameter");
			return;
		}
		
		WWWForm form = new WWWForm ();
		foreach(KeyValuePair<string, string> p in param)
		{
			form.AddField (p.Key, p.Value);
		}
		
		string loginURL = webProtocol + knownServers[server] + "/api/v0/users/facebookLogin";

		www = new WWW (loginURL, form);
		StartCoroutine (SendPlayDataToServerResponse (www, callback));
	}
	
	IEnumerator SendPlayDataToServerResponse(WWW www, DataSent callback)
	{
		yield return www;
		
		// check for error first
		if (www.error != null && !string.IsNullOrEmpty (www.error))
		{
			if (callback != null)
			{
				Debug.Log ("WWW Error text - WaitForUserData: " + www.text + " error message " + www.error + "url " + www.url);
				callback (false, www.text);
				//return(false);
			}
		}
		
		if (www.text != null && !string.IsNullOrEmpty (www.text))
		{
			Debug.Log ("SUCCESS! " + www.text);
			
			//I've received a response from the server
			if (callback != null)
			{
				callback (true, www.text);
			}
		}
		else
		{
			//Something went wrong
			if (callback != null)
			{
				Debug.Log ("Error text: " + www.text + " error message " + www.error);
				callback (false, www.text);
			}
		}
	}
}