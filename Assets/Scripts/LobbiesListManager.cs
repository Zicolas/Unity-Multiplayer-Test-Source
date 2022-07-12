using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbiesListManager : MonoBehaviour
{

    public static LobbiesListManager Instance;

    //Lobbies List Variables
    public GameObject LobbiesMenu;
    public GameObject LobbyDataItemPrefab;
    public GameObject LobbyListContent;

    public GameObject LobbiesButton, HostButton;

    public List<GameObject> ListOfLobbies = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null) {Instance = this;}
    }

    public void GetListOfLobbies()
    {
        LobbiesButton.SetActive(false);
        HostButton.SetActive(false);

        LobbiesMenu.SetActive(true);

        SteamLobby.Instance.GetLobbiesList();
    }

    public void DisplayLobbies(List<CSteamID> LobbyIDs, LobbyDataUpdate_t result)
    {
        for(int i = 0; i < LobbyIDs.Count; i++)
        {
            if(LobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                GameObject createdItem = Instantiate(LobbyDataItemPrefab);

                createdItem.GetComponent<LobbyDataEntry>().LobbyID = (CSteamID)LobbyIDs[i].m_SteamID;

                createdItem.GetComponent<LobbyDataEntry>().LobbyName = SteamMatchmaking.GetLobbyData((CSteamID)LobbyIDs[i].m_SteamID, "name");

                createdItem.GetComponent<LobbyDataEntry>().SetLobbyData();

                createdItem.transform.SetParent(LobbyListContent.transform);
                createdItem.transform.localScale = Vector3.one;

                ListOfLobbies.Add(createdItem);
            }
        }
    }

    public void DestroyLobbies()
    {
        foreach(GameObject LobbyItem in ListOfLobbies)
        {
            Destroy(LobbyItem);
        }
        ListOfLobbies.Clear();
    }
}
