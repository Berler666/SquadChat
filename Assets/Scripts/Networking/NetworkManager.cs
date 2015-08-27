using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Photon;

public class NetworkManager : PunBehaviour {
    const string ROOM_NAME = "SQUAD_CHAT";
    const int MESSAGE_COUNT = 6;

    [SerializeField] InputField messageInput;
    [SerializeField] Text chatLog;
    [SerializeField] Text connectionLog;
    [SerializeField] Button sendButton;
    Queue<string> messages = new Queue<string>(MESSAGE_COUNT);
    
    void Start() {
        Init();
        InitPhoton();
    }

    void Init() {
        sendButton.onClick.AddListener(CheckSendMessage);
    }

    void InitPhoton() {
        // Choose what Photon logs
        PhotonNetwork.logLevel =
            PhotonLogLevel.ErrorsOnly;
            //PhotonLogLevel.Full;
            //PhotonLogLevel.Informational;

        // Join lobby automatically after joining master server.
        PhotonNetwork.autoJoinLobby = true;

        // Connect using specified version (e.g. version 0.1). This is an
        // arbitrary value that ensures everyone is using the same build. If
        // you update the project, you should change this so people with
        // outdated builds can't join and exploit cheats/bugs from that build.
        PhotonNetwork.ConnectUsingSettings("0.1");

        // Start updating connection string.
        StartCoroutine("UpdateConnectionString");
    }

    IEnumerator UpdateConnectionString() {
        while (true) {
            // Update connection text with Photon connection state.
            connectionLog.text = PhotonNetwork.connectionStateDetailed.ToString();
            yield return null;
        }
    }

    // Photon event triggered when player joins the lobby.
    public override void OnJoinedLobby() {
        base.OnJoinedLobby();

        print("Joined lobby...");
        JoinRoom();
    }

    void JoinRoom() {
        print("Joining or creating room [" + ROOM_NAME + "]...");

        // Set username (needs validation).
        PhotonNetwork.player.name = User.GetName();

        // Join room if it exists or create new one otherwise.
        PhotonNetwork.JoinOrCreateRoom(
            ROOM_NAME,
            new RoomOptions {
                isVisible = true,
                maxPlayers = 10
            },
            TypedLobby.Default
        );
    }

    // Photon event triggered when player joins a room.
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();

        LogMessage(
            PhotonNetwork.player.name
            + " has joined " +
            PhotonNetwork.room.name
        );
    }

    void CheckSendMessage() {
        if (CanSendMessage())
            SendMessage();
    }

    bool CanSendMessage() {
        return messageInput.text.Length != 0;
    }

    void SendMessage() {
        LogMessage(SquadChatMessage(messageInput.text));
        messageInput.text = "";
        messageInput.Select();
    }

    string SquadChatMessage(string message) {
        return PhotonNetwork.player.name + ": " + message;
    }

    void LogMessage(string message) {
        print("Logging message [" + message + "]");

        // Send call to LogMessage_RPC method on all targets passing message
        // as the message parameter.
        photonView.RPC("LogMessage_RPC", PhotonTargets.All, message);
    }

    [PunRPC] void LogMessage_RPC(string message) {
        // Ensure messages queue has room for a new message BEFORE
        // queueing the new message.
        if (messages.Count == MESSAGE_COUNT)
            messages.Dequeue();

        // Add message to front of messages queue.
        messages.Enqueue(message);

        // Clear chat log.
        chatLog.text = "";

        // Write queued messages to chat log.
        foreach (string msg in messages)
            chatLog.text += msg + "\n";
    }

    // Dummy implementation of OnPhotonSerializeView().
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}
}