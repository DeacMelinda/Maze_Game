using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 25, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);    
    }
}
