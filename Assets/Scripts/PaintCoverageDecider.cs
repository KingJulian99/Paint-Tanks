using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCoverageDecider : MonoBehaviour
{
    public GameObject paintableObject; // (needs to be paintable and all)
    private double[] scores; // Array holding scores for each color R, G and B. 

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print(DecideWinner());
        }
    }

    public string DecideWinner() {
        scores = new double[3];

        RenderTexture renderTex = paintableObject.GetComponent<Renderer>().material.GetTexture("_MaskTexture") as RenderTexture;
        RenderTexture.active = renderTex;
        Texture2D tex = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGB24, false); 

        tex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);

        Color32[] pixels = tex.GetPixels32();

        foreach (Color32 pixel in pixels) {
            int teamColorCodeOfPixel = GetTeamCodeColor(pixel);
            if(teamColorCodeOfPixel >= 0) {
                scores[teamColorCodeOfPixel]++;
            }
        }

        // Cleanup
        DestroyImmediate(tex);

        // Return winner!
        if(scores[0] == 0 && scores[1] == 0 && scores[2] == 0) {
            return "none";
        } else if(scores[0] >= scores[1] && scores[0] >= scores[2]) {
            return "red";
        } else if(scores[1] >= scores[0] && scores[1] >= scores[2]) {
            return "green";
        } else if(scores[2] >= scores[0] && scores[2] >= scores[1]) {
            return "blue";
        }

        return "none";
    }

    private int GetTeamCodeColor(Color32 color) {

        // r = 0, g = 1, b = 2. none = -1
        
        if(color.r == 0.0f && color.g == 0.0f && color.b == 0.0f) {
            return -1;

        } else if(color.r >= color.g) {
            if(color.r >= color.b) {
                return 0;
            } else {
                return 2;
            }
        } else {
            if(color.g >= color.b) {
                return 1;
            } else {
                return 2;
            }
        }
    }
}
