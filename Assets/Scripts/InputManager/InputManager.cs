using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : VuMonoBehaviour
{
    //public static InputManager Instance {  get; private set; }
    public PlayerControls playerControls { get; private set; }

    //[Space]
    //[Header("Joystick Move")]
    //[SerializeField] protected FixedJoystick _moveJoystick;
    //public FixedJoystick MoveJoyStick => _moveJoystick;
    //[Header("Joystick Move")]
    //[SerializeField] protected FixedJoystick _rotationJoystick;
    //public FixedJoystick RotationJoystick => _rotationJoystick;

    [SerializeField] protected Vector2 _moveInput;
    public Vector2 MoveInput=> _moveInput;
    [Space]

    [SerializeField] protected Vector3 _mouseInput;
    public Vector3 MousePosition=> _mouseInput;
    private RaycastHit lastKnowMouseHit; 

    [SerializeField] private LayerMask aimLayerMask;

    [SerializeField] protected bool _isShooting=false;
   // public bool IsShooting=> _isShooting;   
    protected override void LoadComponents()
    {
        base.LoadComponents();
        playerControls = new PlayerControls();
      // if (playerControls != null) Debug.Log("Success");
    }
    protected override void Start()
    {
        base.Start();
        this.AssignMoveInputEvents();
        this.AssignMouseInputEvents();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        playerControls.Enable();
    }



    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mouseInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnowMouseHit;
    }
    protected virtual void AssignMoveInputEvents()
    {
        //controls = player.controls;
        // Key Input
        playerControls.Character.Movement.performed += context => _moveInput = context.ReadValue<Vector2>();
        playerControls.Character.Movement.canceled += context => _moveInput = Vector2.zero;
        //playerControls.Character.Run.performed += context =>
        //{
        //    speed = runSpeed;
        //    isRunning = true;
        //};
        //playerControls.Character.Run.canceled += context => { speed = walkSpeed; isRunning = false; };
   
    }
    protected virtual void AssignMouseInputEvents()
    {
        // Mouse Input
        playerControls.Character.Aim.performed += context => _mouseInput = context.ReadValue<Vector2>();
        playerControls.Character.Aim.canceled += context => _mouseInput = Vector2.zero;

        //Fire
        playerControls.Character.Fire.performed += context => _isShooting = true;
        playerControls.Character.Fire.canceled += context => _isShooting = false;
    }
    public virtual bool IsFiring()=>_isShooting;
  
    protected override void OnDisable()
    {
        base.OnDisable();
        playerControls.Disable();
    }
}




















//protected virtual void LoadMoveJoystick()
//{
//    if (this._moveJoystick != null) return;

//    _moveJoystick = GameObject.Find("MoveJoystick").GetComponent<FixedJoystick>();
//    Debug.Log("Load MoveJoystick Success at " + this.transform.name);
//}
//protected virtual void LoadRotaJoystick()
//{
//    if (this._rotationJoystick != null) return;

//    _rotationJoystick = GameObject.Find("RotationJoystick").GetComponent<FixedJoystick>();
//    Debug.Log("Load RotationJoystick Success at " + this.transform.name);
//}

