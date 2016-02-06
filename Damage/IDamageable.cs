using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGJ.Damages
{
    /// <summary>
    /// ダメージを受けることができるオブジェクトに実装する
    /// </summary>
    public interface IDamageable
    {
        void ApplyDamage(Damage damage);
    }
}
