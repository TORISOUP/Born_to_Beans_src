using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GGJ.Player;
using UnityEngine.UI;
using UniRx;
namespace GGJ.UIs
{
    /// <summary>
    /// 画面に残弾を表示する奴氏
    /// </summary>
    public class WeaponAmmoPresenter : MonoBehaviour
    {
        [SerializeField]
        private int PlayerId = 1;

        void Start()
        {
            //Sliderを適当なのに変えて
            var slider = GetComponent<Slider>();

            //プレイヤが生成されたらUIをHookする
            PlayerManager.Instance.OnPlayerSpawnedAsObservable
                .Where(x => x.PlayerId == PlayerId)
                .FirstOrDefault()
                .Subscribe(player =>
                {

                    var wm = player.GetComponent<WeaponManager>();

                    //武器の残弾数を画面にだす
                    wm.CurrentWeapon
                        .Where(w => w != null)
                        .SelectMany(w => w.CurrentAmmoRate.TakeUntil(w.OnFinishedAsync))
                        .Subscribe(r => slider.value = r);
                });
        }
    }
}
