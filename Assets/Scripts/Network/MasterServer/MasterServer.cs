using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class MasterServer
{
    private MasterServer(){}

    private List<Lobby> lobbies;

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<MasterServerClientHostRequestToBeAddedMessage>(OnMasterServerClientHostRequestToBeAdded);
        NetworkServer.RegisterHandler<MasterServerClientHostRequestToBeRemovedMessage>(OnMasterServerClientHostRequestToBeRemoved);
        NetworkServer.RegisterHandler<MasterServerDedicatedServerRequestToBeAddedMessage>(OnMasterServerDedicatedServerRequestToBeAdded);
        NetworkServer.RegisterHandler<MasterServerDedicatedServerRequestToBeRemovedMessage>(OnMasterServerDedicatedServerRequestToBeRemoved);
        NetworkServer.RegisterHandler<MasterServerClientRequestServerListMessage>(OnMasterServerClientRequestServerListing);

    }

    public void Init()
    {
        lobbies = new List<Lobby>();
    }

    private void OnMasterServerClientHostRequestToBeAdded(NetworkConnection connection, MasterServerClientHostRequestToBeAddedMessage message)
    {
        int newLobbyId = GenerateID();

        Lobby lobby = new Lobby
        {
            dedicated = false,
            id = newLobbyId,
            name = message.lobbyName,
            isPrivate = message.isPrivate,
            password = message.password
        };

        lobbies.Add(lobby);

        connection.identity.connectionToClient.Send(new MasterClientServerAddedDedicatedServerMessage{id = newLobbyId});

    }

    private void OnMasterServerClientHostRequestToBeRemoved(NetworkConnection connection, MasterServerClientHostRequestToBeRemovedMessage message)
    {
        for (var i = 0; i < lobbies.Count; i++)
        {
            if (lobbies[i].id == message.clientHostServerId)
            {
                lobbies.RemoveAt(i);
                break;
            }

        }
    }


    private int GenerateID()
    {
        int nextId = -1;

        for (var i = 0; i < lobbies.Count; i++)
        {
            int foundLobbyID = lobbies[i].id;

            if (foundLobbyID > nextId)
            {
                nextId = foundLobbyID;
            }
        }


        return nextId + 1;

    }

    private void OnMasterServerDedicatedServerRequestToBeRemoved(NetworkConnection connection, MasterServerDedicatedServerRequestToBeRemovedMessage message)
    {
        for (var i = 0; i < lobbies.Count; i++)
        {
            if (lobbies[i].id == message.dedicatedServerId)
            {
                lobbies.RemoveAt(i);
                break;
            }
        }
    }

    private void OnMasterServerDedicatedServerRequestToBeAdded(NetworkConnection connection, MasterServerDedicatedServerRequestToBeAddedMessage message)
    {
        int newLobbyId = GenerateID();

        Lobby lobby = new Lobby
        {
            dedicated = true,
            id = newLobbyId,
            name = message.lobbyName,
            isPrivate = message.isPrivate,
            password = message.password
        };

        lobbies.Add(lobby);

        connection.identity.connectionToClient.Send(new MasterClientServerAddedDedicatedServerMessage{id = newLobbyId});
    }

    private void OnMasterServerClientRequestServerListing(NetworkConnection connection, MasterServerClientRequestServerListMessage message)
    {
        Lobby[] lobbiesArray = lobbies.ToArray();
        connection.identity.connectionToClient.Send(new MasterClientServerSentServerListingMessage{lobbiesOnServer = lobbiesArray});
    }

}