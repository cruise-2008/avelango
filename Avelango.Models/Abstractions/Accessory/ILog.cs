namespace Avelango.Models.Abstractions.Accessory
{
    public interface ILog
    {
        void AddInfo(string eventName, string data);
        void AddError(string eventName, string data);
        void AddFatal(string eventName, string data);
        void AddDebug(string eventName, string data);
    }
}
