using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item")]
    public class ItemSO : ScriptableObject
    {
        [Header("Properties")]
        public float cooldown;
        public ItemType itemType;
        public Sprite item_sprite;
    }

    public enum ItemType
    {
        Matches,
        Rifle,
        Flask,
        firstAid
    }
}