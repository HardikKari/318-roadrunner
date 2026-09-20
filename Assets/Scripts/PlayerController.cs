using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 8f;

    private void Update()
    {
        transform.position +=
            Vector3.forward * forwardSpeed * Time.deltaTime;
    }
}