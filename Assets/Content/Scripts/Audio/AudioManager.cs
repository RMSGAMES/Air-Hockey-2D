using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _puckCollision;
    [SerializeField] private AudioClip _goal;
    [SerializeField] private AudioClip _lostGame;
    [SerializeField] private AudioClip _wonGame;

    #region Private
    private AudioSource _audioSource;
    #endregion

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPuckCollision()
    {
        _audioSource.PlayOneShot(_puckCollision);
    }

    public void PlayGoal()
    {
        _audioSource.PlayOneShot(_goal);
    }

    public void PlayLostGame()
    {
        _audioSource.PlayOneShot(_lostGame);
    }

    public void PlayWonGame()
    {
        _audioSource.PlayOneShot(_wonGame);
    }
}
