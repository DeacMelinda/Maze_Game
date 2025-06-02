using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    Animator animator;
    AudioSource audioSource;
    MoveToCoinAgent moveToCoinAgent;
    [SerializeField]
    private GameObject coin;

    public int coins = 0;
    private float distanceToCoin;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        moveToCoinAgent = GetComponent<MoveToCoinAgent>();

        distanceToCoin = Vector3.Distance(coin.transform.position, moveToCoinAgent.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            moveToCoinAgent.SetReward(10f);
            moveToCoinAgent.EndEpisode();
            audioSource.Play();
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
            moveToCoinAgent.AddReward(-1f);
        }
    }

    private void UpdateRewards()
    {
        var newDistance = Vector3.Distance(coin.transform.position, moveToCoinAgent.transform.position);
        if (newDistance >= distanceToCoin)
        {
            moveToCoinAgent.AddReward(-1);
        }
        else
        {
            moveToCoinAgent.AddReward(1);
        }
        distanceToCoin = newDistance;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsJumping", true);
        }

        UpdateRewards();
    }

    public void JumpAnimationOver()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

}
