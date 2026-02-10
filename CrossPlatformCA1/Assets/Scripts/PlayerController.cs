using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    public FixedJoystick fixedJoystick;

    [Header("Screen Clamp")]
    [SerializeField] private float padding = 0.5f; // tweak so ship stays fully visible

    [SerializeField] private float joystickDeadzone = 0.15f;

    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _verticalInput;

    private float _minX, _maxX, _minY, _maxY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.freezeRotation = true;

        CacheCameraBounds();
    }

    private void Update()
    {
        if (fixedJoystick == null)
        {
            _horizontalInput = 0f;
            _verticalInput = 0f;
            return;
        }

        _horizontalInput = fixedJoystick.Horizontal;
        _verticalInput = fixedJoystick.Vertical;

        // Deadzone to stop tiny drift from pinning you to an edge
        if (Mathf.Abs(_horizontalInput) < joystickDeadzone) _horizontalInput = 0f;
        if (Mathf.Abs(_verticalInput) < joystickDeadzone) _verticalInput = 0f;
    }

    private void FixedUpdate()
    {
        // Read current pos
        Vector2 pos = _rb.position;

        // Apply velocity from input
        Vector2 vel = new Vector2(_horizontalInput * speed, _verticalInput * speed);

        // Clamp position
        float clampedX = Mathf.Clamp(pos.x, _minX, _maxX);
        float clampedY = Mathf.Clamp(pos.y, _minY, _maxY);

        // If we're at the left/right edge AND still trying to move further out, stop that component
        if ((pos.x <= _minX && vel.x < 0f) || (pos.x >= _maxX && vel.x > 0f))
            vel.x = 0f;

        // If we're at the bottom/top edge AND still trying to move further out, stop that component
        if ((pos.y <= _minY && vel.y < 0f) || (pos.y >= _maxY && vel.y > 0f))
            vel.y = 0f;

        _rb.velocity = vel;
        _rb.position = new Vector2(clampedX, clampedY);
    }

    private void CacheCameraBounds()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            // Fallback: no camera found; prevents NaNs
            _minX = -999f; _maxX = 999f; _minY = -999f; _maxY = 999f;
            return;
        }

        // Orthographic camera bounds in world units
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
