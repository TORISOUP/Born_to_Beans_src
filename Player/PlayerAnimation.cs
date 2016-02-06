using System;
using UnityEngine;
using System.Collections;
using GGJ.Utils;
using UniRx;

namespace GGJ.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;
        private bool IsRunning
        {
            set { animator.SetBool("IsRunning", value); }
        }


        void Start()
        {
            animator = GetComponent<Animator>();
            var cc = GetComponent<CharacterController>();
            var core = GetComponent<PlayerCore>();

            //ダメージを受けてアニメーション処理を中断している状態
            var isAnimationDisabled = false;

            //ダメージ受けたとき
            core.OnPlayerDamagedObservable
                .Do(_ =>
                {
                    animator.Play("Damage");
                    IsRunning = false;
                    isAnimationDisabled = true;
                })
                //プレイヤが操作可能状態に戻るのを待つ
                .SelectMany(_ => core.PlayerControllable.Where(x => x))
                .FirstOrDefault()
                .RepeatUntilDestroy(this.gameObject)
                .TakeUntil(core.OnPlayerDeadAsObservable)
                .Subscribe(_ => isAnimationDisabled = false);

            //プレイヤの移動を監視してアニメーションを変える
            cc.ObserveEveryValueChanged(x => x.velocity)
                .TakeUntil(core.OnPlayerDeadAsObservable)
                .Where(_ => !isAnimationDisabled)
                .Subscribe(x =>
                {
                    var speed = x.magnitude;
                    //走りモーション
                    IsRunning = speed > 0.1f;

                    //プレイヤの向き
                    if (!(speed > 0.1f)) return;
                    var forward = x.SuppressY();
                    if (!(forward.magnitude > 0)) return;
                    var lookRotation = Quaternion.LookRotation(forward);
                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        lookRotation,
                        Time.deltaTime * 20.0f
                        );
                }).AddTo(this.gameObject);

            core.OnPlayerDeadAsObservable
                .Subscribe(_ => animator.Play("Dead"));

        }

        void OnCallChangeFace(string face)
        {

        }
    }
}