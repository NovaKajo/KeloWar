using UnityEngine;
using Cinemachine;


namespace Kelo.Core
{
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's X co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraLock : CinemachineExtension
{
    [Tooltip("Lock the camera's X position to this value")]
    public float m_XPosition = 0;
    private float lastYPosition = -1000000; // Set it low, incase you start low

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;

            if (pos.y > lastYPosition)
              
            pos.x = m_XPosition;
            state.RawPosition = pos;
        }
    }
}
}