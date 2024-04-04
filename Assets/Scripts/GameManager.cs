using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class GameManager : NetworkBehaviour
{
    public ImprovisedPlayerScript playerPrefab;
    private Vector3 spawnPoint1 = new Vector3(-13, 3 , 12);
    private Vector3 spawnPoint2 = new Vector3(-13, 3, -8);
    public static GameManager instance { get; private set; }

    public int noOfPlayers { get; private set; }

    public List<ImprovisedPlayerScript> playerList { get; private set; }
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NewPlayerConnected;
        }
        playerList = new List<ImprovisedPlayerScript>();

        base.OnNetworkSpawn();
    }

    void NewPlayerConnected(ulong playerID)
    {
        if (IsServer)
        {
            noOfPlayers++;

            ImprovisedPlayerScript player;

            player = Instantiate(playerPrefab, spawnPoint1, Quaternion.identity);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerID);
            player.spawnpoint = spawnPoint1;
            player.playerName.Value = "Player " + noOfPlayers;
            player.SetName();
            playerList.Add(player);

            
            
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
