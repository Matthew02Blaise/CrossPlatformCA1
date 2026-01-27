using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement Settings
    [SerializeField] private float speed = 5f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private float _horizontalInput;
    private float _verticalInput;

    public FixedJoystick fixedJoystick;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.freezeRotation = true;

        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _horizontalInput = fixedJoystick.Horizontal;
        _verticalInput = fixedJoystick.Vertical;
        //_horizontalInput = Input.GetAxisRaw("Horizontal");
        //_verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(
            _horizontalInput * speed,
            _verticalInput * speed
        );
    }
}
