using UnityEngine;

namespace Tiles
{
    public class GrassTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var material = new PhysicsMaterial("Grass")
            {
                dynamicFriction = 100f,
                staticFriction = 100f,
                bounciness = 0f,
                frictionCombine = PhysicsMaterialCombine.Minimum,
                bounceCombine = PhysicsMaterialCombine.Minimum
            };
            return material;
        }
    }
}
