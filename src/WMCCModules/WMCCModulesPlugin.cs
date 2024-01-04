using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using SpaceWarp;
using SpaceWarp.API.Configuration;
using SpaceWarp.API.Mods;
using WMCCModules.Patches;

namespace WMCCModules;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class WMCCModulesPlugin : BaseSpaceWarpPlugin
{
    // Useful in case some other mod wants to use this mod a dependency
    [PublicAPI] public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    [PublicAPI] public const string ModName = MyPluginInfo.PLUGIN_NAME;
    [PublicAPI] public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

    // Singleton instance of the plugin class
    [PublicAPI] public static WMCCModulesPlugin Instance { get; set; }
    public ConfigValue<bool> EnableControlRange;
    private void Start()
    {
        EnableControlRange = new ConfigValue<bool>(SWConfiguration.Bind("Module Settings", "Enable Control Range", true,
            "Enable the control range module (what forces you to lose control in certain situations until you unlock a certain tech node)"));
    }

    /// <summary>
    /// Runs when the mod is first initialized.
    /// </summary>
    public override void OnInitialized()
    {
        Harmony.CreateAndPatchAll(typeof(PartComponentModule_CommandPatch));
        base.OnInitialized();

        Instance = this;
    }
}

