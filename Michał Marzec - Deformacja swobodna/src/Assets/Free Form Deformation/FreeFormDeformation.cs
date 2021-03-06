﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Vector3Int
{
    public int X;
    public int Y;
    public int Z;
}

public struct XYZFloatTables
{
    public float[] tableX;
    public float[] tableY;
    public float[] tableZ;
}

public class FreeFormDeformation : MonoBehaviour {

    Mesh mesh;

    public MeshFilter targetMeshFilter;

    public Vector3Int controlPointsGridSize;

    Transform controlPointsHolder;

    Vector3[] orginalVertices;

    Transform[,,] controlPoints;

    Vector3 controlPointsGridStartPosition;

    Vector3 meshSpace;

    float controlPointsOffsetY;
    Vector3 controlPointsRotOffset;

    public Vector2 minMaxUpdateRate;
    public Slider updateRateSlider;
    float currentUpdateDelay;
    float lastSliderValue;
    float nextUpdateTime;

    XYZFloatTables[] berstainPolynomialsValues;

    Vector3[] newVertsBuffer;

    public void Start()
    {
        enabled = false;
    }

    public void Inicialize() //tworzy punkty controlne wokol obiektu podpietego pod targetMeshFilter, ustawia startowe parametry
    {
        if (controlPointsHolder) //niszczy poprzednie punkty controlne
        {
            Destroy(controlPointsHolder.gameObject);
            enabled = false;
            Invoke("Inicialize", 0.1f);
            return;
        }

        mesh = targetMeshFilter.mesh;

        meshSpace = mesh.bounds.size;

        orginalVertices = mesh.vertices;

        controlPointsOffsetY = Vector3.Distance(targetMeshFilter.transform.position, targetMeshFilter.GetComponent<Collider>().bounds.center); //liczy offset miedzy pivotem obiektu a jego środkiem
        controlPointsGridStartPosition = targetMeshFilter.transform.position - mesh.bounds.size / 2;

        controlPointsRotOffset = targetMeshFilter.transform.eulerAngles;
        //Parameterize();
        CreateControlPoints();

        CalculateBerstrainPolynomialValues();

        newVertsBuffer = new Vector3[mesh.vertices.Length];

        enabled = true;
    }

    public void Reset() //Resetuje vertexy mesha do stanu poczatkowego
    {
        //mesh.vertices = orginalVertices;

        Destroy(controlPointsHolder.gameObject);
        CreateControlPoints();

        return;
    }

    public void Finish() //Nieszczy punkty kontrolne
    {
        Destroy(controlPointsHolder.gameObject);

        enabled = false;
    }

    void CreateControlPoints()
    {
        //tworzenie kontenera na punkty kontrolne
        GameObject controlPointsHolderObj = new GameObject();
        controlPointsHolderObj.name = "Control Points Holder";
        controlPointsHolder = controlPointsHolderObj.transform;
        controlPointsHolder.parent = targetMeshFilter.transform;
        controlPointsHolder.localPosition = Vector3.zero;

        //tworzenie "prefaba" punktu kontrolnego
        GameObject controlPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MeshRenderer controlPointMeshRender = controlPoint.GetComponent<MeshRenderer>();
        Material controlPointMaterial = new Material(controlPointMeshRender.material);
        controlPointMaterial.color = Color.red;
        controlPointMeshRender.material = controlPointMaterial;
        controlPoint.transform.parent = controlPointsHolder;
        controlPoint.name = "CP";

        //tworzenie punktów kontrolnych
        controlPoints = new Transform[controlPointsGridSize.X + 1, controlPointsGridSize.Y + 1, controlPointsGridSize.Z + 1];
        for (int x = 0; x <= controlPointsGridSize.X; x++)
        {
            for (int y = 0; y <= controlPointsGridSize.Y; y++)
            {
                for (int z = 0; z <= controlPointsGridSize.Z; z++)
                {
                    Transform newControlPoint = Instantiate(controlPoint, controlPointsHolder).transform;
                    newControlPoint.position = controlPointsGridStartPosition
                        + new Vector3(
                            x / (float)controlPointsGridSize.X * meshSpace.x,
                            y / (float)controlPointsGridSize.Y * meshSpace.y + controlPointsOffsetY,
                            z / (float)controlPointsGridSize.Z * meshSpace.z);
                    newControlPoint.localScale = new Vector3(0.1f, 0.1f, 0.1f) * mesh.bounds.size.magnitude / 2;
                    newControlPoint.name = "CP";
                    controlPoints[x, y, z] = newControlPoint;

                    if(x != 0 && x != controlPointsGridSize.X && //ukrywa punkty kontrolne nie będące na zewnątrze, bez nich jest bardziej czytelnie
                        y != 0 && y != controlPointsGridSize.Y &&
                        z != 0 && z != controlPointsGridSize.Z)
                    {
                        newControlPoint.gameObject.SetActive(false);
                    }
                }
            }
        }
        controlPointsHolder.transform.eulerAngles = controlPointsRotOffset;

        Destroy(controlPoint);
    }
    
    void Update()
    {
        UpdateForInharit(); //do aktualizowania wielkości gridu punktów kontrolnych przez użytkownika

        if (Time.time >= nextUpdateTime)
        {
            if (lastSliderValue != updateRateSlider.value) //aktualizuje częstotliwość aktualizacji
            {
                lastSliderValue = updateRateSlider.value;
                currentUpdateDelay = lastSliderValue * (minMaxUpdateRate.y - minMaxUpdateRate.x) + minMaxUpdateRate.x;
                currentUpdateDelay = minMaxUpdateRate.x + (minMaxUpdateRate.y - currentUpdateDelay);
            }

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                newVertsBuffer[i] = DeformVertex(mesh.vertices[i], i); //deformuje verteksy na podstawie pozycji punktów kontrolnych
            }

            mesh.vertices = newVertsBuffer;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            nextUpdateTime = Time.time + currentUpdateDelay;
        }
    }

    protected virtual void UpdateForInharit()
    {

    }

    public Vector3 DeformVertex(Vector3 vec, int orginalIndex)
    {
        Vector3 vecXYZ = Vector3.zero;
        for (int x = 0; x <= controlPointsGridSize.X; x++)
        {

            Vector3 vecYZ = Vector3.zero;
            for (int y = 0; y <= controlPointsGridSize.Y; y++)
            {

                Vector3 vecZ = Vector3.zero;
                for (int z = 0; z <= controlPointsGridSize.Z; z++)
                {
                    try
                    {
                        if (!controlPoints[x, y, z])
                            return vec;
                    }
                    catch
                    {
                        return vec;
                    }
                    vecZ += berstainPolynomialsValues[orginalIndex].tableZ[z] * (controlPoints[x, y, z].transform.localPosition - new Vector3(0, controlPointsOffsetY,0));
                }
                vecYZ += berstainPolynomialsValues[orginalIndex].tableY[y] * vecZ;
            }
            vecXYZ += berstainPolynomialsValues[orginalIndex].tableX[x] * vecYZ;
        }

        return vecXYZ;
    }

    void CalculateBerstrainPolynomialValues() //obliczanie wielomianów Berstraina dla danej wielkości gridu
    {

        berstainPolynomialsValues = new XYZFloatTables[orginalVertices.Length];

        for (int i = 0; i < orginalVertices.Length; i++)
        {

            berstainPolynomialsValues[i].tableX = new float[controlPointsGridSize.X + 1];
            berstainPolynomialsValues[i].tableY = new float[controlPointsGridSize.Y + 1];
            berstainPolynomialsValues[i].tableZ = new float[controlPointsGridSize.Z + 1];

            Vector3 diff = orginalVertices[i] + meshSpace / 2;

            Vector3 vecXYZ = Vector3.zero;
            for (int x = 0; x <= controlPointsGridSize.X; x++)
            {

                Vector3 vecYZ = Vector3.zero;
                for (int y = 0; y <= controlPointsGridSize.Y; y++)
                {

                    Vector3 vecZ = Vector3.zero;
                    for (int z = 0; z <= controlPointsGridSize.Z; z++)
                    {
                        berstainPolynomialsValues[i].tableZ[z] = bernsteinPolynomial(controlPointsGridSize.Z, z, diff.z / meshSpace.z);
                    }
                    berstainPolynomialsValues[i].tableY[y] = bernsteinPolynomial(controlPointsGridSize.Y, y, diff.y / meshSpace.y);
                }
                berstainPolynomialsValues[i].tableX[x] = bernsteinPolynomial(controlPointsGridSize.X, x, diff.x / meshSpace.x);
            }
        }
    }

    float bernsteinPolynomial(int n, int v, float x)
    {
        return binomialCoefficient(n, v) * Mathf.Pow(x, (float)v) * Mathf.Pow((float)(1.0f - x), (float)(n - v));
    }

    float binomialCoefficient(int n, int k)
    {
        float total = 1.0f;
        for (int i = 1; i <= k; i++)
        {
            total *= (n - (k - i)) / (float)i;
        }
        return total;
    }

}
