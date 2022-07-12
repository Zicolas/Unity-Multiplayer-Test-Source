using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    //Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    //Lobies Callbacks
    protected Callback<LobbyMatchList_t> LobbyList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;

    public List<CSteamID> LobbyIDs = new List<CSteamID>();

    //Variables
    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager manager;

    private void Start()
    {
        if(!SteamManager.Initialized) {return;}
        if(Instance == null) {Instance = this;}

        manager = GetComponent<CustomNetworkManager>();

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK) {return;}
        {
            Debug.Log("Lobby Created Succesfully");

            manager.StartHost();

            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        }
    }
    
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //Everyone
        CurrentLobbyID = callback.m_ulSteamIDLobby;

        //Client
        if(NetworkServer.active) {return;}
        
        manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        manager.StartClient();
    }

    public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
    }

    public void GetLobbiesList()
    {
        if(LobbyIDs.Count > 0) {LobbyIDs.Clear();}

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(100);
        SteamMatchmaking.RequestLobbyList();
    }

    void OnGetLobbyList(LobbyMatchList_t result)
    {
        if(LobbiesListManager.Instance.ListOfLobbies.Count > 0) {LobbiesListManager.Instance.DestroyLobbies();}
        
        for(int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID LobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            LobbyIDs.Add(LobbyID);
            SteamMatchmaking.RequestLobbyData(LobbyID);
        }
    }

    void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbiesListManager.Instance.DisplayLobbies(LobbyIDs, result);
    }
}
