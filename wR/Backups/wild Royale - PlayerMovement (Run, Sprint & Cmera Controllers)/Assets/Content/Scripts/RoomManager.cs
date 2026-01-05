using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public GameObject player;
    public Transform[] spawnPoints;

    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("testroon", null, null);

        Debug.Log("We are connected to a lobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We are in a room");

        //Spawn our player
        StartGame();
    }

    public void StartGame()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
    }
}
