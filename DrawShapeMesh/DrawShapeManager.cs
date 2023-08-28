using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawShapeManager : MonoBehaviour
{
    DonutShapeMesh DountShape;
    RectangleShapeMesh RectangleShape;
    PolygonShapeMesh PolygonShape;

    public bool isDonut=false;
    public bool isRectangle=false;
    public bool isTest = false;


    float tickTimer = 0.01f;
    float timer = 0.0f;

    void Start()
    {
        DountShape = GetComponent<DonutShapeMesh>(); 
        RectangleShape = GetComponent<RectangleShapeMesh>();
        PolygonShape = GetComponent<PolygonShapeMesh>();
    }

    void Update()
    {
        //transform.Rotate(new Vector3(0, 3.5f * Time.deltaTime, 0));
        Debug.DrawRay(this.transform.position, this.transform.forward * 3.0f);

        if(isDonut)
        {
            BoolDrawShape(DountShape);
        }

        if(isRectangle)
        {
            BoolDrawShape(RectangleShape);
        }

        if(isTest)
        {
            BoolDrawShape(PolygonShape);
        }

    }
    void BoolDrawShape(ShapeMesh shape)
    {
        shape.DrawShape(this.transform.position , this.transform.forward);

        if (tickTimer > timer)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0.0f;
            shape.fillProgress += 0.01f;
            
            if (shape.fillProgress >= 1.0f)
            {
                shape.fillProgress = 0.01f;
            }

        }
    }

}
