using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GGJ.Player;
using UniRx;
using System.Linq;
using GGJ.Utils;

namespace GGJ.RuleSelect
{
    /// <summary>
    /// Multi menu select controller.
    /// インプットの音、操作をやる。
    /// </summary>
    public class MultiMenuSelectController : SingletonMonoBehaviour<MultiMenuSelectController>
    {
        [SerializeField]
        MultiPlayerInput input;
        [SerializeField]
        GameObject selectMark;

        [SerializeField]
        List<SelectableItemList> MenuList = new List<SelectableItemList>();
        [SerializeField]
        int defaultMenuId = 0;
        public ReactiveProperty<int> CurrentMenuId = new ReactiveProperty<int>(0);
        public SelectableItemList CurrentMenu { get { return MenuList.FirstOrDefault(item => item.id == CurrentMenuId.Value); } }

        ReactiveProperty<int> selectIndex = new ReactiveProperty<int>(0);
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip selectSE;
        [SerializeField]
        AudioClip enterSE;

        [SerializeField]
        Vector3 cursorOffSet = Vector3.zero;
        [SerializeField]
        Vector3 inputMovePos = new Vector3(0, 0, -1);

        void Start()
        {
            CurrentMenuId.Where(id => id < 2).Subscribe(id =>
                    {
                        CurrentMenu.Initialize();
                        selectIndex.SetValueAndForceNotify(0);
                    }).AddTo(this);


            selectIndex.Subscribe(index =>
                {
                    var parent = GetSelectableItemIndex(index);

                    selectMark.transform.parent = parent.transform;
                    selectMark.GetComponent<RectTransform>().anchoredPosition3D = cursorOffSet;
                    selectMark.transform.localScale = Vector3.one;
                }).AddTo(this);


            input.MoveDirection.DistinctUntilChanged().Subscribe(dir =>
            {
                var inputDir = GetMoveIndex(dir);
                var currentIndex = selectIndex.Value + inputDir;
                if (inputDir != 0)
                    audioSource.PlayOneShot(selectSE);

                var selectableItems = CurrentMenu.selectableItems;

                var select = currentIndex < 0 ? selectableItems.Count - 1 : currentIndex >= selectableItems.Count ? 0 : currentIndex;
                selectIndex.Value = select;
            }).AddTo(this);

            input.OnAttackButtonObservable.DistinctUntilChanged().Subscribe(attack =>
                {
                    if (attack)
                    {
   
                        var selectedItem = CurrentMenu.selectableItems.FirstOrDefault(item => item.id == selectIndex.Value);
                        if (selectedItem != null)
                        {
                            selectedItem.Invoke();
                            audioSource.PlayOneShot(enterSE);
                        }
                    }
                }).AddTo(this);

        }

        SelectableItem GetSelectableItemIndex(int index)
        {
            return CurrentMenu.selectableItems.FirstOrDefault(item => item.id == index);
        }

        int GetMoveIndex(Vector3 inputDir)
        {
            return CalcMoveIndex((int)inputDir.x, (int)inputMovePos.x) +
                CalcMoveIndex((int)inputDir.y, (int)inputMovePos.y) +
                CalcMoveIndex((int)inputDir.z, (int)inputMovePos.z);
        }

        int CalcMoveIndex(int input, int move)
        {
            return input > 0 ? move : (input < 0 ? -move : 0);
        }

        public void Back()
        {
            if (CurrentMenuId.Value > 0)
            {
                CurrentMenuId.Value--;
            }

        }

        public void Next()
        {
            if (CurrentMenuId.Value < MenuList.Count)
            {
                CurrentMenuId.Value++;
            }
        }

    }
}