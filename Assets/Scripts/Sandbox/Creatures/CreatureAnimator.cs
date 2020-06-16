using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimator : MonoBehaviour
{
    const float locomotionSmoothTime = 0.1f;

    Animator animator;
    CharacterController motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = motor.velocity.magnitude / 8f;
        animator.SetFloat("speedPercent", speedPercent, locomotionSmoothTime, Time.deltaTime);
    }
}
