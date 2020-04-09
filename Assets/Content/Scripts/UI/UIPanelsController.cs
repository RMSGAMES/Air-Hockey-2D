using UnityEngine;
using System.Collections.Generic;

public class UIPanelsController : MonoBehaviour
{
    [Header("Inputs")]
    public KeyCode PauseMenuKey = KeyCode.Escape;

    [Header("References")]
    [SerializeField] private List<Panel> _panels = new List<Panel>();

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private AudioClip _click = null;

    public int currentWindow = 0;
    public int lastWindow = 0;
    public bool isMenuActive = true;

    public void ChangeWindow(int id)
    {
        lastWindow = currentWindow;
        currentWindow = id;

        ChangeWindow(id, -1);
    }

    private void ChangeWindow(int id, int id2)
    {
        for (int i = 0; i < _panels.Count; i++)
        {
            if (i == id || i == id2)
            {
                _panels[i].panel.SetActive(true);
            }
            else
            {
                _panels[i].panel.SetActive(false);
            }
        }
    }

    public void ClickSound()
    {
        if (_click != null) { PlayAudioClip(_click, 1.0f); }
    }

    private void PlayAudioClip(AudioClip clip, float volume)
    {
        if (_audioSource)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.Play();
        }
        else
        {
            Debug.LogError("No AudioSource!");
        }
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    [System.Serializable]
    public class Panel
    {
        public string panelName;
        public GameObject panel;
    }
}
