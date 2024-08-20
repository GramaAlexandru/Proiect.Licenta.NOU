using PuzzleGame.Gameplay;
using UnityEngine;

namespace PuzzleGame
{
    [CreateAssetMenu(fileName = "GamePreset", menuName = "Game Preset")]
    public class GamePreset : ScriptableObject
    {
        public BaseGameController gamePrefab;
        public LastChance lastChance;
        public bool canBuyBoosters = true;
    }
}