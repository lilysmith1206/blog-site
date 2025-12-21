using Microsoft.EntityFrameworkCore;
namespace LylinkBackend_DatabaseAccessLayer.Models
{
    public partial class LylinkdbContext
    {
        public LylinkdbContext(DbContextOptions<LylinkdbContext> options)
            : base(options)
        {
        }
    }
}
