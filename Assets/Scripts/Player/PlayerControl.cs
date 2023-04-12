using Cinemachine;
using UnityEngine;


public class PlayerControl : ControllerBase
{

    private new void Start()
    {
        base.Start();

        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cam)
        {
            Transform lookTransform = transform.Find("CameraLookAt");
            cam.Follow = lookTransform;
            cam.LookAt = lookTransform;
        }
    }
    
    
    private new void Update()
    {
        base.Update();
        GetInputs();

    }

    private void GetInputs()
    {
        BasicAttackInput = Input.GetKey(KeyCode.Mouse0);
        Skill1Input = Input.GetKey(KeyCode.Q);
        Skill2Input = Input.GetKey(KeyCode.E);
        Skill3Input = Input.GetKey(KeyCode.R);
        JumpInput = Input.GetKeyDown(KeyCode.Space);
        GetStunInputTest = Input.GetKeyDown(KeyCode.RightShift);
        BreakStunInputTest = Input.GetKeyDown(KeyCode.Escape);
        BlockInput = Input.GetKey(KeyCode.Mouse1);
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        HorizontalRaw = Input.GetAxisRaw("Horizontal");
        VerticalRaw = Input.GetAxisRaw("Vertical");
        RotationInput = Input.GetAxis("Mouse X");
        RollInput = Input.GetKeyDown(KeyCode.LeftShift);
    }

    
}