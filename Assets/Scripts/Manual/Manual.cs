using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manual : MonoBehaviour
{
    public enum Chapter
    {
        Spaceships,
        Components,
        Pieces,
        Machines,
        Materials,
    }

    public static event System.Action OnManualOpened = null;
    public static event System.Action OnManualClosed = null;

    [SerializeField] private Button settingsButton = null;
    [SerializeField] private Button openManualButton = null;
    [SerializeField] private Button closeManualButton = null;
    [SerializeField] private Button coverButton = null;
    [SerializeField] private Button returnToChaptersButton = null;
    [SerializeField] private GameObject chapters = null;
    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject background = null;
    [SerializeField] private GameObject backgroundFade = null;
    [SerializeField] private ChapterButton[] chapterButtons = null;
    [SerializeField] private ManualPage bodyPage = null;

    private void Start()
    {
        openManualButton.OnClick += () =>
        {
            settingsButton.SetInteractable(false);
            openManualButton.SetInteractable(false);

            backgroundFade.SetActive(true);
            coverButton.gameObject.SetActive(true);
            closeManualButton.gameObject.SetActive(true);
            OnManualOpened?.Invoke();
        };

        closeManualButton.OnClick += CloseManual;

        coverButton.OnClick += () =>
        {
            coverButton.gameObject.SetActive(false);
            chapters.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
        };

        returnToChaptersButton.OnClick += () =>
        {
            returnToChaptersButton.gameObject.SetActive(false);
            body.SetActive(false);
            chapters.gameObject.SetActive(true);
        };

        foreach (ChapterButton chapterButton in chapterButtons)
        {
            chapterButton.GetButton().OnClick += () =>
            {
                chapters.gameObject.SetActive(false);
                body.SetActive(true);
                bodyPage.SetupPage(chapterButton.GetChapter(), this);
                returnToChaptersButton.gameObject.SetActive(true);
            };
        }
    }

    public void CloseManual()
    {
        body.SetActive(false);
        chapters.SetActive(false);
        background.SetActive(false);
        backgroundFade.SetActive(false);
        coverButton.gameObject.SetActive(false);
        closeManualButton.gameObject.SetActive(false);
        returnToChaptersButton.gameObject.SetActive(false);

        settingsButton.SetInteractable(true);
        openManualButton.SetInteractable(true);
        OnManualClosed?.Invoke();
    }

    [System.Serializable]
    private struct ChapterButton
    {
        [SerializeField] private Chapter chapter;
        [SerializeField] private Button button;

        public Chapter GetChapter() => chapter;
        public Button GetButton() => button;
    }
}
