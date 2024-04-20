using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMaterial : MonoBehaviour
{
    void Start()
    {
        // Create a new Physics Material
        PhysicMaterial newMaterial = new PhysicMaterial();

        // Set its bounciness
        newMaterial.bounciness = 0.8f; // Adjust the bounciness value as needed

        // Optionally, set other properties like friction, etc.
        // newMaterial.friction = 0.5f;

        // Assign the material to the colliders of the objects you want to make bouncy
        GetComponent<Collider>().material = newMaterial;
    }
}
