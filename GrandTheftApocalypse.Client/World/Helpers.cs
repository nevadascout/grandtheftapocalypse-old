namespace GrandTheftApocalypse.Client.World
{
    using System.Drawing;

    using GrandTheftApocalypse.Client.Internal;

    using GTA;
    using GTA.Math;
    using GTA.Native;

    public class Helpers : Script
    {
        /// <summary>
        /// Create a prop on the ground in front of a player
        /// </summary>
        public static void CreatePropInFrontOfPlayer(string propName)
        {
            Logger.Log("Trying to spawn prop '" + propName + "'");

            var model = new Model(propName);
            model.Request(250);

            if (model.IsInCdImage && model.IsValid)
            {
                while (!model.IsLoaded) Script.Wait(50);

                var prop = World.CreateProp(model, Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)), true, true);

                // Move the fire down so it appears on the ground instead of floating
                if (propName == "prop_beach_fire")
                {
                    prop.Position = new Vector3(prop.Position.X, prop.Position.Y, prop.Position.Z - 1f);
                }

                Logger.Log("Prop '" + propName + "' spawn success");
            }

            model.MarkAsNoLongerNeeded();
        }

        /// <summary>
        /// Create a drivable vehicle in the game world
        /// Call this method when loading vehicles from the database
        /// </summary>
        public static Vehicle CreateDrivableVehicle(VehicleHash vehicleHash, Vector3 position, Color color, float heading = 0)
        {
            var vehicle = World.CreateVehicle(vehicleHash, position, heading);

            vehicle.DirtLevel = 15f;
            vehicle.IsPersistent = true;
            vehicle.CanTiresBurst = true;

            vehicle.CustomPrimaryColor = color;
            vehicle.CustomSecondaryColor = color;

            // vehicle.NumberPlate = "NEVADA";
            vehicle.PlaceOnGround();

            // Add front winch + spare tyre if dubsta
            if (vehicleHash == VehicleHash.Dubsta)
            {
                Function.Call(Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
                vehicle.SetMod(VehicleMod.FrontBumper, 3, true);
                vehicle.SetMod(VehicleMod.RearBumper, 1, true);
                vehicle.SetMod(VehicleMod.Hood, 2, true);
            }

            // Disable radio
            Function.Call(Hash.SET_VEHICLE_RADIO_ENABLED, vehicle, false);

            // Debug -- add blip
            //Blip blip = vehicle.AddBlip();
            //blip.Color = BlipColor.White;

            return vehicle;
        }

        /// <summary>
        /// Change the player character's model
        /// </summary>
        public static void SetPlayerModel(PedHash pedHash)
        {
            var characterModel = new Model(pedHash);
            characterModel.Request(500);

            if (characterModel.IsInCdImage && characterModel.IsValid)
            {
                while (!characterModel.IsLoaded) Script.Wait(100);

                Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, characterModel.Hash);
                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player);
            }

            characterModel.MarkAsNoLongerNeeded();
        }
    }
}
