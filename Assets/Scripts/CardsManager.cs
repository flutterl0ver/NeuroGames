using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour {
    private int servers = 0;
    private int totalServers = 3;

    public static CardsManager Instance;

    [SerializeField]
    private GameObject window;

    [SerializeField]
    private GameObject returnText;

    [SerializeField]
    private Button closeButton;

    private Card[] cards;

    public void Init(int totalServers) {
        this.totalServers = totalServers;
        servers = 0;
    }
    
    private void Awake() {
        Instance = this;
        cards = FindObjectsByType<Card>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public void Open() {
        window.SetActive(true);
        foreach (Card card in cards) {
            card.gameObject.SetActive(servers == card.serverNumber);
        }
    }

    public void SelectCard(Card selectedCard) {
        foreach (Card card in cards) {
            card.gameObject.SetActive(card == selectedCard);
        }
        closeButton.gameObject.SetActive(true);
    }
    
    public void Close() 
    {
        window.SetActive(false);
        PlayerController.SetMovementLocked(false);
        Server.CurrentServer.Deactivate();
        servers++;
        if(servers == totalServers) returnText.SetActive(true);
    }
}
