using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GGJ.Stage;

namespace GGJ.RuleSelect {
    public class GameMatchSetting
    {
        public enum PlayMode
        {
            OneOnOne,
            TriBattleRoyale,
            BattleRoyale,
        }
        
        private static GameMatchSetting instance;
        public static GameMatchSetting Instance{
            get {
                if (instance == null)
                    instance = new GameMatchSetting();
                return instance;
            }
            private set { instance = value; }
        }

        public Dictionary<int, PlayerSetting> PlayerSettings = new Dictionary<int, PlayerSetting>(){
            { 1, new PlayerSetting( 1, 1) },
            { 2, new PlayerSetting( 2, 2) }
        };
        public PlayMode CurrentMode { get; private set; }
        public StageEnum SelectedStageType { get; private set; }
        public int CurrentModePlayerLimit{
            get{
                if (CurrentMode == PlayMode.OneOnOne)
                    return 2;
                else if (CurrentMode == PlayMode.TriBattleRoyale)
                    return 3;
                else if (CurrentMode == PlayMode.BattleRoyale)
                    return 4;
                return 0;

            }
        }

        private GameMatchSetting()
        {
            
        }

        public void SetPlayMode(PlayMode mode)
        {
            CurrentMode = mode;
        }

        public void SetPlayer(int playerNumber, int characterType)
        {
            if (PlayerSettings.ContainsKey(playerNumber))
            {
                PlayerSettings[playerNumber].playerNumber = playerNumber;
                PlayerSettings[playerNumber].characterType = characterType;
            }
            else
            {
                PlayerSettings.Add(playerNumber, new PlayerSetting(playerNumber, characterType));
            }
        }

        public void SetupNextStage()
        {
            int nextStageId = (int)SelectedStageType + 1;
            int stageLimit = Enum.GetValues(typeof(StageEnum)).Length;
            nextStageId = nextStageId >= stageLimit ? 0 : nextStageId < 0 ? stageLimit : nextStageId;
            SelectedStageType = (StageEnum) Enum.ToObject(typeof(StageEnum), nextStageId);
        }
        public void SetStage(StageEnum stage)
        {
            SelectedStageType = stage;
        }

    }

    public class PlayerSetting
    {
        public int playerNumber;
        public int characterType;
        public PlayerSetting(int number, int type){
            playerNumber = number;
            characterType = type;
        }
    }
}