using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
#if UNITY_EDITOR 
using UnityEditor;
#endif
 
[RequireComponent(typeof(CharacterController))]
public class BasicFPCC : MonoBehaviour
{
    [Header("Layer Mask")]
    [Tooltip("Layer Mask for sphere/raycasts. Assign the Player object to a Layer, then Ignore that layer here.")]
    public LayerMask castingMask;                            
 

    private CharacterController controller;                
    private Transform playerTx;                               
 
    [Header("Main Camera")]
    [Tooltip("Drag the FPC Camera here")]
    public Transform cameraTx;                              
 
    [Header("Optional Player Graphic")]
    [Tooltip("optional capsule to visualize player in scene view")]
    public Transform playerGFX;                            
   
    [Header("Inputs")]
    [Tooltip("Disable if sending inputs from an external script")]
    public bool useLocalInputs = true;
    [Space(5)]
    public string axisLookHorzizontal = "Mouse X";           
    public string axisLookVertical    = "Mouse Y";          
    public string axisMoveHorzizontal = "Horizontal";      
    public string axisMoveVertical    = "Vertical";            
    public KeyCode keyRun             = KeyCode.LeftShift;    
    public KeyCode keyCrouch          = KeyCode.LeftControl;  
    public KeyCode keyJump            = KeyCode.Space;      
    public KeyCode keySlide           = KeyCode.F;          
    public KeyCode keyToggleCursor    = KeyCode.BackQuote;   

    [HideInInspector] public float inputLookX        = 0;      
    [HideInInspector] public float inputLookY        = 0;      
    [HideInInspector] public float inputMoveX        = 0;      
    [HideInInspector] public float inputMoveY        = 0;      
    [HideInInspector] public bool inputKeyRun        = false;  
    [HideInInspector] public bool inputKeyCrouch     = false; 
    [HideInInspector] public bool inputKeyDownJump   = false;  
    [HideInInspector] public bool inputKeyDownSlide  = false;  
    [HideInInspector] public bool inputKeyDownCursor = false;  
   
    [Header("Look Settings")]
    public float mouseSensitivityX = 2f;           
    public float mouseSensitivityY = 2f;             
    [Tooltip("larger values for less filtering, more responsiveness")]
    public float mouseSnappiness = 20f;              
    public bool invertLookY = false;                 
    public float clampLookY = 90f;                
   
    [Header("Move Settings")]
    public float crouchSpeed = 3f;                   
    public float walkSpeed = 7f;                     
    public float runSpeed = 12f;                     
    public float slideSpeed = 14f;                  
    public float slideDuration = 2.2f;               
    public float gravity = -9.81f;                  
    public float jumpHeight = 2.5f;                  
 
    [Header("Grounded Settings")]
 

    public float groundCheckY = 0.33f;              
    public float ceilingCheckY = 1.83f;             
    [Space(5)]
    public float sphereCastRadius = 0.25f;           
    public float sphereCastDistance = 0.75f;        
    [Space(5)]
    public float raycastLength = 0.75f;              
    public Vector3 rayOriginOffset1 = new Vector3(-0.2f, 0f, 0.16f);
    public Vector3 rayOriginOffset2 = new Vector3(0.2f, 0f, -0.16f);
 
    [Header("Debug Gizmos")]
    public bool showGizmos = false;                  // Show debug gizmos and lines
 
    // - private reference variables -
    private float defaultHeight = 0;                 // reference to scale player crouch
    private float cameraStartY = 0;                  // reference to move camera with crouch
 
    [Header("- reference variables -")]
    public float xRotation = 0f;                     // the up/down angle the player is looking
    private float lastSpeed = 0;                     // reference for calculating speed
    private Vector3 fauxGravity = Vector3.zero;      // calculated gravity
    private float accMouseX = 0;                     // reference for mouse look smoothing
    private float accMouseY = 0;                     // reference for mouse look smoothing
    private Vector3 lastPos = Vector3.zero;          // reference for player velocity
    [Space(5)]
    public bool isGrounded = false;
    public float groundSlopeAngle = 0f;              // Angle of the slope in degrees
    public Vector3 groundSlopeDir = Vector3.zero;    // The calculated slope as a vector
    private float groundOffsetY = 0;                 // calculated offset relative to height
    public bool isSlipping = false;
    [Space(5)]
    public bool isSliding = false;
    public float slideTimer = 0;                     // current slide duration
    public Vector3 slideForward = Vector3.zero;      // direction of the slide
    [Space(5)]
    public bool isCeiling = false;
    private float ceilingOffsetY = 0;                
    [Space(5)]
    public bool cursorActive = false;                
 
   
    void Start()
    {
        Initialize();
    }
 
    void Update()
    {
        ProcessInputs();
        ProcessLook();
        ProcessMovement();
    }
   
    void Initialize()
    {
       
        controller = GetComponent< CharacterController >();
       
        playerTx = transform;
        defaultHeight = controller.height;
        lastSpeed = 0;
        fauxGravity = Vector3.up * gravity;
        lastPos = playerTx.position;
        cameraStartY = cameraTx.localPosition.y;
        groundOffsetY = groundCheckY;
        ceilingOffsetY = ceilingCheckY;
 
        RefreshCursor();
    }
 
    void ProcessInputs()
    {
        if ( useLocalInputs )
        {
            inputLookX = Input.GetAxis( axisLookHorzizontal );
            inputLookY = Input.GetAxis( axisLookVertical );
 
            inputMoveX = Input.GetAxis( axisMoveHorzizontal );
            inputMoveY = Input.GetAxis( axisMoveVertical );
 
            inputKeyRun        = Input.GetKey( keyRun );
            inputKeyCrouch     = Input.GetKey( keyCrouch );
 
            inputKeyDownJump   = Input.GetKeyDown( keyJump );
            inputKeyDownSlide  = Input.GetKeyDown( keySlide );
            inputKeyDownCursor = Input.GetKeyDown( keyToggleCursor );
        }
 
        if ( inputKeyDownCursor )
        {
            ToggleLockCursor();
        }
    }
 
    void ProcessLook()
    {
        accMouseX = Mathf.Lerp( accMouseX, inputLookX, mouseSnappiness * Time.deltaTime );
        accMouseY = Mathf.Lerp( accMouseY, inputLookY, mouseSnappiness * Time.deltaTime );
 
        float mouseX = accMouseX * mouseSensitivityX * 100f * Time.deltaTime;
        float mouseY = accMouseY * mouseSensitivityY * 100f * Time.deltaTime;

        xRotation += ( invertLookY == true ? mouseY : -mouseY );
        xRotation = Mathf.Clamp( xRotation, -clampLookY, clampLookY );
 
        cameraTx.localRotation = Quaternion.Euler( xRotation, 0f, 0f );
        playerTx.Rotate( Vector3.up * mouseX );
    }
 
    void ProcessMovement()
    {

        float vScale = 1f; 
        float h = defaultHeight;
        float nextSpeed = walkSpeed;
        Vector3 calc; 
        Vector3 move; 
    
        float currSpeed = ( playerTx.position - lastPos ).magnitude / Time.deltaTime;
        currSpeed = ( currSpeed < 0 ? 0 - currSpeed : currSpeed ); 
        GroundCheck();
 
        isSlipping = ( groundSlopeAngle > controller.slopeLimit ? true : false );

        if ( isGrounded && !isCeiling && inputKeyRun )
        {
            nextSpeed = runSpeed; // to run speed
        }
 
        if ( inputKeyCrouch ) // crouch
        {
            vScale = 0.5f;
            h = 0.5f * defaultHeight;
            nextSpeed = crouchSpeed; // slow down when crouching
        }  
 
        // - Slide -
 
        // if not sliding, and not stuck on ceiling, and is running
        if ( !isSliding && !isCeiling && inputKeyRun && inputKeyDownSlide ) // slide
        {
            // check velocity is faster than walkSpeed
            /*if ( currSpeed > walkSpeed )
            {
                slideTimer = 0; // start slide timer
                isSliding = true;
                slideForward = ( playerTx.position - lastPos ).normalized;
            }*/
        }
        lastPos = playerTx.position; // update reference
 
        // check slider timer and velocity
        if ( isSliding )
        {
            nextSpeed = currSpeed; // default to current speed
            move = slideForward; // set input to direction of slide
 
            slideTimer += Time.deltaTime; // slide timer
           
            // if timer max, or isSliding and not moving, then stop sliding
            if ( slideTimer > slideDuration || currSpeed < crouchSpeed )
            {
                isSliding = false;
            }
            else // confirmed player is sliding
            {
                vScale = 0.5f;            // gfx scale
                h = 0.5f * defaultHeight; // height is crouch height
                nextSpeed = slideSpeed;   // to slide speed
            }
        }
        else // - Player Move Input -
        {
            move = ( playerTx.right * inputMoveX ) + ( playerTx.forward * inputMoveY );
 
            if ( move.magnitude > 1f )
            {
                move = move.normalized;
            }
        }
 
        // - Height -
 
        // crouch/stand up smoothly
        float lastHeight = controller.height;
        float nextHeight = Mathf.Lerp( controller.height, h, 5f * Time.deltaTime );
 
        // if crouching, or only stand if there is no ceiling
        if ( nextHeight < lastHeight || !isCeiling )
        {
            controller.height = Mathf.Lerp( controller.height, h, 5f * Time.deltaTime );
 
            // fix vertical position
            calc = playerTx.position;
            calc.y += ( controller.height - lastHeight ) / 2f;
            playerTx.position = calc;
 
            // offset camera
            calc = cameraTx.localPosition;
            calc.y = ( controller.height / defaultHeight ) + cameraStartY - ( defaultHeight * 0.5f );
            cameraTx.localPosition = calc;
 
            // calculate offset
            float heightFactor = ( defaultHeight - controller.height ) * 0.5f;
           
            // offset ground check
            groundOffsetY = heightFactor + groundCheckY;
           
            // offset ceiling check
            ceilingOffsetY = heightFactor + controller.height - ( defaultHeight - ceilingCheckY );
 
            // scale gfx (optional)
            if ( playerGFX )
            {
                calc = playerGFX.localScale;
                calc.y = Mathf.Lerp( calc.y, vScale, 5f * Time.deltaTime );
                playerGFX.localScale = calc;
            }
        }
 
        // - Slipping Jumping Gravity -
 
        // smooth speed
        float speed;
       
        if ( isGrounded )
        {
            if ( isSlipping ) // slip down slope
            {
                // movement left/right while slipping down
                // player rotation to slope
                Vector3 slopeRight = Quaternion.LookRotation( Vector3.right ) * groundSlopeDir;
                float dot = Vector3.Dot( slopeRight, playerTx.right );
                // move on X axis, with Y rotation relative to slopeDir
                move = slopeRight * ( dot > 0 ? inputMoveX : -inputMoveX );
 
                // speed
                nextSpeed = Mathf.Lerp( currSpeed, runSpeed, 5f * Time.deltaTime );
 
                // increase angular gravity
                float mag = fauxGravity.magnitude;
                calc = Vector3.Slerp( fauxGravity, groundSlopeDir * runSpeed, 4f * Time.deltaTime );
                fauxGravity = calc.normalized * mag;
            }
            else
            {
                // reset angular fauxGravity movement
                fauxGravity.x = 0;
                fauxGravity.z = 0;
 
                if ( fauxGravity.y < 0 ) // constant grounded gravity
                {
                    //fauxGravity.y = -1f;
                    fauxGravity.y = Mathf.Lerp( fauxGravity.y, -1f, 4f * Time.deltaTime );
                }
            }
 
            // - Jump -
            if ( !isSliding && !isCeiling && inputKeyDownJump ) // jump
            {
                fauxGravity.y = Mathf.Sqrt( jumpHeight * -2f * gravity );
            }
 
            // --
 
            // - smooth speed -
            // take less time to slow down, more time speed up
            float lerpFactor = ( lastSpeed > nextSpeed ? 4f : 2f );
            speed = Mathf.Lerp( lastSpeed, nextSpeed, lerpFactor * Time.deltaTime );
        }
        else // no friction, speed changes slower
        {
            speed = Mathf.Lerp( lastSpeed, nextSpeed, 0.125f * Time.deltaTime );
        }
 
        // prevent floating if jumping into a ceiling
        if ( isCeiling )
        {
            speed = crouchSpeed; // clamp speed to crouched
 
            if ( fauxGravity.y > 0 )
            {
                fauxGravity.y = -1f; // 0;
            }
        }
 
        lastSpeed = speed; // update reference
 
        // - Add Gravity -
 
        fauxGravity.y += gravity * Time.deltaTime;
       
        // - Move -
 
        calc = move * speed * Time.deltaTime;
        calc += fauxGravity * Time.deltaTime;
 
        controller.Move( calc );
 
        // - DEBUG -
       

    }
 
    // lock/hide or show/unlock cursor
    public void SetLockCursor( bool doLock )
    {
        cursorActive = doLock;
        RefreshCursor();
    }
 
    void ToggleLockCursor()
    {
        cursorActive = !cursorActive;
        RefreshCursor();
    }
 
    void RefreshCursor()
    {
        if ( !cursorActive && Cursor.lockState != CursorLockMode.Locked )   { Cursor.lockState = CursorLockMode.Locked; }
        if (  cursorActive && Cursor.lockState != CursorLockMode.None   )   { Cursor.lockState = CursorLockMode.None;   }
    }
    void CeilingCheck()
    {
        //Dont do anything for now
    }
   

    void GroundCheck()
    {
     
        Vector3 origin = new Vector3( playerTx.position.x, playerTx.position.y + groundOffsetY, playerTx.position.z );
 

        RaycastHit hit;
 

        if (Physics.SphereCast(origin, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, castingMask))
        {
            groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);
 

            Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);

            groundSlopeDir = Vector3.Cross(temp, hit.normal);
 
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }   
        RaycastHit slopeHit1;
        RaycastHit slopeHit2;
 

        if (Physics.Raycast(origin + rayOriginOffset1, Vector3.down, out slopeHit1, raycastLength))
        {

            #if UNITY_EDITOR
            if (showGizmos) { Debug.DrawLine(origin + rayOriginOffset1, slopeHit1.point, Color.red); }
            #endif

            float angleOne = Vector3.Angle(slopeHit1.normal, Vector3.up);
 

            if (Physics.Raycast(origin + rayOriginOffset2, Vector3.down, out slopeHit2, raycastLength))
            {

                #if UNITY_EDITOR
                if (showGizmos) { Debug.DrawLine(origin + rayOriginOffset2, slopeHit2.point, Color.red); }
                #endif

                float angleTwo = Vector3.Angle(slopeHit2.normal, Vector3.up);

                float[] tempArray = new float[] { groundSlopeAngle, angleOne, angleTwo };
                System.Array.Sort(tempArray);
                groundSlopeAngle = tempArray[1];
            }
            else
            {
                float average = (groundSlopeAngle + angleOne) / 2;
                groundSlopeAngle = average;
            }
        }
    }
   
    void OnControllerColliderHit( ControllerColliderHit hit )
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if ( body == null || body.isKinematic )
        {
            return;
        }
        if ( hit.moveDirection.y < -0.3f )
        {
            return;
        }
 
        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.
        body.velocity = hit.moveDirection * lastSpeed;
    }
 
    // Debug Gizmos
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if ( showGizmos )
        {
            if ( !Application.isPlaying )
            {
                groundOffsetY = groundCheckY;
                ceilingOffsetY = ceilingCheckY;
            }
 
            Vector3 startPoint = new Vector3( transform.position.x, transform.position.y + groundOffsetY, transform.position.z );
            Vector3 endPoint = startPoint + new Vector3( 0, -sphereCastDistance, 0 );
            Vector3 ceilingPoint = new Vector3( transform.position.x, transform.position.y + ceilingOffsetY, transform.position.z );
 
            Gizmos.color = ( isGrounded == true ? Color.green : Color.white );
            Gizmos.DrawWireSphere( startPoint, sphereCastRadius );
 
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere( endPoint, sphereCastRadius );
 
            Gizmos.DrawLine( startPoint, endPoint );
 
            Gizmos.color = ( isCeiling == true ? Color.red : Color.white );
            Gizmos.DrawWireSphere( ceilingPoint, sphereCastRadius );
        }
    }
    #endif
}
 
 
// =======================================================================================================================================
 
// ** DELETE from here down, if menu item and auto configuration is NOT Required **
 
// this section adds create BasicFPCC object to the menu : New -> GameObject -> 3D Object
// then configures the gameobject
// demo layer used : Ignore Raycast
// also finds the main camera, attaches and sets position
// and creates capsule gfx object (for visual while editing)
 
// A using clause must precede all other elements defined in the namespace except extern alias declarations
//#if UNITY_EDITOR
//using UnityEditor;
//#endif
 
public class BasicFPCC_Setup : MonoBehaviour
{
    #if UNITY_EDITOR
 
    private static int playerLayer = 2; // default to the Ignore Raycast Layer (to demonstrate configuration)
 
    [MenuItem("GameObject/3D Object/BasicFPCC", false, 0)]
    public static void CreateBasicFPCC()
    {
        GameObject go = new GameObject( "Player" );
 
        CharacterController controller = go.AddComponent< CharacterController >();
        controller.center = new Vector3( 0, 1, 0 );
 
        BasicFPCC basicFPCC = go.AddComponent< BasicFPCC >();
 
        // Layer Mask
        go.layer = playerLayer;
        basicFPCC.castingMask = ~(1 << playerLayer);
        Debug.LogError( "** SET the LAYER of the PLAYER Object, and the LAYERMASK of the BasicFPCC castingMask **" );
        Debug.LogWarning(
            "Assign the BasicFPCC Player object to its own Layer, then assign the Layer Mask to ignore the BasicFPCC Player object Layer. Currently using layer "
            + playerLayer.ToString() + ": " + LayerMask.LayerToName( playerLayer )
        );
 
        // Main Camera
        GameObject mainCamObject = GameObject.Find( "Main Camera" );
        if ( mainCamObject )
        {
            mainCamObject.transform.parent = go.transform;
            mainCamObject.transform.localPosition = new Vector3( 0, 1.7f, 0 );
            mainCamObject.transform.localRotation = Quaternion.identity;
 
            basicFPCC.cameraTx = mainCamObject.transform;
        }
        else // create example camera
        {
            Debug.LogError( "** Main Camera NOT FOUND ** \nA new Camera has been created and assigned. Please replace this with the Main Camera (and associated AudioListener)." );
 
            GameObject camGo = new GameObject( "BasicFPCC Camera" );
            camGo.AddComponent< Camera >();
           
            camGo.transform.parent = go.transform;
            camGo.transform.localPosition = new Vector3( 0, 1.7f, 0 );
            camGo.transform.localRotation = Quaternion.identity;
 
            basicFPCC.cameraTx = camGo.transform;
        }
 
        // GFX
        GameObject gfx = GameObject.CreatePrimitive( PrimitiveType.Capsule );
        Collider cc = gfx.GetComponent< Collider >();
        DestroyImmediate( cc );
        gfx.transform.parent = go.transform;
        gfx.transform.localPosition = new Vector3( 0, 1, 0 );
        gfx.name = "GFX";
        gfx.layer = playerLayer;
        basicFPCC.playerGFX = gfx.transform;
    }
    #endif
}
 