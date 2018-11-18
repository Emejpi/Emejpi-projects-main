using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFDControl : FreeFormDeformation
{
    public void InicializeFromCameraRaycaster() //inicializuje Free Form Deformation po kliknieciu na mesh, obsluga klikniecia jest w CameraRaycaster
    {
        MeshFilter meshFilter = Camera.main.GetComponent<CameraRaycaster>().lastHit.GetComponent<MeshFilter>();
        if (meshFilter.gameObject.name == "CP") //sprawdza czy obiekt jest punktem controlnym
            return;

        targetMeshFilter = meshFilter;

        controlPointsGridSize = new Vector3Int { X = 1, Y = 1, Z = 1 };
        Inicialize();
    }

    protected override void UpdateForInharit() //do aktualizowania wielkości gridu punktów kontrolnych przez użytkownika
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            controlPointsGridSize.X ++;
            Inicialize();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            controlPointsGridSize.Y++;
            Inicialize();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            controlPointsGridSize.Z++;
            Inicialize();
        }
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }
}
