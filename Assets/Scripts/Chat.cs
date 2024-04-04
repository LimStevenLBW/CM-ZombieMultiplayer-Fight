using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Chat : NetworkBehaviour
{
    public ChatTextItem chatPrefab;
    public TMP_InputField chatInputField;
    private List<ChatTextItem> chatLog = new List<ChatTextItem>();
    private bool isFocused;

    public bool ToggleChat(string playerName)
    {
        if (!isFocused) //They werent typing, start typing
        {
            isFocused = true;
            //Allows them to start typing
            chatInputField.ActivateInputField();
            return isFocused;
        }
        else //Send Message
        {
            isFocused = false;
            string text = chatInputField.text;

            if(!text.Equals(""))
            {
                if (IsServer) //Server
                {
                    SyncChatClientRpc(playerName, text);
                }
                else //Client
                {
                    NotifyChatServerRpc(playerName, text);
                }
            }

            chatInputField.text = "";
            chatInputField.DeactivateInputField();
            return isFocused;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void NotifyChatServerRpc(string playerName, string text)
    {
        SyncChatClientRpc(playerName, text);
    }

    [ClientRpc]
    void SyncChatClientRpc(string playerName, string text)
    {
        CreateChatItem(playerName, text);
    }

    void CreateChatItem(string playerName, string text)
    {
        StopAllCoroutines();
        StartCoroutine(CleanChat());

        ChatTextItem newChatPrefab = Instantiate(chatPrefab);
        newChatPrefab.SetText(playerName, text);
        newChatPrefab.transform.SetParent(transform);
        chatLog.Add(newChatPrefab);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CleanChat()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (chatLog.Count == 0) continue; //if the list is empty, do not remove anything

            ChatTextItem chatItem = chatLog[0];
            chatItem.gameObject.SetActive(false);
            chatLog.RemoveAt(0);
        }
    }
}
