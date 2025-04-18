using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    Animator animator;
    AudioSource audioSource;

    [SerializeField]
    TMPro.TextMeshProUGUI textMeshPro;


    public int coins = 0;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        textMeshPro.text = "Coins = " + coins;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            audioSource.Play();
            Destroy(collision.gameObject);
            textMeshPro.text = "Coins = " + coins;
        }
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsJumping", true);
        }

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
