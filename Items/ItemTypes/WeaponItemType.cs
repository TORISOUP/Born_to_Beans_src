using GGJ.Weapons;
using UnityEngine;

namespace GGJ.Items
{
    class WeaponItemType : ItemType
    {
        public WeaponEnum ItemType { get { return _itemType; } }
        private WeaponEnum _itemType;

        public WeaponItemType(WeaponEnum weaponEnum)
        {
            this._itemType = weaponEnum;
        }
    }
}
