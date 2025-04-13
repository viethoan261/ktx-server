using WebFilm.Core.Enitites.Request;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IRequestService : IBaseService<int, Request>
    {
        Request CreateRequest(RequestDTO requestDTO);
        
        Request RespondToRequest(int id, RequestResponseDTO responseDTO);
        
        List<RequestDetailDTO> GetAllRequests();
        
        List<RequestDetailDTO> GetStudentRequests(int studentId);
        
        RequestDetailDTO GetRequestById(int id);
    }
} 