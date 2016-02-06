using UnityEngine;
using System.Collections;

namespace GGJ.Player{
    
    public class PlayerColor : MonoBehaviour {
        [SerializeField]
        SpriteRenderer CircleSprite;
        PlayerCore player;

        public void SetPlayerColor(Color color)
        {
            CircleSprite.color = color;
        }
    }
}