using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EquinoxsModUtils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using ToolBuddy.ThirdParty.VectorGraphics;
using UnityEngine;

namespace EarlyBaseBuilding
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class EarlyBaseBuildingPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.equinox.EarlyBaseBuilding";
        private const string PluginName = "EarlyBaseBuilding";
        private const string VersionString = "2.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            EMU.Events.GameDefinesLoaded += OnGameDefinesLoaded;
            EMU.Events.GameLoaded += OnGameLoaded;

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void OnGameDefinesLoaded() {
            List<string> calyciteParts = new List<string>() {
                EMU.Names.Resources.CalycitePlatform1x1,
                EMU.Names.Resources.CalycitePlatform3x3,
                EMU.Names.Resources.CalycitePlatform5x5,
                EMU.Names.Resources.Catwalk3x9,
                EMU.Names.Resources.Catwalk5x9,
                EMU.Names.Resources.CalyciteWall3x3,
                EMU.Names.Resources.CalyciteWall5x3,
                EMU.Names.Resources.CalyciteWall5x5,
                EMU.Names.Resources.CalyciteGate5x2,
                EMU.Names.Resources.CalyciteGate5x5,
                EMU.Names.Resources.CalyciteGate10x5,
                EMU.Names.Resources.CalyciteWallCap3x1,
                EMU.Names.Resources.CalyciteWallCap5x1,
                EMU.Names.Resources.CalyciteWallCorner1x1,
                EMU.Names.Resources.CalyciteVerticalWallCap3x1,
                EMU.Names.Resources.CalyciteVerticalWallCap5x1,
                EMU.Names.Resources.CalyciteVerticalWallCorner1x1,
                EMU.Names.Resources.CalyciteWallCutaway2x2,
                EMU.Names.Resources.CalyciteWallCutaway3x3,
                EMU.Names.Resources.CalyciteWallCutaway5x3,
                EMU.Names.Resources.CalyciteWallCutaway5x5,
                EMU.Names.Resources.CalycitePillar1x1,
                EMU.Names.Resources.CalycitePillar1x3,
                EMU.Names.Resources.CalycitePillar1x5,
                EMU.Names.Resources.CalyciteAngleSupport3x3,
                EMU.Names.Resources.CalyciteBeam1x1,
                EMU.Names.Resources.CalyciteBeam3x1,
                EMU.Names.Resources.CalyciteBeam5x1,
                EMU.Names.Resources.CalyciteRamp1x1,
                EMU.Names.Resources.CalyciteRamp1x3,
                EMU.Names.Resources.CalyciteRamp1x5,
            };
            List<string> metalParts = new List<string>() {
                EMU.Names.Resources.MetalStairs1x1,
                EMU.Names.Resources.MetalStairs3x1,
                EMU.Names.Resources.MetalStairs3x3,
                EMU.Names.Resources.MetalStairs3x5,
                EMU.Names.Resources.Railing1x1,
                EMU.Names.Resources.Railing3x1,
                EMU.Names.Resources.Railing5x1,
                EMU.Names.Resources.RailingCorner1x1,
                EMU.Names.Resources.RailingCorner3x3,
                EMU.Names.Resources.MetalPillar1x1,
                EMU.Names.Resources.MetalPillar1x3,
                EMU.Names.Resources.MetalPillar1x5,
                EMU.Names.Resources.MetalAngleSupport5x2,
                EMU.Names.Resources.MetalRibBase1x2,
                EMU.Names.Resources.MetalRibMiddle1x3,
                EMU.Names.Resources.MetalBeam1x1,
                EMU.Names.Resources.MetalBeam1x3,
                EMU.Names.Resources.MetalBeam1x5,
            };
            List<string> flowers = new List<string>() {
                EMU.Names.Resources.SmallFloorPot,
                EMU.Names.Resources.WallPot,
                EMU.Names.Resources.MediumFloorPot,
                EMU.Names.Resources.CeilingPlant1x1,
                EMU.Names.Resources.CeilingPlant3x3,
                EMU.Names.Resources.WallPlant1x1,
                EMU.Names.Resources.WallPlant3x3,
            };
            List<string> lights = new List<string>() {
                EMU.Names.Resources.LightStick,
                EMU.Names.Resources.RedLightStick,
                EMU.Names.Resources.GreenLightStick,
                EMU.Names.Resources.BlueLightStick,
                EMU.Names.Resources.OverheadLight,
                EMU.Names.Resources.StandingLamp1x5,
                EMU.Names.Resources.WallLight1x1,
                EMU.Names.Resources.WallLight3x1,
                EMU.Names.Resources.HangingLamp1x1,
                EMU.Names.Resources.FanLamp7x2,
                EMU.Names.Resources.DiscoBlock1x1,
                EMU.Names.Resources.GlowBlock1x1
            };

            List<ResourceInfo> calyciteIngredients = new List<ResourceInfo>() { EMU.Resources.GetResourceInfoByName(EMU.Names.Resources.Limestone) };
            List<ResourceInfo> metalIngredients = new List<ResourceInfo>() { EMU.Resources.GetResourceInfoByName(EMU.Names.Resources.IronIngot) };
            List<ResourceInfo> flowersIngredients = new List<ResourceInfo>() { EMU.Resources.GetResourceInfoByName(EMU.Names.Resources.Plantmatter) };
            List<ResourceInfo> lightsIngredients = new List<ResourceInfo>() {
                EMU.Resources.GetResourceInfoByName(EMU.Names.Resources.IronIngot),
                EMU.Resources.GetResourceInfoByName(EMU.Names.Resources.CopperWire)
            };

            foreach(string name in calyciteParts) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(EMU.Resources.GetResourceInfoByName(name));
                recipe.ingTypes = calyciteIngredients.ToArray();
                recipe.ingQuantities = new int[] { 5 };
            }

            foreach(string name in metalParts) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(EMU.Resources.GetResourceInfoByName(name));
                recipe.ingTypes = metalIngredients.ToArray();
                recipe.ingQuantities = new int[] { 2 };
            }

            foreach(string name in flowers) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(EMU.Resources.GetResourceInfoByName(name));
                recipe.ingTypes = flowersIngredients.ToArray();
                recipe.ingQuantities = new int[] { 5 };
            }

            foreach(string name in lights) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(EMU.Resources.GetResourceInfoByName(name));
                recipe.ingTypes = lightsIngredients.ToArray();
                recipe.ingQuantities = new int[] { 1, 1 };
            }
        }

        private void OnGameLoaded() {
            List<string> unlockNames = new List<string>() {
                EMU.Names.Unlocks.StairsAndWalkwaysI,
                EMU.Names.Unlocks.StairsAndWalkwaysII,
                EMU.Names.Unlocks.StairsAndWalkwaysIII,
                EMU.Names.Unlocks.LightStickPrimaryColors,
                EMU.Names.Unlocks.DECOSeriesI,
                EMU.Names.Unlocks.DECOSeriesII,
                EMU.Names.Unlocks.DECOSeriesIII,
                EMU.Names.Unlocks.DECOPlantsAndCeilings,
                EMU.Names.Unlocks.DECOPlantsAndWalls,
                EMU.Names.Unlocks.GatesI,
                EMU.Names.Unlocks.GatesII,
                EMU.Names.Unlocks.GatesIII,
                EMU.Names.Unlocks.GatesIV,
                EMU.Names.Unlocks.LightStick,
                EMU.Names.Unlocks.OverheadLight,
                EMU.Names.Unlocks.OverheadLightII,
                EMU.Names.Unlocks.PlatformsI,
                EMU.Names.Unlocks.Beams,
                EMU.Names.Unlocks.PlatformsII,
                EMU.Names.Unlocks.DECOStaticLights,
                EMU.Names.Unlocks.DECODynamicLights,
                EMU.Names.Unlocks.SupportsI,
                EMU.Names.Unlocks.SupportsII,
                EMU.Names.Unlocks.SupportsIII,
                EMU.Names.Unlocks.SupportsIV,
                EMU.Names.Unlocks.WallLights,
                EMU.Names.Unlocks.WallsI,
                EMU.Names.Unlocks.WallsII,
                EMU.Names.Unlocks.WallsIII
            };

            foreach (string name in unlockNames) {
                Unlock unlock = EMU.Unlocks.GetUnlockByName(name);
                TechTreeState.instance.UnlockTech(unlock.uniqueId, false, false);
            }
        }

        private SchematicsRecipeData GetRecipeForBuildable(ResourceInfo info) {
            return GameDefines.instance.schematicsRecipeEntries.Where(recipe => recipe.outputTypes.Contains(info)).First();
        }
    }
}
