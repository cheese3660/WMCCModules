using KSP.Game.Science;
using KSP.Sim;
using KSP.Sim.Definitions;

namespace WMCCModules.Modules;

public class Data_ControlRange : ModuleData
{
    public override Type ModuleType => typeof(Module_ControlRange);

    
    public class ControlRangeDefinition
    {
        public List<string> RequiredTechs;
        public Dictionary<string, HashSet<ScienceSitutation>> AllowedScienceSituations;
        public string LocalizationKey;
    }

    [KSPDefinition]
    public List<ControlRangeDefinition> ControlRangeDefinitions;

    [KSPState] public bool Controllable;
    [KSPState] public Dictionary<string, HashSet<ScienceSitutation>> UnlockedScienceSituations;
    [KSPState] public string CurrentLocalizationKey;

    [LocalizedField("WMCCModules/ControlRange")] [PAMDisplayControl(SortIndex = 1)]
    public ModuleProperty<string> ControlRange = new("", true);
    [LocalizedField("WMCCModules/IsControllable")] [PAMDisplayControl(SortIndex = 1)]
    public ModuleProperty<string> IsControllable = new("", true);
}