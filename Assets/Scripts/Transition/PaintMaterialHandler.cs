using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintMaterialHandler : MonoBehaviour {


    public void SetPaintingMaterialTex(Texture2D tex)
    {
        GetComponent<Renderer>().material.mainTexture = tex;
    }
}
