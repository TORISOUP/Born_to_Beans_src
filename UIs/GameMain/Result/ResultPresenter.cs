using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GGJ.GameManager;
using GGJ.Player;
using UniRx;
using UnityEngine.UI;
using System;
using GGJ.Utils;
using GGJ.RuleSelect;

public class ResultPresenter : MonoBehaviour
{

    [SerializeField]
    private GameObject resultPanel;

    //1位のテキスト
    [SerializeField]
    private Text winnerText;
    //敗者共のテキスト
    [SerializeField]
    private Text loaserText;

    void Start()
    {
              
    }

    /// <summary>
    /// GameReusltManagerが実行する
    /// </summary>
    /// <param name="results"></param>
    public void ShowResult(IEnumerable<ResultInfo> results, PlayerCore winner)
    {
        resultPanel.SetActive(true);

        var lt =
            results.Select((x, index) => string.Format("{0}位: {1}({2:F1}秒)\n", index + 2, x.PlayerCore.PlayerName, x.DeadTime))
                .Aggregate((p, c) => p + c);

        loaserText.text = lt;
        winnerText.text = string.Format("1位: {0}", winner.PlayerName);
    }

}
