namespace MainProject.MainGame.Source.Player.PlayerController.States
{
    public enum PlayerControllerState : byte
    {
        None = 0,
        Idle = 1,
        Movement = 2,
        Jump = 3,
        Fall = 4,
        Roll = 5,
        
        NullState = 255
    }
}