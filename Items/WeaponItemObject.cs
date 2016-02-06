using GGJ.GameManager;
using GGJ.Weapons;
using UnityEngine;

namespace GGJ.Items
{
    /// <summary>
    /// 武器アイテムオブジェクト
    /// </summary>
    public class WeaponItemObject : MonoBehaviour, IItemObject
    {
        /// <summary>
        /// アイテムの種別
        /// </summary>
        [SerializeField]
        private  WeaponEnum _weapon;

        [SerializeField] private AudioClip soundEffect;

        public ItemType ItemType { get { return new WeaponItemType(_weapon); } }

        public void PickUp()
        {
            if (soundEffect != null)
            {
                SEPlayer.Instance.PlaySe(soundEffect);
            }
            Destroy(this.gameObject);
        }
    }
}