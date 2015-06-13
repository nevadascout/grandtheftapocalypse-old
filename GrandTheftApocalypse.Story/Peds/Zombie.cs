// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Zombie.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.Peds
{
    using GTA;
    using GTA.Math;
    using GTA.Native;

    public class Zombie
    {
        public static Ped Spawn()
        {
            // TODO -- set random character model
            // TODO -- apply random blood texture(s)
            var zed = World.CreatePed(PedHash.JohnnyKlebitz, Game.Player.Character.Position.Around(50));
            
            var groundHeight = World.GetGroundHeight(zed.Position);
            zed.Position = new Vector3(zed.Position.X, zed.Position.Y, groundHeight);

            Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, zed);

            var blip = zed.AddBlip();
            blip.Color = BlipColor.Red;
            blip.Scale = 0.6f;

            return zed;
        }
    }
}
