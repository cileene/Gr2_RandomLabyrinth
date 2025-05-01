using UnityEngine;

namespace Tiles
{
    public class WoodTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var material = new PhysicsMaterial("Wood")
            {
                dynamicFriction = 0.6f,
                staticFriction = 0.5f,
                bounciness = 0f,
                frictionCombine = PhysicsMaterialCombine.Multiply,
                bounceCombine = PhysicsMaterialCombine.Average
            };
            return material;
        }
    }
}