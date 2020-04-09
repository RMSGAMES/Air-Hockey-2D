using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(1, 10)] private int _speed = 1;

    [Header("Other")]
    public AudioManager audioManager;

    #region Private
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;

    private Rigidbody2D _rb;
    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        SwipeController.OnSwipe += MovementControl;
    }

    private void MovementControl(SwipeData data)
    {
        if (_rb && !GameController.isWin)
        {
            Vector2 direction = data.StartPosition - data.EndPosition;
            _rb.AddForce(direction * _speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!GameController.isWin)
        {
            switch (coll.transform.tag)
            {
                case "Obstacle":
                    EventHandler.onGameLose();
                    _rb.isKinematic = true;
                    _rb.Sleep();
                    break;
                case "Gate":
                    GameController.isWin = true;
                    EventHandler.onGameWin();
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        audioManager.PlayPuckCollision();
    }

    public void ResetPosition(Vector2 pos)
    {
        GameController.isWin = false;

        _rb.isKinematic = false;
        _rb.WakeUp();
        _rb.position = pos;
    }
}