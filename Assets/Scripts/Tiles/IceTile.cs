using UnityEngine;

namespace Tiles
{
    public class IceTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            var material = new PhysicsMaterial("Ice");
            material.dynamicFriction = 0.05f;
            material.staticFriction = 0.05f;
            material.frictionCombine = PhysicsMaterialCombine.Minimum;
            material.bounceCombine = PhysicsMaterialCombine.Minimum;
            return material;
        }
    }
}