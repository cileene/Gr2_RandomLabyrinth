using UnityEngine;

namespace Tiles
{
    public class SandTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var material = new PhysicsMaterial("Sand")
            {
                dynamicFriction = 0.6f,
                staticFriction = 0.9f,
                bounciness = 0f,
                frictionCombine = PhysicsMaterialCombine.Maximum,
                bounceCombine = PhysicsMaterialCombine.Average
            };
            return material;
        }
    }
}