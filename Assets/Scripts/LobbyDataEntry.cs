using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class LobbyDataEntry : MonoBehaviour
{
    //Data
    public CSteamID LobbyID;
    public string LobbyName;
    public TMP_Text LobbyNameText;

    public void SetLobbyData()
    {
        if(LobbyName == "")
        {
            LobbyNameText.text = "Empty Lobby";
        }
        else
        {
            LobbyNameText.text = LobbyName;
        }
    }

    public void JoinLobby()
    {
        SteamLobby.Instance.JoinLobby(LobbyID);
    }
}
