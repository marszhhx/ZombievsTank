// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    private Animator _mAnimator;
    private bool isWalking02 = false;

    // Start is called before the first frame update
    void Start()
    {
        _mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isWalking02 = true;
            _mAnimator.SetBool("isWalking02", isWalking02); // Updated the parameter name for consistency
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isWalking02 = false;
            _mAnimator.SetBool("isWalking02", isWalking02); // Updated the parameter name for consistency
        }
    }
}
