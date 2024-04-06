using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AttackMode
{
    Attack01,
    Attack02
}

public enum MoveMode
{
    Walk01,
    Walk02,
    Run
}

public class Skeleton : MonoBehaviour
{
    private Animator _mAnimator;
    private UnityEngine.AI.NavMeshAgent _agent;
    // Mode fields
    private AudioSource _audioSource; // Reference to the AudioSource component
    public AudioClip deathSound;
    
    public AttackMode currentAttackMode = AttackMode.Attack01;
    public MoveMode currentMoveMode = MoveMode.Walk01;

    public float walkSpeed = 5f; // Adjust as needed for appropriate movement speed

    [Header("Stats")] public float damageAmount;

    [HideInInspector] public bool alive = true;
    [HideInInspector] public bool isAttacking = false;
    private Transform target;

    // // Reference to the target. This could be set up in various ways, for example, by detecting the player in the skeleton's vicinity.
    // public Transform target;

    // Walk towards player
    private void WalkTowardsTarget()
    {
        // if not alive or if attacking, do not execute below
        if (!alive || isAttacking)
        {
            _agent.isStopped = true; // Stop the agent from moving
            return;
        }

        if (target == null)
        {
            Debug.LogError("Target not set for Skeleton.");
            return;
        }

        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y; // Keep skeleton at its current y position to avoid moving up/down
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the skeleton towards the target
        transform.position += direction * walkSpeed * Time.deltaTime;

        // Optionally rotate the skeleton to face the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            // Apply a 180-degree rotation around the Y-axis to correct the backward facing.
            // This effectively aligns the skeleton's forward direction with the target.
            toRotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, walkSpeed * Time.deltaTime * 100);
        }
        
        _agent.isStopped = false;
        _agent.SetDestination(target.position); // Set the destination of the agent to the target's position
        EnableMoveAnimation();
    }

    public void EnableMoveAnimation()
    {
        // Use currentWalkMode to set animation
        switch (currentMoveMode)
        {
            case MoveMode.Walk01:
                _mAnimator.SetBool("isWalking01", true);
                break;
            case MoveMode.Walk02:
                _mAnimator.SetBool("isWalking02", true);
                break;
            case MoveMode.Run:
                _mAnimator.SetBool("isRunning", true);
                break;
        }
    }

    // Disable Displacement
    public void DisableMoveAnimation()
    {
        switch (currentMoveMode)
        {
            case MoveMode.Walk01:
                _mAnimator.SetBool("isWalking01", false);
                break;
            case MoveMode.Walk02:
                _mAnimator.SetBool("isWalking02", false);
                break;
            case MoveMode.Run:
                _mAnimator.SetBool("isRunning", false);
                break;
        }
    }

    // Attack
    public void Attack(GameObject player)
    {
        if (alive)
        {
            isAttacking = true;
            EnableAttackAnimation();
        }
    }

    public void EnableAttackAnimation()
    {
        switch (currentAttackMode)
        {
            case AttackMode.Attack01:
                _mAnimator.SetBool("isAttacking01", true);
                break;
            case AttackMode.Attack02:
                _mAnimator.SetBool("isAttacking02", true);
                break;
        }
    }

    public void DisableAttackAnimation()
    {
        switch (currentAttackMode)
        {
            case AttackMode.Attack01:
                _mAnimator.SetBool("isAttacking01", false);
                break;
            case AttackMode.Attack02:
                _mAnimator.SetBool("isAttacking02", false);
                break;
        }
    }
    
    // Die method would play cause the skeleton to play animation and disapear after a certain period
    public void Die()
    {
        if (alive)
        {
            alive = false;
        }

        // Stop walking and play died animation
        if (_mAnimator != null)
        {
            _mAnimator.SetTrigger("died");
            DisableMoveAnimation();
            DisableAttackAnimation();
        }
        
        if (_audioSource != null && deathSound != null)
        {
            _audioSource.PlayOneShot(deathSound);
        }


        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the length of the animation. Adjust the time according to your animation's length
        yield return new WaitForSeconds(1.3f); // Assuming the death animation is about 1 second long
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Die();
        }
        else if (other.CompareTag("Target"))
        {
            // Debug.Log("Attacking!!!");
            Attack(other.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _mAnimator = GetComponentInChildren<Animator>();
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // Initialize the NavMeshAgent
        target = GameObject.FindGameObjectWithTag("Target").transform;
        _agent.speed = walkSpeed; // Set the agent's walking speed
        _audioSource = GetComponent<AudioSource>(); // Initialize the AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        WalkTowardsTarget();
    }
}