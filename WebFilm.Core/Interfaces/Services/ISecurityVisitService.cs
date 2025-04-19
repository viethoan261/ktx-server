using WebFilm.Core.Enitites.Security;

namespace WebFilm.Core.Interfaces.Services
{
    public interface ISecurityVisitService : IBaseService<int, SecurityVisit>
    {
        IEnumerable<SecurityVisitResponseDTO> GetAllVisits();
        
        IEnumerable<SecurityVisitResponseDTO> GetActiveVisits();
        
        IEnumerable<SecurityVisitResponseDTO> GetVisitsByDate(DateTime date);
        
        IEnumerable<SecurityVisitResponseDTO> GetVisitsByStudentId(int studentId);
        
        SecurityVisitResponseDTO CheckIn(SecurityVisitDTO visitDTO);
        
        SecurityVisitResponseDTO CheckOut(int id);
        
        SecurityVisitResponseDTO GetVisitById(int id);
        
        int DeleteVisit(int id);
    }
} 