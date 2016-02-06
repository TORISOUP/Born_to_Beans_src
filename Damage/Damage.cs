using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GGJ.Damages
{
    /// <summary>
    /// ダメージオブジェクト
    /// </summary>
    public struct Damage
    {
        public Vector3 HitDirection { get; private set; }
        public float DamageValue { get; private set; }
        public IAttacker Attacker { get; private set; }

        /// <summary>
        /// Damageオブジェクト
        /// </summary>
        /// <param name="direction">吹っ飛ぶ方向</param>
        /// <param name="value">ダメージ値</param>
        public Damage(Vector3 direction, float value, IAttacker attacker)
        {
            HitDirection = direction;
            DamageValue = value;
            Attacker = attacker;
        }
    }
}
