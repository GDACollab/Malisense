using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ScentBeastAnimations : MonoBehaviour
{
    public Animator _puppetAnimator;

    private StateMachine _stateMachine;
    private Rigidbody2D _rd2d;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
        _rd2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Feed speed data to animator
        _puppetAnimator.SetFloat("Speed", _rd2d.velocity.magnitude / 3f);
    }
}
