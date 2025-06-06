namespace WaterlilyLabs.Services.Delegates
{
    public delegate void EntityChangedNotification(string entityType, string action, int? entityId = null);
}
