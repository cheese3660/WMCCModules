using HarmonyLib;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using WMCCModules.Modules;

namespace WMCCModules.Patches;


[HarmonyPatch(typeof(PartComponentModule_Command))]
public class PartComponentModule_CommandPatch
{
    [HarmonyPatch(nameof(PartComponentModule_Command.UpdateControlStatus))]
    [HarmonyPrefix]
    public static bool UpdateControlStatus(PartComponentModule_Command __instance)
    {
        if (!__instance.Part.Modules.TryGetValue(typeof(PartComponentModule_ControlRange), out var comp)) return true;
        var mcr = comp as PartComponentModule_ControlRange;
        if (mcr!.DataControlRange == null || mcr.DataControlRange.Controllable) return true; 
        __instance.dataCommand.controlStatus.SetValue(CommandControlState.NoCommNetConnection);
        return false;
    }
}