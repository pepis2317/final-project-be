using Entities;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace final_project_backend
{
    public class DatabaseXmlRepository : IXmlRepository
    {
        private readonly FinalProjectTrainingDbContext _context;
        public DatabaseXmlRepository(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return _context.DataProtectionKeys
                .AsNoTracking()
                .Select(k => XElement.Parse(k.XmlData))
                .ToList()
                .AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var xmlString = element.ToString();
            _context.DataProtectionKeys.Add(new DataProtectionKey { XmlData = xmlString });
            _context.SaveChanges();
        }
    }
}
