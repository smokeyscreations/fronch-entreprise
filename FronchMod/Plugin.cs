using BepInEx;
using BepInEx.Logging;
using FronchMod.Patches;
using HarmonyLib;
using LethalLib.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FronchMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class ModBase : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private static ModBase Instance;
        internal ManualLogSource mls;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_GUID);
            mls.LogInfo("Mod base awakened :");

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "itemmod");
            mls.LogInfo("Reached and grabbed from assetDir");

            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);
            mls.LogInfo("Bundle loaded from assetDir");

            Item baguette = bundle.LoadAsset<Item>("Assets/ItemMods/Baguette.asset");
            mls.LogInfo("Asset bundle loaded");

            Utilities.FixMixerGroups(baguette.spawnPrefab);
            mls.LogInfo("Fixed Mixer Groups");

            if(baguette != null)
            {
                Items.RegisterScrap(baguette, 1000, Levels.LevelTypes.All);
                NetworkPrefabs.RegisterNetworkPrefab(baguette.spawnPrefab);
            }
            

            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.clearPreviousText = true;
            node.displayText = "This is a fronch baguette";
            if (node != null)
            {
                mls.LogInfo("Created terminal node instance");
                Items.RegisterShopItem(baguette, null, null, node, 0);
                mls.LogInfo("Registered shop item");

                
            }
            
            

            harmony.PatchAll();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            
        }
    }
}