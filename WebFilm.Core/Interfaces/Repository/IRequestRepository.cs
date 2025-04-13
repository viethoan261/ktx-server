using WebFilm.Core.Enitites.Request;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IRequestRepository : IBaseRepository<int, Request>
    {
        Request CreateRequest(Request request);
        
        Request UpdateRequestStatus(int id, string status, string response);
        
        List<RequestDetailDTO> GetAllRequests();
        
        List<RequestDetailDTO> GetRequestsByStudentId(int studentId);
        
        RequestDetailDTO GetRequestById(int id);
    }
} 