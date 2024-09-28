using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Characters;

namespace FastForward
{
    internal sealed class ModEntry : Mod
    {
        internal static IMonitor ModMonitor { get; set; }
        internal new static IModHelper Helper { get; set; }
        internal static ModConfig Config;
        private ConfigMenu ConfigMenu;

        public static int curr_speed = 0;
        public static bool locked;

        public override void Entry(IModHelper helper)
        {
            ModMonitor = Monitor;
            Helper = helper;
            ConfigMenu = new ConfigMenu(this);

            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;
            //OnUpdateTicking added/removed via button press
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Config = Helper.ReadConfig<ModConfig>();
            ConfigMenu.RegisterMenu();
        }

        private void OnSaveLoaded(object sender, EventArgs ex)
        {
            try
            {
                Config = Helper.ReadConfig<ModConfig>();
            }
            catch (Exception e)
            {
                ModMonitor.Log($"Fast Forward: Failed to load config settings. Will use default settings instead. Error details:\n{e}", LogLevel.Debug);
                Config = new ModConfig();
            }
        }

            private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == Config.button1)
            {
                if (curr_speed == 0)
                {
                    curr_speed = 1;
                    Helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
                    Game1.addHUDMessage(new HUDMessage($"Fast Forward X{Config.speed1} enabled", 2));
                }
                else
                {
                    curr_speed = 0;
                    Helper.Events.GameLoop.UpdateTicking -= OnUpdateTicking;
                    Game1.addHUDMessage(new HUDMessage("Fast Forward Disabled", 3));
                }
            }
            else if (e.Button == Config.button2)
            {
                if (curr_speed == 0)
                {
                    curr_speed = 2;
                    Helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
                    Game1.addHUDMessage(new HUDMessage($"Fast Forward X{Config.speed2} enabled", 2));
                }
                else
                {
                    curr_speed = 0;
                    Helper.Events.GameLoop.UpdateTicking -= OnUpdateTicking;
                    Game1.addHUDMessage(new HUDMessage("Fast Forward Disabled", 3));
                }
            }
        }

        private void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
        {
            if (locked)
                return;
            locked = true;
            for (int i = 0; i < (curr_speed == 1 ? Config.speed1 : Config.speed2); i++)
            {
                Helper.Reflection.GetMethod(Game1.game1, "Update").Invoke([Game1.currentGameTime]);
            }
            locked = false;
        }
    }
}