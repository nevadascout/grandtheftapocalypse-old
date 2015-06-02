// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableAmbientLife.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Client.World
{
    using System;

    using GTA;
    using GTA.Native;

    /// <summary>
    /// Disables most ambient civilian life
    /// </summary>
    public class DisableAmbientLife : Script
    {
        public DisableAmbientLife()
        {
            this.Tick += this.OnTick;
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            this.UpdateMultipliers();
        }

        private void UpdateMultipliers()
        {
            // TODO -- NOTE: some random events still occur -> eg. cars arriving at roadside market stands
            Function.Call(Hash.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0f);
            Function.Call(Hash.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0f);
            Function.Call(Hash.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME, 0f);
            Function.Call(Hash.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME, 0f);
            Function.Call(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0f);
        }
    }
}
