using UnityEngine;
using System.Collections;
using GGJ.Player;
using UnityEngine.UI;
using UniRx;

public class PlayerPresenter : MonoBehaviour {

    [SerializeField] Text weaponName;
    [SerializeField] Text playerName;
    [SerializeField] Slider slider;

    [SerializeField] int PlayerId = 1;

	// Use this for initialization
	void Start () {

        // プレイヤー情報を更新
        PlayerManager.Instance.OnPlayerSpawnedAsObservable
            .Where(x => x.PlayerId == PlayerId)
            .FirstOrDefault()
            .Subscribe(player =>
                {
                    // プレイヤー名を表示する
                    playerName.text = player.PlayerName;

                    var wm = player.GetComponent<WeaponManager>();

                    // 武器の名前を表示する
                    wm.CurrentWeapon
                        .Where(w => w != null)
                        .Subscribe(r => weaponName.text = r.WeaponName);

                    //武器の残弾数を画面にだす
                    wm.CurrentWeapon
                        .Where(w => w != null)
                        .SelectMany(w => w.CurrentAmmoRate.TakeUntil(w.OnFinishedAsync))
                        .Subscribe(r => slider.value = r);
                });
	}
}
