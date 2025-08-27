using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool edgeScroll = true;
    [SerializeField] private bool dragToMove = true;
    [SerializeField] private float invertScroll = 1f;

    [Header("Movement")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float edgeScrollSize = 10f;
    private Vector2 mousePos;
    private Vector2 mousePosDrag;
    [SerializeField] private float dragSpeed = 0.1f;
    private Vector3 movementInput;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomStep = 0.5f;
    [SerializeField] private float targetFOV = 2.5f;
    [SerializeField] private float FOVMax = 10f;
    [SerializeField] private float FOVMin = 1f;
    private Vector2 scroll;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera virtualCamera;

    private InputSystemActions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystemActions();
    }

    private void OnEnable()
    {
        inputSystem.Player.Move.Enable();
        inputSystem.UI.ScrollWheel.Enable();
    }
    public void OnDisable()
    {
        inputSystem.Player.Move.Disable();

        inputSystem.UI.ScrollWheel.Disable();
    }
    void Update()
    {
        GetInputs();
        HandleCameraMovement();
        HandleCameraZoom();
        Move();
    }

    private void HandleCameraMovement()
    {
        if (edgeScroll) HandleCameraMovementEdgeScroll();
        if (dragToMove) HandleCameraMovementDrag(); // Fix drag to move
    }

    private void GetInputs()
    {
        movementInput = inputSystem.Player.Move.ReadValue<Vector2>();
        scroll = inputSystem.UI.ScrollWheel.ReadValue<Vector2>();
        mousePos = Mouse.current.position.ReadValue();
    }

    private void HandleCameraMovementEdgeScroll()
    {
        if (mousePos.x < edgeScrollSize) movementInput.x = -1f;
        if (mousePos.x > Screen.width - edgeScrollSize) movementInput.x = 1f;
        if (mousePos.y < edgeScrollSize) movementInput.y = -1f;
        if (mousePos.y > Screen.height - edgeScrollSize) movementInput.y = 1f;
    }

    private void HandleCameraMovementDrag()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseMoveDelta =  mousePosDrag - (Vector2)Mouse.current.position.ReadValue();
            movementInput = mouseMoveDelta.normalized * dragSpeed;
            mousePosDrag = Mouse.current.position.ReadValue();
        }
    }

    private void Move()
    {
        this.transform.position += movementInput * Time.deltaTime * speed;
    }

    private void HandleCameraZoom()
    {
        if (scroll.y > 0) {
            targetFOV -= invertScroll + zoomStep;
           
        }
        if (scroll.y < 0) {
            targetFOV += invertScroll + zoomStep;
        }
        targetFOV = Mathf.Clamp(targetFOV, FOVMin, FOVMax);

        virtualCamera.Lens.OrthographicSize = Mathf.Lerp(virtualCamera.Lens.OrthographicSize, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
