using UniRx;
using UnityEngine;

namespace GGJ.Player
{
    /// <summary>
    /// プレイヤの入力周りインターフェイス
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>
        /// 攻撃ボタンが押されているかどうか
        /// </summary>
        IObservable<bool> OnAttackButtonObservable { get; }

        /// <summary>
        /// プレイヤの移動方向
        /// </summary>
        ReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    }
}
