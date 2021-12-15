using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float DefaultZoom = 10f;
    public float EnemyZoom = 15f;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;

    // Start is called before the first frame update
    void Start()
    {
        this.virtualCamera = GetComponent<CinemachineVirtualCamera>();
        this.framingTransposer = this.virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void SetDefaultZoom()
    {
        this.framingTransposer.m_TrackedObjectOffset = new Vector3(0f, DefaultZoom, 0f);
    }
    public void SetEnemyZoom()
    {
        this.framingTransposer.m_TrackedObjectOffset = new Vector3(0f, EnemyZoom, 0f);
    }
}
