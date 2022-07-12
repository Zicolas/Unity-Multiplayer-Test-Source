using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar]public int ConnectionID;
    [SyncVar]public int PlayerIdNumber;
    [SyncVar]public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;

    //Cosemetics
    [SyncVar(hook = nameof(SendPlayerColor))] public int PlayerColor;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if(manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void PlayerReadyUpdate(bool OldValue, bool NewValue)
    {
        if(isServer)
        {
            this.Ready = NewValue;
        }
        if(isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }

    public void ChangeReady()
    {
        if(hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    public void PlayerNameUpdate(string OldValue, string NewValue)
    {
        if(isServer) //Host
        {
            this.PlayerName = NewValue;
        }
        if(isClient) //Client
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    //Start Game

    public void CanStartGame(string SceneName)
    {
        if(hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }

    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }

    //Cosmetics

    [Command]
    public void CmdUpdatePlayerColor(int NewValue)
    {
        SendPlayerColor(PlayerColor, NewValue);
    }

    public void SendPlayerColor(int OldValue, int NewValue)
    {
        if(isServer) //Host
        {
            PlayerColor = NewValue;
        }
        if(isClient && (OldValue != NewValue)) //Client
        {
            UpdateColor(NewValue);
        }
    }

    void UpdateColor(int message)
    {
        PlayerColor = message;
    }
}
