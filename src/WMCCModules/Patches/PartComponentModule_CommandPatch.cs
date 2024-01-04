using HarmonyLib;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using WMCCModules.Modules;

namespace WMCCModules.Patches;


[HarmonyPatch(typeof(PartComponentModule_Command))]
public class PartComponentModule_CommandPatch
{
    [HarmonyPatch(nameof(PartComponentModule_Command.UpdateControlStatus))]
    [HarmonyPostfix]
    public static void UpdateControlStatus(PartComponentModule_Command __instance)
    {
        if (!WMCCModulesPlugin.Instance.EnableControlRange.Value) return;
        if (!__instance.Part.Modules.TryGetValue(typeof(PartComponentModule_ControlRange), out var comp)) return;
        var mcr = comp as PartComponentModule_ControlRange;
        if (mcr!.DataControlRange == null || mcr.DataControlRange.Controllable) return;
        if (__instance.dataCommand.controlStatus.GetValue() == CommandControlState.FullyFunctional)
            __instance.dataCommand.controlStatus.SetValue(CommandControlState.Hibernating);
    }
}