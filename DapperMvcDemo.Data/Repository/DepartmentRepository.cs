using DapperMvcDemo.Data.DataAccess;
using DapperMvcDemo.Data.Models.Domain;

namespace DapperMvcDemo.Data.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ISqlDataAccess _db;
        public DepartmentRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _db.GetData<Department, dynamic>("sp_get_departments", new { });
        }
    }
}
