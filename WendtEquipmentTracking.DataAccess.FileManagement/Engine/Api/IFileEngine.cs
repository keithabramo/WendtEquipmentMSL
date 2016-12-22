using System.Collections.Generic;
using Team6.DataAccess.FileManagement.Domain;

namespace Team6.DataAccess.FileManagement.Api
{
    public interface IFileEngine
    {
        byte[] GetFile(MergeDocFile mergeDocFile);
        void SaveFile(MergeDocFile mergeDocFile);
        IEnumerable<string> GetBookmarks(MergeDocFile mergeDocFile);

        byte[] GetPopulatedFile(MergeDocFile mergeDocFile, IDictionary<string, string> mappings);
    }
}