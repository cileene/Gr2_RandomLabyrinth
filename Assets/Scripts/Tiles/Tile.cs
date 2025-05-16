using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour
    {
        private PhysicsMaterial Material;

        public void Awake()
        {
            Material = CreateMaterial();  // Child defines *how* to create the material
            ApplyMaterial();              // Shared logic: assigns it to the Collider
        }

        private void ApplyMaterial()
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null && Material != null)
            {
                collider.material = Material;  // Applies the material to the object
            }
        }
        

        protected abstract PhysicsMaterial CreateMaterial();  // Forces child to define this
    }
}