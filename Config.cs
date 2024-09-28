using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;

namespace FastForward
{
    public class ModConfig
    {
        public SButton button1 = (SButton)192;
        public SButton button2 = (SButton)186;
        public int speed1 { get; set; } = 7;
        public int speed2 { get; set; } = 2;
    }

    class ConfigMenu
    {
        private readonly IModHelper Helper;
        private readonly IManifest ModManifest;

        private ModConfig Config
        {
            get => ModEntry.Config;
            set => ModEntry.Config = value;
        }

        public ConfigMenu(IMod mod)
        {
            Helper = mod.Helper;
            ModManifest = mod.ModManifest;
        }

        public void RegisterMenu()
        {
            var GMCM = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (GMCM is not null)
            {
                GMCM.Register(ModManifest, () => Config = new ModConfig(), () => Helper.WriteConfig(Config));

                GMCM.AddKeybind(ModManifest, () => Config.button1, (SButton val) => Config.button1 = val, () => "Button 1", () => "Button that toggles Speed 1");
                GMCM.AddNumberOption(ModManifest, () => Config.speed1, (int val) => Config.speed1 = val, () => "Speed 1", () => "Speed multiplier toggled by Button 1", min: 2);
                GMCM.AddKeybind(ModManifest, () => Config.button2, (SButton val) => Config.button2 = val, () => "Button 2", () => "Button that toggles Speed 2");
                GMCM.AddNumberOption(ModManifest, () => Config.speed2, (int val) => Config.speed2 = val, () => "Speed 2", () => "Speed multiplier toggled by Button 2", min: 2);
            }
        }
    }
}