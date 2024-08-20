using System;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleGame.Themes
{
    public class ThemePreview : MonoBehaviour
    {
        [SerializeField]
        Button button;

        [SerializeField]
        Image[] bricks;
        [SerializeField]
        Image[] background;

        [SerializeField]
        GameObject[] checkMarks;

        ThemePreset theme;

        public event Action<ThemePreview> Click = delegate { };

        public ThemePreset Theme
        {
            get => theme;
            set
            {
                theme = value;
                UpdateTheme();
            }
        }

        void OnClick()
        {
            Click.Invoke(this);
        }

        void UpdateTheme()
        {
            if (theme == null)
                return;

            for (int i = 0; i < bricks.Length; i++)
            {
                bricks[i].color = theme.GetColor(ColorType.Button, i + 1);
            }

            foreach (var image in background)
            {
                image.color = theme.GetColor(ColorType.Field);
            }

            foreach (GameObject checkMark in checkMarks)
            {
                checkMark.SetActive(ThemeController.Instance.CurrentTheme == theme);
            }
        }

        void OnThemeUpdate(ThemePreset themePreset)
        {
            UpdateTheme();
        }

        void Awake()
        {
            button.onClick.AddListener(OnClick);
            ThemeController.Instance.ThemeChanged += OnThemeUpdate;
        }

        void Start()
        {
            UpdateTheme();
        }

        void OnEnable()
        {
            UpdateTheme();
        }

        void OnDestroy()
        {
            ThemeController.Instance.ThemeChanged -= OnThemeUpdate;
        }
    }
}
