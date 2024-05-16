using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private int cardValue;

    private Sprite cardSprite;

    private GameObject vfxDestroy;

    public void CardContructor(int value, Sprite sprite, GameObject vfx)
    {
        cardValue = value;

        cardSprite = sprite;

        vfxDestroy = vfx;

        GetComponent<SpriteRenderer>().sprite = cardSprite;
    }

    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].canPlayGame)
            return;

        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].PickCard(this);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void EnableCard()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public int GetCardValue()
    {
        return cardValue;
    }

    public void PlayVfx()
    {
        GameObject vfx = Instantiate(vfxDestroy, transform.position, Quaternion.identity) as GameObject;
        Destroy(vfx, 1f);
    }

}
