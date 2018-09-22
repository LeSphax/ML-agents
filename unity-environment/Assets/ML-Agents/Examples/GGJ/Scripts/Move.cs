using UnityEngine;

public class Move : MonoBehaviour
{
    Vector3 lastpos = Vector3.zero;
    public Population parent;
    private Vector3 targetPosition;
    private bool moving = false;
    //Set by Person
    public float Speed;
    private Animator animator;

    // Update is called once per frame
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("MoveToRandom", Random.value * parent.MovementChangeRate);
    }

    private Vector3 velocity;

    private void Update()
    {

      

        if (moving)
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Speed / 2, Speed);



        //moces if distance 
        if (Vector2.Distance(gameObject.transform.position, lastpos) > 0.01f)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
        lastpos = gameObject.transform.position;
    }

    void MoveToRandom()
    {
        moving = true;
        targetPosition = parent.GetRandomPositionInBounds();
        Invoke("MoveToRandom", Random.value * parent.MovementChangeRate);
    }
}
