using UnityEngine;
using UnityEngine.InputSystem;

public class DraggingCircleScript : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform arrowStartTransform;
    [SerializeField] Transform arrowEndTransform;
    [SerializeField] SpriteRenderer arrowSprite;
    [SerializeField] SpriteRenderer dottedCircle;
    [SerializeField] SpriteRenderer fadedCircle;
    bool lineVisible = false;
    public void SetPosition(Vector3 position)
    {
        arrowStartTransform.position = position;
    }

    public Vector3 GetPosition()
    {
        return arrowStartTransform.position;
    }

    public Vector2 GetScale()
    {
        return arrowStartTransform.localScale;
    }

    public void ToggleArrow(bool on)
    {
        lineRenderer.enabled = on;
        arrowSprite.enabled = on;
        dottedCircle.enabled = on;
        // fadedCircle.enabled = !on;
        lineVisible = on;
    }

    void Update()
    {
        if (!lineVisible || PauseCanvas.Paused || GameOverCanvas.GameOver) return;
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mouseWorldPos.z = 0f;
		arrowEndTransform.position = mouseWorldPos;

        Vector3 start = arrowStartTransform.position;
        Vector3 end = arrowEndTransform.position;        

		Vector3 direction = (end - start).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		arrowEndTransform.rotation = Quaternion.Euler(0, 0, angle);

		float maxArrowLength = 7.5f;
        float currentMouseLength = (end - start).magnitude;
        if (currentMouseLength > maxArrowLength)
        {
            end = start + direction * maxArrowLength;
            arrowEndTransform.position = end;
        }

		lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
