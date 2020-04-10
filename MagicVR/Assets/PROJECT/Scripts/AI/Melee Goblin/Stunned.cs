using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunned : MonoBehaviour
{

    Transform swordTarget;

    Animator anim;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

    }

    // Update is called once per frame
    void OnAnimatorIK()
    {
        if (anim)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, swordTarget.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, swordTarget.rotation);
        }
    }

    private void OnDisable()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    }
}
