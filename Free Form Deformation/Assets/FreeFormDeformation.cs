using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Vector3Int
{
    public int X;
    public int Y;
    public int Z;
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

    public void Start()
    {
        enabled = false;
    }

    public void Inicialize() //tworzy punkty controlne wokol obiektu podpietego pod targetMeshFilter, ustawia startowe parametry
    {
        if (controlPointsHolder)
        {
            Destroy(controlPointsHolder.gameObject);
            enabled = false;
            Invoke("Inicialize", 0.1f);
            return;
        }

        mesh = targetMeshFilter.mesh;

        meshSpace = mesh.bounds.size;

        orginalVertices = mesh.vertices;

        controlPointsGridStartPosition = targetMeshFilter.transform.position - mesh.bounds.size / 2;
        //Parameterize();
        CreateControlPoints();

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
        controlPointMaterial.color = Color.green;
        controlPointMeshRender.material = controlPointMaterial;
        controlPoint.transform.parent = controlPointsHolder;
        controlPoint.name = "CP";

        //tworzenie punktw kontrolnych
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
                            y / (float)controlPointsGridSize.Y * meshSpace.y,
                            z / (float)controlPointsGridSize.Z * meshSpace.z);
                    newControlPoint.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    newControlPoint.name = "CP";
                    controlPoints[x, y, z] = newControlPoint;
                }
            }
        }

        Destroy(controlPoint);
    }
    
    void Update()
    {
        UpdateForInharit();

        Vector3[] newVerts = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            newVerts[i] = DeformVertex(mesh.vertices[i], i);
        }

        mesh.vertices = newVerts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    protected virtual void UpdateForInharit()
    {

    }

    public Vector3 DeformVertex(Vector3 vec, int orginalIndex)
    {
        Vector3 diff = orginalVertices[orginalIndex] + meshSpace/2;

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
                    vecZ += bernsteinPoly(controlPointsGridSize.Z, z, diff.z / meshSpace.z) * controlPoints[x, y, z].transform.localPosition;
                }
                vecYZ += bernsteinPoly(controlPointsGridSize.Y, y, diff.y / meshSpace.y) * vecZ;
            }
            vecXYZ += bernsteinPoly(controlPointsGridSize.X, x, diff.x / meshSpace.x) * vecYZ;
        }

        return vecXYZ;
    }

    float bernsteinPoly(int n, int v, float x)
    {
        return binomialCoeff(n, v) * Mathf.Pow(x, (float)v) * Mathf.Pow((float)(1.0f - x), (float)(n - v));
    }

    float binomialCoeff(int n, int k)
    {
        float total = 1.0f;
        for (int i = 1; i <= k; i++)
        {
            total *= (n - (k - i)) / (float)i;
        }
        return total;
    }

    //void Parameterize()
    //{
    //    Vector3 min = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    //    Vector3 max = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);
    //    foreach (Vector3 v in orginalVertices)
    //    {
    //        max = Vector3.Max(v, max);
    //        min = Vector3.Min(v, min);
    //    }
    //    controlPointsGridStartPosition = min;
    //    //SetMeshSpace(min, max);
    //}

    //void SetMeshSpace(Vector3 min, Vector3 max)
    //{
    //    meshSpace = new Vector3(
    //        max.x - min.x,
    //        max.y - min.y,
    //        max.z - min.z);
    //}
}
