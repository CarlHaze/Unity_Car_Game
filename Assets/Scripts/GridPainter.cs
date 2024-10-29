using UnityEngine;

public class GridPainter : MonoBehaviour
{
    public Material materialA; // First material for the grid
    public Material materialB; // Second material for the grid

    public int gridSize = 10; // Size of the grid (number of squares)
    public float unitSize = 1.0f; // Size of each grid square

    private void Start()
    {
        ApplyGrid();
    }

    private void ApplyGrid()
    {
        // Get the MeshRenderer component of the plane
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshRenderer == null || meshFilter == null)
        {
            Debug.LogError("MeshRenderer or MeshFilter is missing from the plane object.");
            return;
        }

        // Create a new material array
        Material[] materials = new Material[gridSize * gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // Check if the current position should use material A or B
                bool useMaterialA = (x + z) % 2 == 0; // Simple checkerboard pattern
                materials[x + z * gridSize] = useMaterialA ? materialA : materialB;
            }
        }

        // Apply the materials to the mesh
        meshRenderer.materials = materials;

        // Create the grid mesh if needed
        // You may create your own mesh here if you want to modify vertices, etc.
        // For a simple plane, the default mesh can be used.
    }
}
