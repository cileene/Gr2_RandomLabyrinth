using UnityEngine;

namespace Tiles
{
    public class GrassTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var mat = new PhysicsMaterial("Grass");
            mat.dynamicFriction = 1f;
            mat.staticFriction = 1f;
            mat.frictionCombine = PhysicsMaterialCombine.Minimum;
            mat.bounceCombine = PhysicsMaterialCombine.Minimum;
            return mat;
        }
    }
}
