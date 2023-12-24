using System.Globalization;
using KSP;
using KSP.Game;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using UnityEngine;

namespace WMCCModules.Modules;

public class Module_Pressurization : PartBehaviourModule
{
    [SerializeField] protected Data_Pressurization _dataPressurization;
    public override Type PartComponentModuleType => typeof(PartComponentModule_Pressurization);

    protected override void AddDataModules()
    {
        base.AddDataModules();
        _dataPressurization ??= new Data_Pressurization();
        DataModules.TryAddUnique(_dataPressurization, out _dataPressurization);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        if (PartBackingMode == PartBackingModes.Flight)
        {
            moduleIsEnabled = true;
        }
        _dataPressurization.IsPressurized =
            _dataPressurization.DefaultPressurizationState || GetIsPressurizationUnlocked();
        if (_dataPressurization.IsPressurized)
        {
            _dataPressurization.PressurizationState.SetValue("Yes");
            _dataPressurization.ServiceCeiling.SetValue("Infinite");
        }
        else
        {
            _dataPressurization.PressurizationState.SetValue("No");
            _dataPressurization.ServiceCeiling.SetValue(
                $"{_dataPressurization.KillAltitude.ToString(CultureInfo.InvariantCulture)} {Units.TextMeters}");
        }
    }

    private bool GetIsPressurizationUnlocked()
    {
        if (!_dataPressurization.UseScienceToChangePressurization) return false;
        if (!GameManager.Instance.GameModeManager.IsGameModeFeatureEnabled("SciencePoints")) return true;
        var scienceManager = GameManager.Instance.Game.ScienceManager;
        return _dataPressurization.TechNodesForPressurization.All(tech => scienceManager.IsNodeUnlocked(tech));
    }
}