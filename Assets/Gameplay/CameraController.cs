using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float DefaultZoom = 25f;
    public float EnemyZoom = 30f;

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
        this.framingTransposer.m_CameraDistance = DefaultZoom;
    }
    public void SetEnemyZoom()
    {
        this.framingTransposer.m_CameraDistance = EnemyZoom;
    }
}
