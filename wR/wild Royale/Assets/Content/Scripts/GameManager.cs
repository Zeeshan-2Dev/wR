using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject airplanePrefab;
    public Transform mapCenter;

    private Transform[] seatPositions;
    private int seatIndex = 0;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject plane = PhotonNetwork.Instantiate(airplanePrefab.name, Vector3.zero, Quaternion.identity);
            seatPositions = plane.GetComponentsInChildren<Transform>().Where(t => t.name.Contains("Seat")).ToArray();
            plane.GetComponent<AirshipPath>().dropZoneCenter = mapCenter;

            

        }

        StartCoroutine(SpawnPlayerAfterDelay());
    }

    System.Collections.IEnumerator SpawnPlayerAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Vector3 spawn = new Vector3(0, 100, 0); // High position
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawn, Quaternion.identity);
        Vector3 seatPos = seatPositions[seatIndex % seatPositions.Length].position;
        player.transform.SetParent(seatPositions[seatIndex % seatPositions.Length]);
        seatIndex++;

    }
}
