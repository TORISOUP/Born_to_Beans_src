using UnityEngine;
using System.Collections;
using GGJ.Debugs;
using GGJ.GameManager;
using GGJ.Items;
using GGJ.Weapons;
using UniRx;

namespace GGJ.Player
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField]
        private WeaponEnum defaultWeapon;
        private ReactiveProperty<Weapon> _currentWeapon = new ReactiveProperty<Weapon>();
        private PlayerCore core;
        private IObservable<bool> AttackObservable;

        private GameObject DefaultWeaponPrefab
        {
            get { return WeaponProvider.Instance.GetWeaponPrefab(defaultWeapon); }
        }

        public ReadOnlyReactiveProperty<Weapon> CurrentWeapon
        {
            get { return _currentWeapon.ToReadOnlyReactiveProperty(); }
        }

        void Start()
        {
            var input = GetComponent<IPlayerInput>();
            core = GetComponent<PlayerCore>();

            AttackObservable = DebugSceneMarker.Instance == null
                ? input.OnAttackButtonObservable
                    .SkipUntil(GameState.Instance.GameStateReactiveProperty.Where(x => x == GameStateEnum.GameUpdate))
                    .TakeUntil(core.OnPlayerDeadAsObservable)
                : input.OnAttackButtonObservable.TakeUntil(core.OnPlayerDeadAsObservable);

            //デフォルト装備
            SwitchWeapon(DefaultWeaponPrefab);

            core.OnPickUpItemObservable
                .TakeUntil(core.OnPlayerDeadAsObservable)
                .OfType(default(WeaponItemType))
                .Subscribe(x =>
                {
                    var waeponObject = WeaponProvider.Instance.GetWeaponPrefab(x.ItemType);
                    SwitchWeapon(waeponObject);
                });

        }

        /// <summary>
        /// Weaponを切り替える
        /// </summary>
        /// <param name="weaponObject"></param>
        void SwitchWeapon(GameObject weaponObject)
        {
            if (_currentWeapon.Value != null && _currentWeapon.Value.gameObject != null)
            {
                Destroy(_currentWeapon.Value.gameObject);
            }

            var wo = Instantiate(weaponObject, transform.position, Quaternion.identity) as GameObject;
            EquipeWeapon(wo.GetComponent<Weapon>());
        }

        void EquipeWeapon(Weapon weapon)
        {
            weapon.Initializer(core, AttackObservable);
            weapon.transform.SetParent(this.transform, true);
            weapon.transform.localPosition = new Vector3(0, 0.5f, 0.25f);
            weapon.transform.rotation = Quaternion.LookRotation(transform.forward);
            _currentWeapon.Value = weapon;
            _currentWeapon.Value.OnFinishedAsync.FirstOrDefault()
                .Subscribe(_ => SwitchWeapon(DefaultWeaponPrefab));
        }
    }
}
