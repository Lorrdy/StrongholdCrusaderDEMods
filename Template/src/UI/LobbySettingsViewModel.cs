using SHCDESE.API;
using SHCDESE.API.Components.Network;
using SHCDESE.ViewModels;
using System;
using System.ComponentModel;

namespace __GUID__.UI;

public class LobbySettingsViewModel : LobbyModSettingsBaseViewModel
{
    // -------------------------------------------------------------------------
    // Host-only settings: only the host changes them; clients receive updates.
    // -------------------------------------------------------------------------

    [SyncHostOnly]
    public bool ModEnabled
    {
        get => _modEnabled;
        set
        {
            _modEnabled = value;
            OnPropertyChanged(nameof(ModEnabled));
        }
    }
    private bool _modEnabled = true;

    [SyncHostOnly]
    public int NumberToPick
    {
        get => _numberToPick;
        set
        {
            _numberToPick = value;
            OnPropertyChanged(nameof(NumberToPick));
        }
    }
    private int _numberToPick = 0;
}