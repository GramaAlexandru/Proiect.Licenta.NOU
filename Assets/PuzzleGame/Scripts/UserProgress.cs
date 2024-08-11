using System;
using System.Collections.Generic;
using PuzzleGame.Gameplay;
using UnityEngine;

namespace PuzzleGame
{
    [Serializable]
    public class UserProgress
    {
        static UserProgress current;

        public event Action ProgressUpdate = delegate { };

        Dictionary<string, GameState> gameStates = new();

        [SerializeField]
        int coins;

        [SerializeField]
        string currentGameId;

        public static UserProgress Current
        {
            get
            {
                if (current != null)
                    return current;

                string progressJson = PlayerPrefs.GetString("UserProgress", "{}");
                Debug.Log("UserProgress : " + progressJson);
                current = JsonUtility.FromJson<UserProgress>(progressJson);

                return current;
            }
        }

        public int Coins
        {
            get => coins;
            set
            {
                coins = value;

                Save();

                ProgressUpdate.Invoke();
            }
        }

        public string CurrentGameId
        {
            get => currentGameId;
            set
            {
                currentGameId = value;

                Save();

                ProgressUpdate.Invoke();
            }
        }

        public string CurrentThemeId
        {
            get => GetGameState<GameState>(currentGameId)?.ThemeId;
            set
            {
                GameState gameState = GetGameState<GameState>(currentGameId);
                gameState.ThemeId = value;
                SetGameState(currentGameId, gameState);
                SaveGameState(currentGameId);
                Save();

                ProgressUpdate.Invoke();
            }
        }

        public T GetGameState<T>(string id) where T : GameState
        {
            if (string.IsNullOrEmpty(id))
                return null;

            if (gameStates.ContainsKey(id) && gameStates[id] is T)
                return (T) gameStates[id];

            if (!PlayerPrefs.HasKey(id))
                return null;

            if (gameStates.ContainsKey(id))
                gameStates.Remove(id);

            GameState gameState = JsonUtility.FromJson<T>(PlayerPrefs.GetString(id));
            gameStates.Add(id, gameState);

            return (T) gameState;
        }

        public void SetGameState<T>(string id, T state) where T : GameState
        {
            gameStates[id] = state;
        }

        public void SaveGameState(string id)
        {
            if (gameStates.TryGetValue(id, out var state))
                PlayerPrefs.SetString(id, JsonUtility.ToJson(state));
        }

        public void Save()
        {
            string progressJson = JsonUtility.ToJson(this, true);
            PlayerPrefs.SetString("UserProgress", progressJson);
        }
    }
}