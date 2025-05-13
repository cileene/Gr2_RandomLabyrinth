using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour
    {
        private PhysicsMaterial material;

        protected void Awake()
        {
            material = CreateMaterial();  // Child defines *how* to create the material
            ApplyMaterial();              // Shared logic: assigns it to the Collider
        }

        private void ApplyMaterial()
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null && material != null)
            {
                collider.material = material;  // Applies the material to the object
            }
        }
        

        protected abstract PhysicsMaterial CreateMaterial();  // Forces child to define this
    }
}