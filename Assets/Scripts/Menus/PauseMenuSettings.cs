



public class PauseMenuSettings : BaseMenuSettings
{
    protected override void OnBackButtonClicked()
    {
        base.OnBackButtonClicked();
        GameManager.One.UnloadPauseMenuSettings();
    }
}