using I2.Loc;
using KSP.Game;
using KSP.Game.Science;
using KSP.Sim.Definitions;

namespace WMCCModules.Modules;

public class Module_ControlRange : PartBehaviourModule
{
    public override Type PartComponentModuleType => typeof(PartComponentModule_ControlRange);
    private Data_ControlRange _dataControlRange;
    public override void AddDataModules()
    {
        base.AddDataModules();
        _dataControlRange ??= new Data_ControlRange();
        DataModules.TryAddUnique(_dataControlRange, out _dataControlRange);
    }

    public override void OnInitialize()
    {
        if (PartBackingMode == PartBackingModes.Flight)
        {
            moduleIsEnabled = true;
            _dataControlRange.SetVisible(_dataControlRange.IsControllable, true);
        }
        else
        {
            _dataControlRange.SetVisible(_dataControlRange.IsControllable, false);
            foreach (var definition in _dataControlRange.ControlRangeDefinitions.Where(ComputeControllability))
            {
                foreach (var (key, value) in definition.AllowedScienceSituations)
                {
                    if (_dataControlRange.UnlockedScienceSituations.TryGetValue(key, out var hs))
                    {
                        hs.UnionWith(value);
                    }
                    else
                    {
                        _dataControlRange.UnlockedScienceSituations.Add(key, new HashSet<ScienceSitutation>(value));
                    }

                    _dataControlRange.CurrentLocalizationKey = definition.LocalizationKey;
                }
            }
        }

        var str = new LocalizedString(_dataControlRange.CurrentLocalizationKey).ToString() ??
                  _dataControlRange.CurrentLocalizationKey;
        if (str == "") str = _dataControlRange.CurrentLocalizationKey;
        _dataControlRange.ControlRange.SetValue(str);
    }

    private bool ComputeControllability(Data_ControlRange.ControlRangeDefinition definition)
    {
        if (!GameManager.Instance.GameModeManager.IsGameModeFeatureEnabled("SciencePoints")) return true;
        var scienceManager = GameManager.Instance.Game.ScienceManager;
        return definition.RequiredTechs.Count == 0 || definition.RequiredTechs.All(tech => scienceManager.IsNodeUnlocked(tech));
    }
}