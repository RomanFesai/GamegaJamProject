using Cinemachine;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Assets.Scripts
{
    public class IncreaseCameraTriggerWobble : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cam;
        void Start()
        {
            _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1f;
            _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1f;
        }
    }
}