using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "ScriptableObjects/Card", order = 1)]
public class CardBase : ScriptableObject
{
    public int value;
    public Sprite sprite;
}
