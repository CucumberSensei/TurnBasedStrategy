using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemSingleVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRender;   
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        meshRender.enabled = false;
    }

    public void Show()
    {
        meshRender.enabled = true;
    }
}
