using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Net.NetworkInformation;

public class NetworkButton : MonoBehaviour
{
    public Camera mainMenuCamera;
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        mainMenuCamera.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        mainMenuCamera.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        mainMenuCamera.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
