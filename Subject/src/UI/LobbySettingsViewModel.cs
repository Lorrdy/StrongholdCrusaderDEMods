using SHCDESE.API;
using SHCDESE.API.Components.Network;
using SHCDESE.ViewModels;
using System;
using System.ComponentModel;

namespace LorrdySubject.UI;

public class LobbySettingsViewModel : LobbyModSettingsBaseViewModel
{
    // -------------------------------------------------------------------------
    // Host-only settings: only the host changes them; clients receive updates.
    // -------------------------------------------------------------------------

    [SyncHostOnly]
    public int GoldForSubjugating
    {
        get => _goldForSubjugating;
        set
        {
            _goldForSubjugating = value;
            OnPropertyChanged(nameof(GoldForSubjugating));
        }
    }
    private int _goldForSubjugating = 500;


    [SyncHostOnly]
    public int GoldForGettingSubjugated
    {
        get => _goldForGettingSubjugated;
        set
        {
            _goldForGettingSubjugated = value;
            OnPropertyChanged(nameof(GoldForGettingSubjugated));
        }
    }
    private int _goldForGettingSubjugated = 2000;


    [SyncHostOnly]
    public int WoodForGettingSubjugated
    {
        get => _woodForGettingSubjugated;
        set
        {
            _woodForGettingSubjugated = value;
            OnPropertyChanged(nameof(WoodForGettingSubjugated));
        }
    }
    private int _woodForGettingSubjugated = 96;


    [SyncHostOnly]
    public int StoneForGettingSubjugated
    {
        get => _stoneForGettingSubjugated;
        set
        {
            _stoneForGettingSubjugated = value;
            OnPropertyChanged(nameof(StoneForGettingSubjugated));
        }
    }
    private int _stoneForGettingSubjugated = 48;


    [SyncHostOnly]
    public int FoodForGettingSubjugated
    {
        get => _foodForGettingSubjugated;
        set
        {
            _foodForGettingSubjugated = value;
            OnPropertyChanged(nameof(FoodForGettingSubjugated));
        }
    }
    private int _foodForGettingSubjugated = 60;
}