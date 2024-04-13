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
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            ModUtils.GameDefinesLoaded += OnGameDefinesLoaded;
            ModUtils.GameLoaded += OnGameLoaded;

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void OnGameDefinesLoaded(object sender, EventArgs e) {
            List<string> calyciteParts = new List<string>() {
                ResourceNames.CalycitePlatform1x1,
                ResourceNames.CalycitePlatform3x3,
                ResourceNames.CalycitePlatform5x5,
                ResourceNames.Catwalk3x9,
                ResourceNames.Catwalk5x9,
                ResourceNames.CalyciteWall3x3,
                ResourceNames.CalyciteWall5x3,
                ResourceNames.CalyciteWall5x5,
                ResourceNames.CalyciteGate5x2,
                ResourceNames.CalyciteGate5x5,
                ResourceNames.CalyciteGate10x5,
                ResourceNames.CalyciteWallCap3x1,
                ResourceNames.CalyciteWallCap5x1,
                ResourceNames.CalyciteWallCorner1x1,
                ResourceNames.CalyciteVerticalWallCap3x1,
                ResourceNames.CalyciteVerticalWallCap5x1,
                ResourceNames.CalyciteVerticalWallCorner1x1,
                ResourceNames.CalyciteWallCutaway2x2,
                ResourceNames.CalyciteWallCutaway3x3,
                ResourceNames.CalyciteWallCutaway5x3,
                ResourceNames.CalyciteWallCutaway5x5,
                ResourceNames.CalycitePillar1x1,
                ResourceNames.CalycitePillar1x3,
                ResourceNames.CalycitePillar1x5,
                ResourceNames.CalyciteAngleSupport3x3,
                ResourceNames.CalyciteBeam1x1,
                ResourceNames.CalyciteBeam3x1,
                ResourceNames.CalyciteBeam5x1,
            };
            List<string> metalParts = new List<string>() {
                ResourceNames.MetalStairs1x1,
                ResourceNames.MetalStairs3x1,
                ResourceNames.MetalStairs3x3,
                ResourceNames.MetalStairs3x5,
                ResourceNames.Railing1x1,
                ResourceNames.Railing3x1,
                ResourceNames.Railing5x1,
                ResourceNames.RailingCorner1x1,
                ResourceNames.RailingCorner3x3,
                ResourceNames.MetalPillar1x1,
                ResourceNames.MetalPillar1x3,
                ResourceNames.MetalPillar1x5,
                ResourceNames.MetalAngleSupport5x2,
                ResourceNames.MetalRibBase1x2,
                ResourceNames.MetalRibMiddle1x3,
                ResourceNames.MetalBeam1x1,
                ResourceNames.MetalBeam1x3,
                ResourceNames.MetalBeam1x5,
            };
            List<string> flowers = new List<string>() {
                ResourceNames.SmallFloorPot,
                ResourceNames.WallPot,
                ResourceNames.MediumFloorPot,
                ResourceNames.CeilingPlant1x1,
                ResourceNames.CeilingPlant3x3,
                ResourceNames.WallPlant1x1,
                ResourceNames.WallPlant3x3,
            };
            List<string> lights = new List<string>() {
                ResourceNames.LightStick,
                ResourceNames.RedLightStick,
                ResourceNames.GreenLightStick,
                ResourceNames.BlueLightStick,
                ResourceNames.OverheadLight,
                ResourceNames.StandingLamp1x5,
                ResourceNames.WallLight1x1,
                ResourceNames.WallLight3x1,
                ResourceNames.HangingLamp1x1,
                ResourceNames.FanLamp7x2,
            };

            List<ResourceInfo> calyciteIngredients = new List<ResourceInfo>() { ModUtils.GetResourceInfoByName(ResourceNames.Limestone) };
            List<ResourceInfo> metalIngredients = new List<ResourceInfo>() { ModUtils.GetResourceInfoByName(ResourceNames.IronIngot) };
            List<ResourceInfo> flowersIngredients = new List<ResourceInfo>() { ModUtils.GetResourceInfoByName(ResourceNames.Plantmatter) };
            List<ResourceInfo> lightsIngredients = new List<ResourceInfo>() {
                ModUtils.GetResourceInfoByName(ResourceNames.IronIngot),
                ModUtils.GetResourceInfoByName(ResourceNames.CopperWire)
            };

            foreach(string name in calyciteParts) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(ModUtils.GetResourceInfoByName(name));
                ModUtils.NullCheck(recipe, name + " Recipe");
                recipe.ingTypes = calyciteIngredients.ToArray();
                recipe.ingQuantities = new int[] { 5 };
            }

            foreach(string name in metalParts) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(ModUtils.GetResourceInfoByName(name));
                ModUtils.NullCheck(recipe, name + " Recipe");
                recipe.ingTypes = metalIngredients.ToArray();
                recipe.ingQuantities = new int[] { 2 };
            }

            foreach(string name in flowers) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(ModUtils.GetResourceInfoByName(name));
                ModUtils.NullCheck(recipe, name + " Recipe");
                recipe.ingTypes = flowersIngredients.ToArray();
                recipe.ingQuantities = new int[] { 5 };
            }

            foreach(string name in lights) {
                SchematicsRecipeData recipe = GetRecipeForBuildable(ModUtils.GetResourceInfoByName(name));
                ModUtils.NullCheck(recipe, name + " Recipe");
                recipe.ingTypes = lightsIngredients.ToArray();
                recipe.ingQuantities = new int[] { 1, 1 };
            }
        }

        private void OnGameLoaded(object sender, EventArgs e) {
            List<string> unlockNames = new List<string>() {
                UnlockNames.StairsAndWalkwaysI,
                UnlockNames.StairsAndWalkwaysII,
                UnlockNames.StairsAndWalkwaysIII,
                UnlockNames.LightStickPrimaryColors,
                UnlockNames.DECOSeriesI,
                UnlockNames.DECOSeriesII,
                UnlockNames.DECOSeriesIII,
                UnlockNames.DECOPlantsAndCeilings,
                UnlockNames.DECOPlantsAndWalls,
                UnlockNames.GatesI,
                UnlockNames.GatesII,
                UnlockNames.GatesIII,
                UnlockNames.GatesIV,
                UnlockNames.LightStick,
                UnlockNames.OverheadLight,
                UnlockNames.OverheadLightII,
                UnlockNames.PlatformsI,
                UnlockNames.BeamsI,
                UnlockNames.PlatformsII,
                UnlockNames.BeamsII,
                UnlockNames.BasicConstruction,
                UnlockNames.SupportsI,
                UnlockNames.SupportsII,
                UnlockNames.SupportsIII,
                UnlockNames.SupportsIV,
                UnlockNames.WallLights,
                UnlockNames.WallsI,
                UnlockNames.WallsII,
                UnlockNames.WallsIII
            };
            foreach (string name in unlockNames) {
                Unlock unlock = ModUtils.GetUnlockByName(name);
                UnlockTechAction action = new UnlockTechAction {
                    info = new UnlockTechInfo {
                        unlockID = unlock.uniqueId,
                        drawPower = false
                    }
                };
                NetworkMessageRelay.instance.SendNetworkAction(action);
            }
        }

        private SchematicsRecipeData GetRecipeForBuildable(ResourceInfo info) {
            return GameDefines.instance.schematicsRecipeEntries.Where(recipe => recipe.outputTypes.Contains(info)).First();
        }
    }
}
