using SHCDESE.API.Components.Network;
using SHCDESE.ViewModels;

namespace LorrdyAISharesGold.UI;

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
    public bool ShowMessage
    {
        get => _showMessage;
        set
        {
            _showMessage = value;
            OnPropertyChanged(nameof(ShowMessage));
        }
    }
    private bool _showMessage = false;


    [SyncHostOnly]
    public int MinGoldToShare
    {
        get => _minGoldToShare;
        set
        {
            _minGoldToShare = value;
            OnPropertyChanged(nameof(MinGoldToShare));
        }
    }
    private int _minGoldToShare = 20000;


    [SyncHostOnly]
    public int MaxGoldToGet
    {
        get => _maxGoldToGet;
        set
        {
            _maxGoldToGet = value;
            OnPropertyChanged(nameof(MaxGoldToGet));
        }
    }
    private int _maxGoldToGet = 1000;


    [SyncHostOnly]
    public int GoldAmountToShare
    {
        get => _goldAmountToShare;
        set
        {
            _goldAmountToShare = value;
            OnPropertyChanged(nameof(GoldAmountToShare));
        }
    }
    private int _goldAmountToShare = 2000;
}