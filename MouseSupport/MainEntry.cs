using UnityModManagerNet;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport
{
    public class MainEntry
    {
        public static UnityModManager.ModEntry Mod { get; private set; }

        private static Harmony harmonyInstance = null;
        private static WindowState windowState = null;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;
            harmonyInstance = new Harmony("de.maxvollmer.phoenotopia.mousesupport");
            modEntry.OnUpdate += OnUpdate;
            modEntry.OnFixedGUI += OnFixedGUI;
            modEntry.OnToggle += OnToggle;

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                modEntry.Logger.Log("Patching all the things");
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                Game.MouseControls.ParseMouseControls();
                windowState = Camera.main.gameObject.AddComponent<WindowState>();
            }
            else
            {
                modEntry.Logger.Log("Unpatching all the things");
                harmonyInstance.UnpatchAll("de.maxvollmer.phoenotopia.mousesupport");
                UnityEngine.Object.Destroy(windowState);
                windowState = null;
            }

            return true;
        }



        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            /*
            modEntry.Logger.Log("OnUpdate");

            modEntry.Logger.Log("PT2.game_paused: " + PT2.game_paused);
            modEntry.Logger.Log("PT2.menu.is_active: " + PT2.menu.is_active);
            modEntry.Logger.Log("PT2.tv_hud._pause_screen.activeSelf: " + PT2.tv_hud.transform.Find("PauseScreen").gameObject.activeSelf);

            foreach (var openingMenuLogic in Resources.FindObjectsOfTypeAll<OpeningMenuLogic>())
            {
                modEntry.Logger.Log("OpeningMenuLogic: " + openingMenuLogic);
            }

            foreach (var directorLogic in Resources.FindObjectsOfTypeAll<DirectorLogic>())
            {
                modEntry.Logger.Log("DirectorLogic: " + directorLogic);
                modEntry.Logger.Log("is_directing: " + directorLogic.is_directing);
                modEntry.Logger.Log("_is_SELECT_PAUSE: " + directorLogic._is_SELECT_PAUSE);
                modEntry.Logger.Log("DialoguersAreInOperation: " + directorLogic.DialoguersAreInOperation());
                //modEntry.Logger.Log("_in_ACTIONS_mode: " + directorLogic._in_ACTIONS_mode);
            }
            */
        }

        private static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            // save_file.HowMuchCanBeAdded
            // save_file.AddItemToolOrStatusIdToInventory
        }
    }
}
