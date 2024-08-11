using PuzzleGame.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PuzzleGame.UI
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        public float interval = 30f;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            UserProgress.Current.GetGameState<GameState>(UserProgress.Current.CurrentGameId).Reset();
            UserProgress.Current.SaveGameState(UserProgress.Current.CurrentGameId);
            UserProgress.Current.Save();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}