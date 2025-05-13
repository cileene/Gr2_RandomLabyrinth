using UnityEngine;

namespace Tiles
{
    public class GrassTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var material = new PhysicsMaterial("Grass")
            {
                dynamicFriction = 1f,
                staticFriction = 1f,
                bounciness = 0f,
                frictionCombine = PhysicsMaterialCombine.Minimum,
                bounceCombine = PhysicsMaterialCombine.Minimum
            };
            return material;
        }
    }
}
