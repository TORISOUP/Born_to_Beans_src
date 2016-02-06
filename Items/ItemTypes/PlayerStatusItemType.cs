using UnityEngine;

namespace GGJ.Items
{
    public enum PlayerStatusItemEnum
    {
        SpeedUp
    }


    class PlayerStatusItemType : ItemType
    {
        public PlayerStatusItemEnum ItemType
        {
            get { return _itemType; }
        }
        private PlayerStatusItemEnum _itemType;
    }
}
