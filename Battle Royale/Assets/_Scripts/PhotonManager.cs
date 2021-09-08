using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] TMP_InputField RoomName;
    [Space]
    [SerializeField] ListItem itemPrefab;
    [SerializeField] Transform content;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }    

    public override void OnConnectedToMaster()
    {
        Debug.Log("�� ������������ �: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("�� ��������� �� �������!");
    }

    public void CreateRoomButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("������� �������, ��� �������: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("�� ������� ������� �������!");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            ListItem listItem = Instantiate(itemPrefab, content);
            if (listItem != null)            
                listItem.SetInfo(info);            
        }
    }
}
