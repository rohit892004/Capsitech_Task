using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public PlayerController player;

    private Vector2 startTouch;
    private Vector2 endTouch;

    public float swipeThreshold = 50f;

    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
            startTouch = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouch = Input.mousePosition;
            DetectSwipe();
        }

#else

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouch = touch.position;

            if (touch.phase == TouchPhase.Ended)
            {
                endTouch = touch.position;
                DetectSwipe();
            }
        }

#endif
    }

    void DetectSwipe()
    {
        Vector2 delta = endTouch - startTouch;

        if (delta.magnitude < swipeThreshold)
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                player.MovePlayer(Vector2Int.right);
            else
                player.MovePlayer(Vector2Int.left);
        }
        else
        {
            if (delta.y > 0)
                player.MovePlayer(Vector2Int.up);
            else
                player.MovePlayer(Vector2Int.down);
        }
    }
}