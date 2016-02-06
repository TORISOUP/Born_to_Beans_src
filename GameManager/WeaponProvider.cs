using System;
using System.Collections.Generic;
using GGJ.Utils;
using GGJ.Weapons;
using System.Linq;
using UnityEngine;

namespace GGJ.GameManager
{
    /// <summary>
    /// Weaponのプレハブを提供するよ！
    /// </summary>
    public class WeaponProvider : SingletonMonoBehaviour<WeaponProvider>
    {
        private string basePath = "Weapons/";
        private Dictionary<WeaponEnum, GameObject> weapons;

        void Awake()
        {
            weapons =
            Enum.GetValues(typeof(WeaponEnum)).Cast<WeaponEnum>()
                .ToDictionary(x => x, x => Resources.Load(basePath + x.ToString()) as GameObject);

            foreach (var x in weapons.Where(x => x.Value == null))
            {
                Debug.LogError(string.Format("WeaponProvider: Load failed {0}", x.Key));
            }

        }

        /// <summary>
        /// WeaponEnumから対応するWeaponを取得する
        /// </summary>

        public GameObject GetWeaponPrefab(WeaponEnum weaponEnum)
        {
            return weapons[weaponEnum];
        }

    }
}
