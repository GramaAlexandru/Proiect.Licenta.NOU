using System;
using PuzzleGame.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleGame
{
    public class GameSelector : MonoBehaviour
    {
        [SerializeField] GamePresetsList gamePresetsList;

        [SerializeField] GameObject navigation;
        [SerializeField] GameObject fieldBlocker;

        [SerializeField] Button next;
        [SerializeField] Button previous;
        [SerializeField] Toggle togglePrefab;
        [SerializeField] Transform togglesParent;

        [SerializeField] int highlightSortingOrder;

        int currentGameIndex;
        BaseGameController currentGame;
    
        Toggle[] toggles;

        static readonly int BigField = Animator.StringToHash("Big");
        static readonly int MiddleField = Animator.StringToHash("Middle");
        static readonly int SmallField = Animator.StringToHash("Small");

        public void MinimizeCurrentGame(bool value)
        {
            if (!value)
            {
                MaximizeCurrentGame();
                return;
            }

            Time.timeScale = 0;
            ResetTriggers();
            currentGame.fieldAnimator.SetTrigger(SmallField);
            navigation.SetActive(false);
        }

        void MaximizeCurrentGame()
        {
            ResetTriggers();
            currentGame.fieldAnimator.SetTrigger(MiddleField);
            navigation.SetActive(true);
            fieldBlocker.SetActive(true);
        }

        void ResetTriggers()
        {
            currentGame.fieldAnimator.ResetTrigger(BigField);
            currentGame.fieldAnimator.ResetTrigger(MiddleField);
            currentGame.fieldAnimator.ResetTrigger(SmallField);
        }

        void OnNextClick()
        {
            currentGameIndex++;
            currentGameIndex %= gamePresetsList.presets.Length;

            UpdateCurrentGame();
        }

        void OnPreviousClick()
        {
            currentGameIndex--;
            if (currentGameIndex < 0)
                currentGameIndex += gamePresetsList.presets.Length;

            UpdateCurrentGame();
        }

        void UpdateCurrentGame()
        {
            if (currentGame)
                DestroyImmediate(currentGame.gameObject);

            currentGame = Instantiate(gamePresetsList.presets[currentGameIndex].gamePrefab);
            currentGame.name = gamePresetsList.presets[currentGameIndex].name;
            UserProgress.Current.CurrentGameId = currentGame.name;
        
            currentGame.gameObject.AddComponent<SetCameraToCanvas>();
            currentGame.HighlightSortingOrder = highlightSortingOrder;

            if (toggles != null && toggles.Length > 0)
            {
                for (int i = 0; i < toggles.Length; i++)
                    toggles[i].isOn = i == currentGameIndex;
            }
        
            Time.timeScale = 1;

            fieldBlocker.SetActive(false);

            currentGame.GameOver += OnGameOver;
        }

        void OnGameOver()
        {
            ResetTriggers();
            currentGame.fieldAnimator.SetTrigger(MiddleField);
            fieldBlocker.SetActive(true);
        }
    
        void Awake()
        {
            currentGameIndex = Array.FindIndex(gamePresetsList.presets, g => g.name == UserProgress.Current.CurrentGameId);

            if (currentGameIndex < 0)
                currentGameIndex = 0;

            bool multipleModesAvailable = gamePresetsList.presets.Length > 1;

            if (multipleModesAvailable)
            {
                toggles = new Toggle[gamePresetsList.presets.Length];

                for (int i = 0; i < gamePresetsList.presets.Length; i++)
                    toggles[i] = Instantiate(togglePrefab, togglesParent);
            }
        
            UpdateCurrentGame();

            next.gameObject.SetActive(multipleModesAvailable);
            next.onClick.AddListener(OnNextClick);

            previous.gameObject.SetActive(multipleModesAvailable);
            previous.onClick.AddListener(OnPreviousClick);
        }
    }
}
