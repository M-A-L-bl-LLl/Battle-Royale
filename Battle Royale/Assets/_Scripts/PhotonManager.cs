using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    [SerializeField] private string region;
    [SerializeField] TMP_InputField RoomName;
    [Space]
    [SerializeField] ListItem itemPrefab;
    [SerializeField] Transform content;
    [Space]
    [SerializeField] TMP_InputField nickName;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    private void Awake()
    {
        Instance = this;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Вы подключились к: " + PhotonNetwork.CloudRegion);
        
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Вы отключены от сервера!");
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
        SetNickName();
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnCreatedRoom()
    {
        
        Debug.Log("Создана комната, имя комнаты: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось создать комнату!");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            for (int i = 0; i < allRoomsInfo.Count; i++)
            {
                if (allRoomsInfo[i].masterClientId == info.masterClientId)
                    return;
            }

            ListItem listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
            {
                listItem.SetInfo(info);
                allRoomsInfo.Add(info);
            }                                
        }
    }

    public override void OnJoinedRoom()
    {        
        Debug.Log("Ваш nick: " + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("Game");
    }

    public void JoinRandomRoomButton()
    {
        SetNickName();
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinButton()
    {
        SetNickName();
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }

    public void SetNickName()
    {
        string PlayerNickName = nickName.text;
        if (PlayerNickName == "")
        {
            PhotonNetwork.NickName = "User";
        }
        else
            PhotonNetwork.NickName = PlayerNickName;
    }
}
