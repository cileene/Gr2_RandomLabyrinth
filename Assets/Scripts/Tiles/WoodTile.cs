using UnityEngine;

namespace Tiles
{
    public class WoodTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var material = new PhysicsMaterial("Wood")
            {
                dynamicFriction = 1f,
                staticFriction = 1f,
                bounciness = 0f,
                frictionCombine = PhysicsMaterialCombine.Multiply,
                bounceCombine = PhysicsMaterialCombine.Average
            };
            return material;
        }
    }
}