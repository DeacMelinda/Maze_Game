using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToCoinAgent : Agent
{
    [SerializeField]
    private GameObject coin;

    private Vector3 GetRandomPosition(float y)
    {
        Vector3 newPos = new Vector3(0, y, 0);
        int zone = Random.Range(0, 3);

        switch (zone)
        {
            case 0:
                newPos.x = Random.Range(-1.5f, 1.8f);
                newPos.z = Random.Range(0.5f, 7f);
                break;
            case 1:
                newPos.x = Random.Range(-1.3f, 16.5f);
                newPos.z = Random.Range(8f, 7f);
                break;
            case 2:
                newPos.x = Random.Range(11.8f, 16.6f);
                newPos.z = Random.Range(8f, 14f);
                break;
        }

        return newPos;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = GetRandomPosition(0.25f);
        coin.transform.position = GetRandomPosition(1.5f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(coin.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed * Time.deltaTime;

        transform.position += move;
    }
}
