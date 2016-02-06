using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ.Player
{
    public class PlayerInput : MonoBehaviour, IPlayerInput
    {
        
        private Subject<bool> onAttackButtonSubject = new Subject<bool>();

        /// <summary>
        /// 攻撃ボタンが押されているかどうか
        /// </summary>
        public IObservable<bool> OnAttackButtonObservable
        {
            get { return onAttackButtonSubject.AsObservable(); }
        }

        private Subject<Vector3> moveDirectionSubject = new Subject<Vector3>();

        /// <summary>
        /// プレイヤの移動方向
        /// </summary>
        public ReadOnlyReactiveProperty<Vector3> MoveDirection
        {
            get { return moveDirectionSubject.ToReadOnlyReactiveProperty(); }
        }

        // Use this for initialization
        private void Start()
        {
                this.UpdateAsObservable()
                    .Select(_ => Input.GetButton("Attack"))
                    .Subscribe(onAttackButtonSubject);

            this.UpdateAsObservable()
                .Select(_ => (new Vector3(
                        Input.GetAxisRaw("Horizontal"),
                        0,
                        Input.GetAxisRaw("Vertical")
                    ).normalized))
                .Subscribe(moveDirectionSubject);
        }
    }
}
