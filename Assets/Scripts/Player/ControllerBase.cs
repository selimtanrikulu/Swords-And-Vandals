using System.Linq;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    private CharacterController _characterController;
    private CharacterStateController _stateController;

    public float MovementSpeed { get; set; }

    public float YVelocity { get; set; }

    #region Inputs
    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    
    public float HorizontalRaw { get; set; }
    public float VerticalRaw { get; set; }
    
    public bool BasicAttackInput { get; set; }
    public bool Skill1Input { get; set; }
    public bool Skill2Input { get; set; }
    
    public bool Skill3Input { get; set; }
    public bool BlockInput { get; set; }
    public bool RollInput { get; set; }
    public bool GetStunInputTest { get; set; }
    public bool BreakStunInputTest { get; set; }
    public bool JumpInput { get; set; }
    public float RotationInput { get; set; }

    #endregion
    
 


    [HideInInspector] public CharacterStateController enemy;
    private float _userRotation;

    protected virtual void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _stateController = GetComponent<CharacterStateController>();
        MovementSpeed = MovementConfig.RunningMovementSpeed;
        
        enemy = FindObjectsOfType<CharacterStateController>().FirstOrDefault(x=>x != _stateController);
    }
    
    protected virtual void Update()
    {
        Move();
        HandleRotation();
    }
    
    private void HandleRotation()
    {
        if(enemy == null || _stateController.MovementState == MovementState.Died) return;
        Vector3 lookAt = (enemy.transform.position - transform.position);
        lookAt.y = 0;
        lookAt.Normalize();
        float rotY = Mathf.Atan2(lookAt.x, lookAt.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, rotY + _userRotation, 0);
    }

    

    private void Move()
    {
        if(_stateController.MovementState == MovementState.Stunned) return;
        
        Vector3 moveDist = new Vector3();

        switch (_stateController.MovementState)
        {
            case MovementState.Move:
                
                Transform myTransform = transform;
                Vector3 verticalMove = myTransform.forward *
                                       (Vertical * MovementConfig.RunningMovementSpeed * Time.deltaTime);
                Vector3 horizontalMove = myTransform.right *
                                         (Horizontal * MovementConfig.RunningMovementSpeed * Time.deltaTime);
                moveDist = verticalMove + horizontalMove;
                break;

            case MovementState.RollForward:
                moveDist = transform.forward * (MovementSpeed * Time.deltaTime);
                break;

            case MovementState.RollBackward:
                moveDist = -transform.forward * (MovementSpeed * Time.deltaTime);
                break;

            case MovementState.RollRight:
                moveDist = transform.right * (MovementSpeed * Time.deltaTime);
                break;

            case MovementState.RollLeft:
                moveDist = -transform.right * (MovementSpeed * Time.deltaTime);
                break;
            
            
            case MovementState.RollForwardRight:
                moveDist = (Quaternion.AngleAxis(45, Vector3.up) * transform.forward)  * (MovementSpeed * Time.deltaTime);
                break;

            case MovementState.RollForwardLeft:
                moveDist = (Quaternion.AngleAxis(-45, Vector3.up) * transform.forward) * (MovementSpeed * Time.deltaTime);
                break;

            case MovementState.RollBackwardRight:
                moveDist = (Quaternion.AngleAxis(135, Vector3.up) * transform.forward) * (MovementSpeed * Time.deltaTime);
                break;

            case MovementState.RollBackwardLeft:
                moveDist = (Quaternion.AngleAxis(-135, Vector3.up) * transform.forward) * (MovementSpeed * Time.deltaTime);
                break;


            case MovementState.Stunned:
                //nothing
                break;
        }
        
        
        
        //Apply gravity
        if (!_characterController.isGrounded)
        {
            YVelocity += MovementConfig.Gravity * Time.deltaTime;
        }


        moveDist += Vector3.up * (Time.deltaTime * YVelocity);

        _characterController.Move(moveDist);
    }
    
}
