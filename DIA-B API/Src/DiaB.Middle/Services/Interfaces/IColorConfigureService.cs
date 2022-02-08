namespace DiaB.Middle.Services.Interfaces
{
    public interface IColorConfigureService : ICommonConfigureService
    {
        string GetColorCode(string colorName);
    }
}
