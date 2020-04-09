using UnityEngine;
using System;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private bool _detectSwipeOnlyAfterRelease = false;
    [SerializeField] private float _minDistanceForSwipe = 20f;

    #region Private
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;
    #endregion

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Update()
    {
        SwipeDetectLogic();
    }

    private void SwipeDetectLogic()
    {
        if(UtilityHelper.isMobile)
        {
            for(int  i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    _fingerUpPosition = Input.touches[i].position;
                    _fingerDownPosition = Input.touches[i].position;
                }

                if (!_detectSwipeOnlyAfterRelease && Input.touches[i].phase == TouchPhase.Moved ||
                    Input.touches[i].phase == TouchPhase.Ended)
                {
                    _fingerDownPosition = Input.touches[i].position;
                    DetectSwipe();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _fingerUpPosition = Input.mousePosition;
                _fingerDownPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _fingerDownPosition = Input.mousePosition;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                SwipeDirection direction = _fingerDownPosition.y - _fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                SwipeDirection direction = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            _fingerUpPosition = _fingerDownPosition;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > _minDistanceForSwipe || HorizontalMovementDistance() > _minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = _fingerDownPosition,
            EndPosition = _fingerUpPosition
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}