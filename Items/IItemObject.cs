using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGJ.Items
{
    /// <summary>
    /// アイテムオブジェクトをであることを表すインターフェイス
    /// </summary>
    interface IItemObject
    {
        ItemType ItemType { get; }
        void PickUp();
    }
}
