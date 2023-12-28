using KSP.Game.Science;
using KSP.Sim.impl;

namespace WMCCModules.Modules;

public class PartComponentModule_ControlRange : PartComponentModule
{
    public override Type PartBehaviourModuleType => typeof(Module_ControlRange);
    private ResearchLocation _lastResearchLocation = null;
    public Data_ControlRange DataControlRange;
    private PartComponentModule_Command _componentModuleCommand;
    public override void OnStart(double universalTime)
    {
        if (!DataModules.TryGetByType(out DataControlRange))
        {
            WMCCModulesPlugin.Instance.SWLogger.LogError(
                $"Unable to find a Data_ControlRange in the PartComponentModule_ControlRange for {Part.PartName}");
            return;
        }

        if (!Part.TryGetModule(out _componentModuleCommand))
        {
            WMCCModulesPlugin.Instance.SWLogger.LogError(
                $"Unable to find a PartComponentModule_Command for the PartComponentModule_ControlRange on {Part.PartName}");
        }
    }

    public override void OnUpdate(double universalTime, double deltaUniversalTime)
    {
        var vessel = Part.PartOwner.SimulationObject.Vessel;
        if (vessel.VesselScienceRegionSituation.ResearchLocation.Equals(_lastResearchLocation)) return;
        _lastResearchLocation = vessel.VesselScienceRegionSituation.ResearchLocation;
        var isControllable = false;

        if (DataControlRange.UnlockedScienceSituations.TryGetValue(_lastResearchLocation.BodyName, out var hs))
        {
            isControllable = hs.Contains(_lastResearchLocation.ScienceSituation);
        }

        DataControlRange.Controllable = isControllable;
        DataControlRange.IsControllable.SetValue(isControllable ? "Yes" : "No");
        _componentModuleCommand.UpdateControlStatus();
    }
}