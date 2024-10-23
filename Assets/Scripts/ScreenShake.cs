using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{   
    public static ScreenShake Instance { get; private set; }
    
    private CinemachineImpulseSource impulseSource;
    private void Awake()
    {   
        if (Instance != null)
        {
            Debug.LogError("There is more than one ScreenShake" + transform);
            return;
        }
        Instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
            
    public void Shake(float intensity = 1f)
    {
        impulseSource.GenerateImpulse(intensity);
    }
}
