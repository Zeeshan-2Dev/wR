using UnityEngine;

public class FootIKHandler : MonoBehaviour
{
    [Header("IK Settings")]
    public bool enableIK = true;
    public float raycastDistance = 1.5f;
    public LayerMask groundLayer;

    [Header("Offsets")]
    public float footHeightOffset = 0.1f;
    public float pelvisOffset = 0.1f;
    public float pelvisAdjustmentSpeed = 5f;
    public float maxPelvisOffset = 0.05f;

    [Header("Speed Settings")]
    public float maxSpeed = 5f; // Set your max sprint speed

    private Animator animator;
    private Vector3 leftFootPositionIK, rightFootPositionIK;
    private Quaternion leftFootRotationIK, rightFootRotationIK;
    private float lastPelvisY;
    private Transform pelvis;

    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        pelvis = animator.GetBoneTransform(HumanBodyBones.Hips);
        lastPelvisY = pelvis.localPosition.y;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate current speed based on position change
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        currentSpeed = velocity.magnitude;
        lastPosition = transform.position;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!enableIK || animator == null)
            return;

        // Calculate IK weight based on speed
        float ikWeight = Mathf.Clamp01(1f - currentSpeed / maxSpeed);

        AdjustFeetTarget(ref leftFootPositionIK, HumanBodyBones.LeftFoot);
        AdjustFeetTarget(ref rightFootPositionIK, HumanBodyBones.RightFoot);

        MoveFeetToIKPosition(AvatarIKGoal.LeftFoot, leftFootPositionIK, leftFootRotationIK, ikWeight);
        MoveFeetToIKPosition(AvatarIKGoal.RightFoot, rightFootPositionIK, rightFootRotationIK, ikWeight);

        AdjustPelvisHeight(ikWeight);
    }

    void AdjustFeetTarget(ref Vector3 footIKPosition, HumanBodyBones foot)
    {
        Transform footTransform = animator.GetBoneTransform(foot);
        Vector3 rayOrigin = footTransform.position + Vector3.up * (raycastDistance / 2f);

        Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, Color.red);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
        {
            footIKPosition = hit.point + Vector3.up * footHeightOffset;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

            if (foot == HumanBodyBones.LeftFoot)
                leftFootRotationIK = targetRotation;
            else
                rightFootRotationIK = targetRotation;
        }
        else
        {
            footIKPosition = footTransform.position;

            if (foot == HumanBodyBones.LeftFoot)
                leftFootRotationIK = footTransform.rotation;
            else
                rightFootRotationIK = footTransform.rotation;
        }
    }

    void MoveFeetToIKPosition(AvatarIKGoal foot, Vector3 positionIK, Quaternion rotationIK, float ikWeight)
    {
        animator.SetIKPositionWeight(foot, ikWeight);
        animator.SetIKRotationWeight(foot, ikWeight);
        animator.SetIKPosition(foot, positionIK);
        animator.SetIKRotation(foot, rotationIK);
    }

    void AdjustPelvisHeight(float ikWeight)
    {
        float leftOffset = leftFootPositionIK.y - animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.y;
        float rightOffset = rightFootPositionIK.y - animator.GetBoneTransform(HumanBodyBones.RightFoot).position.y;
        float totalOffset = Mathf.Min(leftOffset, rightOffset);

        // Clamp pelvis offset to prevent unnatural stretching
        totalOffset = Mathf.Clamp(totalOffset, -maxPelvisOffset, maxPelvisOffset);

        Vector3 newPelvisPosition = pelvis.localPosition;
        float targetY = lastPelvisY + totalOffset + pelvisOffset;
        newPelvisPosition.y = Mathf.Lerp(newPelvisPosition.y, targetY, Time.deltaTime * pelvisAdjustmentSpeed * ikWeight);

        pelvis.localPosition = newPelvisPosition;
        lastPelvisY = pelvis.localPosition.y;
    }
}
