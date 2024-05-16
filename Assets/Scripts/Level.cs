using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

public class Level : MonoBehaviour
{
    public List<GameObject> gameObjects;

    public List<Transform> transforms = new List<Transform>();

    public List<Transform> transforms2 = new List<Transform>();

    public List<Card> cards = new List<Card>();

    public List<Card> cardsPicked = new List<Card>();

    public List<Card> cardsPool = new List<Card>();

    public List<CardBase> cardBases = new List<CardBase>();

    public List<GameObject> vfxs = new List<GameObject>();

    public Transform cardPos;

    public GameObject cardBasePrefab;

    public bool canPlayGame;

    public int maxPoint;

    public int currentPoint;

    public TextMeshProUGUI text;

    private void Start()
    {
        maxPoint = 50;

        currentPoint = 0;

        CreateCard();

        canPlayGame = false;

        StartCoroutine(CardGo());

        SetText();
    }

    private void Update()
    {
 
    }

    private void SetText()
    {
        text.SetText("{0} / {1}", currentPoint, maxPoint);
    }

    private void CreateCard()
    {
        for(int i = 0; i < cardBases.Count; i++)
        {
            GameObject card = Instantiate(cardBasePrefab, cardPos.position, Quaternion.identity) as GameObject;

            CardBase cardBase = cardBases[Random.Range(0, cardBases.Count - 1)];

            card.GetComponent<Card>().CardContructor(cardBase.value, cardBase.sprite, vfxs[Random.Range(0, vfxs.Count - 1)]);

            cards.Add(card.GetComponent<Card>());
        }
    }

    public void PickCard(Card card)
    {
        if (!cardsPicked.Contains(card))
        {
            cardsPool.Remove(card);

            cardsPicked.Add(card);

            card.transform.DOMove(transforms[cardsPicked.IndexOf(card)].position, 1f).OnComplete(() => {
                CalculatePoint();
            });
        }
    }

    private void CalculatePoint()
    {
        if (cardsPicked.Count < 4)
            return;

        canPlayGame = false;

        foreach (Card card in cardsPicked)
        {
            currentPoint += card.GetCardValue();
            card.PlayVfx();
            card.gameObject.SetActive(false);
            SetText();
        }

        if (!CheckPoint())
        {
            cardsPicked.Clear();

            StartCoroutine(CardGo());
        }
    }

    private bool CheckPoint()
    {
        if (currentPoint >= maxPoint)
        {
            GameManager.Instance.CheckLevelUp();
            return true;
        }

        return false;
    }

    private IEnumerator CardGo()
    {
        if (cardsPool.Count == 0)
        {
            for(int i = 0; i < transforms2.Count; i++)
            {
                var tr = cards[i];
                cardsPool.Add(tr);
                tr.transform.DOMove(transforms2[cardsPool.IndexOf(tr)].position, 1f).OnComplete(() =>
                {
                    tr.EnableCard();
                });
                yield return new WaitForSeconds(0.25f);
            }

            foreach(Card card in cardsPool)
            {
                cards.Remove(card);
            }

            canPlayGame = true;
        }
        else
        {
            foreach(Card card in cardsPool)
            {
                card.gameObject.transform.DOMove(transforms2[cardsPool.IndexOf(card)].position, 1f);
            }

            int missingCards = transforms2.Count - cardsPool.Count;

            if (missingCards > cards.Count)
            {
                missingCards = cards.Count;
            }

            for (int i = 0; i < missingCards; i++)
            {
                var tr = cards[i];
                cardsPool.Add(tr);
                tr.transform.DOMove(transforms2[cardsPool.IndexOf(tr)].position, 1f).OnComplete(() =>
                {
                    tr.EnableCard();
                });
                yield return new WaitForSeconds(0.25f);
            }

            foreach (Card card in cardsPool)
            {
                cards.Remove(card);
            }

            canPlayGame = true;
        }


    }
}
