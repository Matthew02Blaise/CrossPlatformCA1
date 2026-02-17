using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public FixedJoystick fixedJoystick;

    // keeps the ship inside the screen
    [SerializeField] private float padding = 0.5f;

    // prevents tiny joystick drift on mobile
    [SerializeField] private float joystickDeadzone = 0.15f;

    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _verticalInput;

    private float _minX, _maxX, _minY, _maxY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // smoother movement
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.freezeRotation = true;

        CacheCameraBounds();
    }

    private void Update()
    {
        // keyboard controls
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");

        // joystick controls (mobile)
        float joyX = 0f;
        float joyY = 0f;

        if (fixedJoystick != null)
        {
            joyX = fixedJoystick.Horizontal;
            joyY = fixedJoystick.Vertical;

            // ignore tiny finger drift
            if (Mathf.Abs(joyX) < joystickDeadzone) joyX = 0f;
            if (Mathf.Abs(joyY) < joystickDeadzone) joyY = 0f;
        }

        // combine keyboard + joystick input
        _horizontalInput = Mathf.Clamp(keyboardX + joyX, -1f, 1f);
        _verticalInput = Mathf.Clamp(keyboardY + joyY, -1f, 1f);
    }

    private void FixedUpdate()
    {
        // current position
        Vector2 pos = _rb.position;

        Vector2 input = new Vector2(_horizontalInput, _verticalInput);
        Vector2 delta = input * speed * Time.fixedDeltaTime;

        // target position
        Vector2 target = pos + delta;

        // clamp target so the ship stays on screen
        target.x = Mathf.Clamp(target.x, _minX, _maxX);
        target.y = Mathf.Clamp(target.y, _minY, _maxY);

        _rb.MovePosition(target);
    }

    private void CacheCameraBounds()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            // if no camera found, just use large bounds to avoid errors
            _minX = -999f; _maxX = 999f;
            _minY = -999f; _maxY = 999f;
            return;
        }

        // convert camera view to world space limits
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        float camX = cam.transform.position.x;
        float camY = cam.transform.position.y;

        _minX = camX - camHalfWidth + padding;
        _maxX = camX + camHalfWidth - padding;
        _minY = camY - camHalfHeight + padding;
        _maxY = camY + camHalfHeight - padding;
    }
}