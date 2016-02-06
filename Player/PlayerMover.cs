using System.Collections;
using GGJ.Utils;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private float MoveSpeed = 1.0f;

        /// <summary>
        /// 移動用ベクトルのBuffer
        /// </summary>
        private Vector3 _moveVector3 = new Vector3();

        private Vector3 currentMoveVelocity;

        public Vector3 CurrentMoveVelocity
        {
            get { return currentMoveVelocity; }
        }

        private Vector3 GravityForce
        {
            get { return Physics.gravity; }
        }

        private IEnumerator FuttobiCoroutine(Vector3 power)
        {
            while (power.magnitude > 0.1f)
            {
                _moveVector3 += power;
                power *= 0.7f;
                yield return null;
            }
        }

        private void Start()
        {
            var cc = GetComponent<CharacterController>();
            var input = GetComponent<IPlayerInput>();
            var core = GetComponent<PlayerCore>();

            input.MoveDirection
                .TakeUntil(core.OnPlayerDeadAsObservable)
                .Where(_ => core.PlayerControllable.Value)
                .Subscribe(x => _moveVector3 += x * MoveSpeed);

            core.OnPlayerDamagedObservable
                .TakeUntil(core.OnPlayerDeadAsObservable)
                .Subscribe(x =>
                {
                    StartCoroutine(FuttobiCoroutine(x.HitDirection * x.DamageValue));
                });

            this.UpdateAsObservable()
                .TakeUntil(core.OnPlayerDeadAsObservable)
                .Subscribe(_ =>
                {
                    var moveVelocity = _moveVector3;
                    currentMoveVelocity = moveVelocity;
                    //現在のY成分のみの移動速度
                    var currentYVelocity = new Vector3(0, cc.velocity.y, 0);
                    //重力加速度加算
                    var addGravityVelocity = currentYVelocity + GravityForce * Time.deltaTime;
                    //最終的な移動速度
                    var finalMoveVelocity = moveVelocity.SetY(moveVelocity.y + addGravityVelocity.y);
                    cc.Move(finalMoveVelocity * Time.deltaTime);
                    _moveVector3 = Vector3.zero;
                });
            core.OnPlayerDeadAsObservable
                .Subscribe(_ => cc.enabled = false);
        }
    }
}
