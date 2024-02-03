using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class RotateSkybox : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 1.0f;
        void Update()
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
        }
    }
}