



public class MainMenuSettings : BaseMenuSettings
{
    protected override void OnBackButtonClicked()
    {
        base.OnBackButtonClicked();
        GameManager.One.LoadMainMenuFromOtherMenu();
    }
}