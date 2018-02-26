namespace Avelango.Models.Abstractions.Db
{
    public interface IMails
    {
        void SaveEmailInfo(string recipient, string subject);
    }
}
