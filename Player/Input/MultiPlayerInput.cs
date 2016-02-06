using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace GGJ.Player
{
    public class MultiPlayerInput : MonoBehaviour, IPlayerInput
    {

        [SerializeField, Range(1, 4)] public int playerNumber;

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
                    .Select(_ => Input.GetButton(string.Format("AttackA_{0}", playerNumber)))
                .Subscribe(onAttackButtonSubject);

            this.UpdateAsObservable()
                .Select(_ => (new Vector3(
                    Input.GetAxisRaw(string.Format("Horizontal_{0}", playerNumber)),
                    0,
                    Input.GetAxisRaw(string.Format("Vertical_{0}", playerNumber))
                ).normalized))
                .Subscribe(moveDirectionSubject);
        }
    }
}
