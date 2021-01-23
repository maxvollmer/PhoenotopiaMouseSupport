using UnityModManagerNet;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using MouseSupport.Helpers;
using Rewired;
using MouseSupport.Patches;
using MouseSupport.Settings;

namespace MouseSupport
{
    public class MainEntry
    {
        public static ModSettings Settings { get; set; } = new ModSettings();

        public static UnityModManager.ModEntry Mod { get; private set; }

        private static Harmony harmonyInstance = null;
        private static WindowState windowState = null;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;
            harmonyInstance = new Harmony("de.maxvollmer.phoenotopia.mousesupport");
            modEntry.OnUpdate += OnUpdate;
            modEntry.OnGUI += OnGUI;
            modEntry.OnToggle += OnToggle;
            modEntry.OnSaveGUI += OnSave;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Draw(modEntry);
        }

        private static void OnSave(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                modEntry.Logger.Log("Patching all the things");
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                Game.MouseControls.ParseMouseControls();
                windowState = Camera.main.gameObject.AddComponent<WindowState>();
                Settings = ModSettings.Load<ModSettings>(modEntry);
            }
            else
            {
                modEntry.Logger.Log("Unpatching all the things");
                harmonyInstance.UnpatchAll("de.maxvollmer.phoenotopia.mousesupport");
                Object.Destroy(windowState);
                windowState = null;
                Settings.Save(modEntry);
            }

            return true;
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!DirectorLogicPatch.IsInControlSetup)
            {
                // Disable game's mouse controls as they interfere with our menu logic. We handle mouse controls completely
                PT2.director.control.control_mapper.showMouse = false;
                if (ReInput.players.Players.Count > 0)
                    ReInput.players.Players[0].controllers.hasMouse = false;
            }
        }
    }
}
