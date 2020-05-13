using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public GameObject deckPositon;
    public GameObject discardPilePosition;

    private List <Transform> Deck = new List<Transform>();
    private List<Transform> DiscardPile = new List<Transform>();

    private int[] myCardPull = new int[4];
    private int[] firstOpponentCardPull = new int[4];
    private int[] secondOpponentCardPull = new int[4];
    private int[] thirdOpponentCardPull = new int[4];

    
    // Start is called before the first frame update
    void Start()
    {
        int children = transform.childCount;

        for (int b = 0; b < children; b++)
        {
            transform.GetChild(b).position = deckPositon.transform.position + new Vector3(0.0f, 2.0f * b, 0.0f);
            transform.GetChild(b).rotation = deckPositon.transform.rotation;

            Deck.Add(transform.GetChild(b));
        }

        foreach (Transform child in transform)
        {
            Deck.Add(child);
        }

        shuffleDeck();

        int a = 2 + 1;
    }

    private void shuffleDeck()
    {
        var count = Deck.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = Deck[i];
            Deck[i] = Deck[r];
            Deck[r] = tmp;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
