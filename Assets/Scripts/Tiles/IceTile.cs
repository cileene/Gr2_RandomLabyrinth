using UnityEngine;

namespace Tiles
{
    public class IceTile : Tile
    {
        protected override PhysicsMaterial CreateMaterial()
        {
            Debug.Log("IceTile: Creating ice material");
            var material = new PhysicsMaterial("Ice")
            {
                dynamicFriction = 0f,
                staticFriction = 0f,
                frictionCombine = PhysicsMaterialCombine.Multiply,
                bounceCombine = PhysicsMaterialCombine.Average
                
            };
            return material;
        }
    }
}