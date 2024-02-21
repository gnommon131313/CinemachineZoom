using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraFreeLookZoom : MonoBehaviour
{
    private InputControls _inputControls;
    private CinemachineFreeLook _cinemachineFreeLook;

    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _acceleration = 3.0f;
    [SerializeField] private float _minRange = 3.0f;
    [SerializeField] private float _maxRange = 10.0f;

    private float _currentMiddleRigRadius = 0;
    private float _newMiddleRigRadius = 0;
    private float _axisY = 0.0f;


    private void Awake()
    {
        _inputControls = new InputControls();
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();

        SubscribeToInputControls();
    }

    private void LateUpdate()
    {
        AbjustZoomIndex();
        UpdateZoomLvl();
    }

    private void OnEnable()
    {
        _inputControls.Enable();
    }

    private void OnDisable()
    {
        _inputControls.Disable();
    }

    private void SubscribeToInputControls()
    {
        _inputControls.Camera.Zoom.performed += contex => _axisY = contex.ReadValue<float>();
        _inputControls.Camera.Zoom.canceled += context => _axisY = 0.0f;
    }

    public void AbjustZoomIndex()
    {
        if (_axisY == 0.0f) return;
        //Debug.Log(_newMiddleRigRadius);

        if (_axisY > 0.0f)
        {
            _newMiddleRigRadius = _currentMiddleRigRadius - _speed;
        }
        else
        {
            _newMiddleRigRadius = _currentMiddleRigRadius + _speed;
        }
    }

    private void UpdateZoomLvl()
    {
        if (_currentMiddleRigRadius == _newMiddleRigRadius) return;

        _currentMiddleRigRadius = Mathf.Lerp(_currentMiddleRigRadius, _newMiddleRigRadius, _acceleration * Time.deltaTime);
        _currentMiddleRigRadius = Mathf.Clamp(_currentMiddleRigRadius, _minRange, _maxRange);

        _cinemachineFreeLook.m_Orbits[1].m_Radius = _currentMiddleRigRadius;
        _cinemachineFreeLook.m_Orbits[0].m_Height = _cinemachineFreeLook.m_Orbits[1].m_Radius;
        _cinemachineFreeLook.m_Orbits[2].m_Height = -_cinemachineFreeLook.m_Orbits[1].m_Radius;
    }
}
