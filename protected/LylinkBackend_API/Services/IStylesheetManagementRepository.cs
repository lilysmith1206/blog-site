using LylinkBackend_API.Models;
using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Services
{
    public interface IStylesheetManagementRepository
    {
        public IEnumerable<PageLink> GetAllStylesheets();

        public IEnumerable<CssRule> RetrieveStylesheetData(string stylesheetName);

        public bool UpdateStylesheet(string stylesheetName, IEnumerable<CssRule> rules);
    }
}
