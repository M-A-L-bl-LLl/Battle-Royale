using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text playerCount;

    public void SetInfo(RoomInfo info)
    {
        roomName.text = info.Name;
        playerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }
}
