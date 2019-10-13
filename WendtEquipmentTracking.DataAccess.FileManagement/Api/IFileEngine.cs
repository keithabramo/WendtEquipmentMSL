namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IFileEngine
    {
        void SaveFile(string filePath, byte[] file);
        void RemoveFile(string filePath);

    }
}