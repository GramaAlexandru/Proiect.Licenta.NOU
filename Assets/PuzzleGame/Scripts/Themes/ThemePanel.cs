using System.Collections.Generic;
using PuzzleGame.UI;
using UnityEngine;

namespace PuzzleGame.Themes
{
    public class ThemePanel : Panel
    {
        [SerializeField]
        ThemePreview themePreviewPrefab;
        [SerializeField]
        Transform themePreviewParent;

        [SerializeField]
        GameObject playButton;
        readonly List<ThemePreview> previews = new List<ThemePreview>();

        ThemePreset lastAvailableTheme;
        int maxPresetsCount = 8;
    
        void Start()
        {
            for (int i = 0; i < maxPresetsCount; i++)
            {
                ThemePreview themePreview = Instantiate(themePreviewPrefab, themePreviewParent);
                themePreview.Click += OnThemeClick;
                themePreview.gameObject.SetActive(false);
                previews.Add(themePreview);
            }
        }

        protected override void Show()
        {
            UpdatePresets();
            content.SetActive(true);
        }

        protected override void Hide()
        {
            content.SetActive(false);
        
            foreach (ThemePreview preview in previews)
                preview.gameObject.SetActive(false);

            ThemeController.Instance.CurrentTheme = lastAvailableTheme;
            ThemeController.Instance.SaveCurrentTheme();
        }

        void UpdateButtons()
        {
            ThemePreset theme = ThemeController.Instance.CurrentTheme;

            playButton.SetActive(false);

            lastAvailableTheme = theme;
            playButton.SetActive(true);
        }

        void OnThemeClick(ThemePreview themePreview)
        {
            ThemeController.Instance.CurrentTheme = themePreview.Theme;
            UpdateButtons();
        }

        void UpdatePresets()
        {
            ThemePreset[] themePresets = ThemeController.Instance.GetThemes();
            for (int i = 0; i < themePresets.Length; i++)
            {
                previews[i].Theme = themePresets[i];
                previews[i].gameObject.SetActive(true);
            }

            UpdateButtons();
        }
    }
}
