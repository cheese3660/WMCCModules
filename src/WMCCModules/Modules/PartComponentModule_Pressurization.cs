using KSP.Game;
using KSP.Sim.impl;

namespace WMCCModules.Modules;

public class PartComponentModule_Pressurization : PartComponentModule
{
    public override Type PartBehaviourModuleType => typeof(Module_Pressurization);
    private KerbalRosterManager _rosterManager;
    private NotificationManager _notificationManager;
    private List<KerbalInfo> _kerbalsInSimObject;
    private Data_Pressurization _dataPressurization;
    private bool _alreadyWarned = false;
    private bool _alreadyKilled = false;
    public override void OnStart(double universalTime)
    {
        if (!DataModules.TryGetByType(out _dataPressurization))
        {
            WMCCModulesPlugin.Instance.SWLogger.LogError(
                $"Unable to find a Data_Pressurization in the PartComponentModule_Pressurization for {Part.PartName}");
            return;
        }

        if (GameManager.Instance.Game == null)
        {
            WMCCModulesPlugin.Instance.SWLogger.LogError(
                $"PartComponentModule_Pressurization being initialized without a game instance set up");
            return;
        }
        _rosterManager = GameManager.Instance.Game.SessionManager.KerbalRosterManager;
        _notificationManager = GameManager.Instance.Game.Notifications;
    }

    public override void OnUpdate(double universalTime, double deltaUniversalTime)
    {
        if (_dataPressurization.IsPressurized) return;
        _kerbalsInSimObject = _rosterManager.GetAllKerbalsInSimObject(Part.SimulationObject.GlobalId);
        var altitude = Part.PartOwner.SimulationObject.Vessel.AltitudeFromSeaLevel;
        if (altitude > _dataPressurization.WarningAltitude)
        {
            if (!_alreadyWarned) {
                _alreadyWarned = true;
                WarnForAllKerbals(universalTime);
            }
        }
        else
        {
            _alreadyWarned = false;
        }

        if (altitude > _dataPressurization.KillAltitude && !_alreadyKilled)
        {
            KillAllKerbals(universalTime);
        }
    }

    private void WarnForAllKerbals(double universalTime)
    {
        foreach (var kerbal in _kerbalsInSimObject)
        {
            _notificationManager.ProcessNotification(new NotificationData
            {
                Tier = NotificationTier.Alert,
                Importance = NotificationImportance.High,
                AlertTitle = new NotificationLineItemData
                {
                    ObjectParams = new object[]{Part.PartOwner.SimulationObject.Vessel.Name},
                    LocKey = "WMCCModules/Pressurization/Suffocating"
                },
                TimeStamp = universalTime,
                FirstLine = new NotificationLineItemData
                {
                    LocKey = kerbal.NameKey
                }
            });
        }

        _alreadyKilled = true;
    }
    private void KillAllKerbals(double universalTime)
    {
        foreach (var kerbal in _kerbalsInSimObject)
        {
            _notificationManager.ProcessNotification(new NotificationData
            {
                Tier = NotificationTier.Alert,
                Importance = NotificationImportance.High,
                AlertTitle = new NotificationLineItemData
                {
                    ObjectParams = new object[]{Part.PartOwner.SimulationObject.Vessel.Name},
                    LocKey = "WMCCModules/Pressurization/Killed"
                },
                TimeStamp = universalTime,
                FirstLine = new NotificationLineItemData
                {
                    LocKey = kerbal.NameKey
                }
            });
            _rosterManager.DestroyKerbal(kerbal.Id);
        }
    }
}