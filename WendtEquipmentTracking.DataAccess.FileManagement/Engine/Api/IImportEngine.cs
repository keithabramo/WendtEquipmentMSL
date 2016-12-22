using System.Collections.Generic;
using Team6.DataAccess.FileManagement.Domain;

namespace Team6.DataAccess.FileManagement.Api
{
    public interface IImportEngine
    {
        IEnumerable<SiteRow> GetSites(byte[] importFile);

    }
}