using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{

    public Camera viewCamera;

    // Start is called before the first frame update
    void Start()
    {
        this.viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!Input.GetMouseButton(0))
            return;

        print("click!");

        RaycastHit hit;

        if (!Physics.Raycast(viewCamera.ScreenPointToRay(Input.mousePosition), out hit)) {return;}

        print("hit something");
        
        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.GetTexture("_MaskTexture") == null || meshCollider == null)
            return;
        
        print("Passes meshcollider and renderer tests");

        RenderTexture rendTex = rend.material.GetTexture("_MaskTexture") as RenderTexture;
        RenderTexture.active = rendTex;

        Texture2D tex = new Texture2D(rendTex.width, rendTex.height, TextureFormat.RGB24, false); 

        tex.ReadPixels(new Rect(0, 0, rendTex.width, rendTex.height), 0, 0);
        tex.Apply();
        
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        print(pixelUV);

        print(tex.GetPixel((int)pixelUV.x, (int)pixelUV.y));

    }
}
