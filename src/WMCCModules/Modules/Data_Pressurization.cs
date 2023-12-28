using JetBrains.Annotations;
using KSP.Sim;
using KSP.Sim.Definitions;

namespace WMCCModules.Modules;

public class Data_Pressurization : ModuleData
{
    public override Type ModuleType => typeof(Module_Pressurization);
    [KSPDefinition] public List<string> TechNodesForPressurization = new();
    [KSPDefinition] public bool UseScienceToChangePressurization = false;
    [KSPDefinition] public bool DefaultPressurizationState = false;
    [KSPDefinition] public double WarningAltitude = 0.0;
    [KSPDefinition] public double KillAltitude = 0.0;
    [KSPState] public bool IsPressurized = false;

    [LocalizedField("WMCCModules/Pressurized")] [PAMDisplayControl(SortIndex = 1)] [KSPState]
    public ModuleProperty<string> PressurizationState = new("", true);
    [LocalizedField("WMCCModules/ServiceCeiling")]
    [PAMDisplayControl(SortIndex = 2)]
    [KSPState] public ModuleProperty<string> ServiceCeiling = new("", true);
}