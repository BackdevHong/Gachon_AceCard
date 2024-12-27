using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public List<CharactorCard> player1Deck = new List<CharactorCard>();
    public List<CharactorCard> player2Deck = new List<CharactorCard>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    public void setPlayer1Deck(List<CharactorCard> deck)
    {
        player1Deck = deck;
    }
    
    public void setPlayer2Deck(List<CharactorCard> deck)
    {
        player2Deck = deck;
    }
}