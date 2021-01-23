using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathLines : MonoBehaviour
{
    // Start is called before the first frame update Material mat;
    Material mat;
    Texture2D tx;


    void Start()
    {
    }

    void Stuff() {
                MeshRenderer rend;
        rend = GetComponent<MeshRenderer>();
        UnityEngine.Assertions.Assert.IsNotNull(rend);
        mat = rend.material;
        UnityEngine.Assertions.Assert.IsNotNull(mat);

        var col = GetComponent<Renderer>();
        tx = new Texture2D(128, 128,TextureFormat.ARGB32,true);
        

        // draw stuff.
        for(int y=0;y<200;y++)
        {
            for(int x=0;x<128;x++)
            {
                float a,r,g,b;
                r=g=b=a=0f;
                if( x<20 || y<20 || x>108 || y>108 )
                    {a=1.0f;r=g=b=0.75f;}
                else
                    {a=0.5f;r=b=0.25f+(x/256.0f);g=0.25f+(y/256.0f);}
                tx.SetPixel(x,y,new Color(r,g,b,a));
            }
            tx.Apply(true); // now really load all those pixels.
        }

        mat.mainTexture = tx;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
