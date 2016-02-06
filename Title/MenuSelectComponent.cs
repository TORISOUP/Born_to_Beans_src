using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GGJ.Player;
using UniRx;
using System.Linq;

public class MenuSelectComponent : MonoBehaviour
{
    [SerializeField]
    MultiPlayerInput input;
    [SerializeField]
    List<SelectableItem> selectableItems = new List<SelectableItem>();
    [SerializeField]
    GameObject selectMark;

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


	// Use this for initialization
	void Start () {
        for (int i = 0; i < selectableItems.Count; i++)
        {
            selectableItems[i].id = i;
        }

        input.MoveDirection.DistinctUntilChanged().Subscribe(dir =>{
            var inputDir = GetMoveIndex(dir);
            var currentIndex = selectIndex.Value + inputDir;
            if (inputDir != 0)
                audioSource.PlayOneShot(selectSE);
            var select = currentIndex < 0 ? selectableItems.Count -1 : currentIndex >= selectableItems.Count ? 0 : currentIndex;
            selectIndex.Value = select;
            }).AddTo(this);

        input.OnAttackButtonObservable.DistinctUntilChanged().Subscribe(attack =>
            {
                if (attack)
                {
                    var selectedItem = selectableItems.FirstOrDefault(item => item.id == selectIndex.Value);
                    if (selectedItem != null){
                        selectedItem.Invoke();
                        audioSource.PlayOneShot(enterSE);
                    }
                }
            }).AddTo(this);

        selectIndex.Subscribe(index =>
            {
                var parent = selectableItems.FirstOrDefault(item => item.id == index);

                selectMark.transform.parent = parent.transform;
                selectMark.GetComponent<RectTransform>().anchoredPosition3D = cursorOffSet;
            }).AddTo(this);
	}

    int GetMoveIndex(Vector3 inputDir)
    {
        return CalcMoveIndex((int)inputDir.x, (int)inputMovePos.x) + 
            CalcMoveIndex((int)inputDir.y, (int)inputMovePos.y) + 
            CalcMoveIndex((int)inputDir.z, (int)inputMovePos.z);
    }

    int CalcMoveIndex(int input, int move)
    {
        return input > 0 ? move : (input < 0 ? - move : 0);
    }

}
