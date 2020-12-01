using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class MainNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SteamP2PTransport.SteamP2PTransport>().ConnectToSteamID = 76561198022385036;
        //NetworkingManager.Singleton.StartHost();
        
        
    }
    public void StartHost()
    {
        if(SteamManager.Initialized) NetworkingManager.Singleton.StartHost();

    }
    public void StartClient()
    {
        if (SteamManager.Initialized) NetworkingManager.Singleton.StartClient();

    }
    public void SetId(string id)
    {
        GetComponent<SteamP2PTransport.SteamP2PTransport>().ConnectToSteamID = ulong.Parse(id);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
