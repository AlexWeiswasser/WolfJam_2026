using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
