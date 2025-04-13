using WebFilm.Core.Enitites.Request;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class RequestService : BaseService<int, Request>, IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserContext _userContext;

        public RequestService(IRequestRepository requestRepository, IUserContext userContext) : base(requestRepository)
        {
            _requestRepository = requestRepository;
            _userContext = userContext;
        }

        public Request CreateRequest(RequestDTO requestDTO)
        {
            // Validate input
            if (string.IsNullOrEmpty(requestDTO.title) || string.IsNullOrEmpty(requestDTO.content))
            {
                throw new ServiceException("Tiêu đề và nội dung không được để trống");
            }

            if (string.IsNullOrEmpty(requestDTO.type) || 
                (!requestDTO.type.Equals("REQUEST") && !requestDTO.type.Equals("COMPLAINT")))
            {
                throw new ServiceException("Loại yêu cầu không hợp lệ");
            }

            // Create request entity
            var request = new Request
            {
                studentId = requestDTO.studentId,
                title = requestDTO.title,
                content = requestDTO.content,
                type = requestDTO.type,
                status = "PENDING"
            };

            // Save to database
            var createdRequest = _requestRepository.CreateRequest(request);
            if (createdRequest == null)
            {
                throw new ServiceException("Tạo yêu cầu thất bại");
            }

            return createdRequest;
        }

        public Request RespondToRequest(int id, RequestResponseDTO responseDTO)
        {
            // Check permissions
            string role = _userContext.Role;
            if ("STUDENT".Equals(role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            // Validate status
            if (string.IsNullOrEmpty(responseDTO.status) || 
                (!responseDTO.status.Equals("PENDING") && 
                 !responseDTO.status.Equals("PROCESSING") && 
                 !responseDTO.status.Equals("RESOLVED")))
            {
                throw new ServiceException("Trạng thái không hợp lệ");
            }

            // Update request
            var updatedRequest = _requestRepository.UpdateRequestStatus(id, responseDTO.status, responseDTO.response);
            if (updatedRequest == null)
            {
                throw new ServiceException("Cập nhật yêu cầu thất bại");
            }

            return updatedRequest;
        }

        public List<RequestDetailDTO> GetAllRequests()
        {
            // Check permissions
            string role = _userContext.Role;
            if ("STUDENT".Equals(role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            return _requestRepository.GetAllRequests();
        }

        public List<RequestDetailDTO> GetStudentRequests(int studentId)
        {
            // Check permissions for non-students trying to view other students' requests
            string role = _userContext.Role;
            int userId = _userContext.UserID;
            
            if ("STUDENT".Equals(role) && userId != studentId)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            return _requestRepository.GetRequestsByStudentId(studentId);
        }

        public RequestDetailDTO GetRequestById(int id)
        {
            var request = _requestRepository.GetRequestById(id);
            
            if (request == null)
            {
                throw new ServiceException("Yêu cầu không tồn tại");
            }

            // Check permissions for students
            string role = _userContext.Role;
            int userId = _userContext.UserID;
            
            if ("STUDENT".Equals(role) && userId != request.studentId)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            return request;
        }
    }
} 