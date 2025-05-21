using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float weight = 10f;
    [SerializeField] private float initialRotation = 0f;
    [SerializeField] private float rayDistance = 2f;

    private float rotation = 0f;
    private Transform attackTarget;
    private Vector3 currentVelocity = Vector3.zero;
    private Animator attackAnimator;



    public void Attacking()
    {
        attackAnimator.SetBool("Attack", true);
    }

}
