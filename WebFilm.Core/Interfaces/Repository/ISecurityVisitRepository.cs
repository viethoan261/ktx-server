using WebFilm.Core.Enitites.Security;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface ISecurityVisitRepository : IBaseRepository<int, SecurityVisit>
    {
        IEnumerable<SecurityVisit> GetAllVisits();
        
        IEnumerable<SecurityVisit> GetActiveVisits();
        
        IEnumerable<SecurityVisit> GetVisitsByDate(DateTime date);
        
        IEnumerable<SecurityVisit> GetVisitsByStudentId(int studentId);
        
        int CheckOut(int id, DateTime exitTime);
        
        int CreateVisit(SecurityVisit visit);
        
        SecurityVisit GetVisitById(int id);
        
        int DeleteVisit(int id);
    }
} 