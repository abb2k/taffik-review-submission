using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cameraMovement : MonoBehaviour
{
    [BoxGroup("Movement")]
    [SerializeField] private float DefaultCameraMovementTime;
    private float CamMovementSpeed;

    [BoxGroup("Movement")]
    [SerializeField] private float DefaultCameraZoomTime;
    private float CamZoomSpeed;

    [BoxGroup("Movement")]
    [SerializeField] private float DefaultCameraZoom;
    private float CamZoom;

    [BoxGroup("Movement")]
    [SerializeField] private Transform locationObj;

    [BoxGroup("Movement")]
    [SerializeField] private bool InCamAnimation;

    private Vector2 currentPosition;

    private bool DisableInfinitAnims;

    private void Start()
    {
        if (locationObj == null)
        {
            locationObj = GameManager.getShared().player.transform;
        }
        CamMovementSpeed = DefaultCameraMovementTime;
    }

    void Update()
    {
        if (!InCamAnimation)
        {
            var position = transform.position;
            position.x = Mathf.Lerp(position.x, locationObj.position.x, DefaultCameraMovementTime * Time.deltaTime);
            position.y = Mathf.Lerp(position.y, locationObj.position.y, DefaultCameraMovementTime * Time.deltaTime);
            transform.position = position;
            Camera camera = GameManager.getShared().Camera.GetComponent<Camera>();
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, DefaultCameraZoom, DefaultCameraZoomTime * Time.deltaTime);
        }
        else
        {
            var position = transform.position;
            position.x = Mathf.Lerp(position.x, currentPosition.x, CamMovementSpeed * Time.deltaTime);
            position.y = Mathf.Lerp(position.y, currentPosition.y, CamMovementSpeed * Time.deltaTime);
            transform.position = position;
            Camera camera = GameManager.getShared().Camera.GetComponent<Camera>();
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, CamZoom, CamZoomSpeed * Time.deltaTime);
        }
    }

    public async void cameraPositionAnimation(CameraAnimationPosition[] anim)
    {
        InCamAnimation = true;

        foreach (CameraAnimationPosition frame in anim)
        {
            currentPosition = frame.position;
            switch (frame.speedType)
            {
                case CameraAnimationPosition.SpeedType.Default:
                    CamMovementSpeed = DefaultCameraMovementTime;
                    break;
                case CameraAnimationPosition.SpeedType.Custom:
                    CamMovementSpeed = frame.CustomSpeed;
                    break;
            }
            switch (frame.zoomSpeedType)
            {
                case CameraAnimationPosition.ZoomSpeedType.Default:
                    CamZoomSpeed = DefaultCameraZoomTime;
                    break;
                case CameraAnimationPosition.ZoomSpeedType.Custom:
                    CamZoomSpeed = frame.ZoomSpeed;
                    break;
            }

            CamZoom = frame.ZoomAmount;

            if (frame.Infinit)
            {
                while (true)
                {
                    if (DisableInfinitAnims)
                    {
                        break;
                    }
                    if (!Application.isPlaying)
                    {
                        break;
                    }
                    await Task.Yield();
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(frame.AnimationTime));
        }

        InCamAnimation = false;
    }

    public async void StopInfinitAnims()
    {
        DisableInfinitAnims = false;
        for (int i = 0; i < 5; i++)
        {
            await Task.Yield();
        }

        DisableInfinitAnims = true;
    }

    [System.Serializable]
    public class CameraAnimationPosition
    {
        //public CameraAnimationPosition(Vector2 _position, SpeedType _speedType, float _CustomSpeed, float _AnimationTime, bool _Infinit)
        //{
        //    position = _position;
        //    speedType = _speedType;
        //    CustomSpeed = _CustomSpeed;
        //    AnimationTime = _AnimationTime;
        //    Infinit = _Infinit;
        //}

        public Vector2 position;
        public SpeedType speedType = CameraAnimationPosition.SpeedType.Default;
        public ZoomSpeedType zoomSpeedType = CameraAnimationPosition.ZoomSpeedType.Default;
        public float CustomSpeed;
        public float ZoomSpeed;
        public float ZoomAmount;
        public float AnimationTime;
        public bool Infinit;

        public enum SpeedType
        {
            Default,
            Custom
        }
        public enum ZoomSpeedType
        {
            Default,
            Custom
        }

    }
}
