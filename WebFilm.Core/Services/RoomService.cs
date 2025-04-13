using WebFilm.Core.Enitites.Room;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class RoomService : BaseService<int, Rooms>, IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserContext _userContext;

        public RoomService(IRoomRepository roomRepository, IUserContext userContext) : base(roomRepository)
        {
            _roomRepository = roomRepository;
            _userContext = userContext;
        }

        public Rooms CreateRoom(RoomDTO roomDTO)
        {
            // Validate user permissions (if needed)
            string role = _userContext.Role;
            if ("STAFF".Equals(role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            // Validate input
            if (string.IsNullOrEmpty(roomDTO.floorNumber) || string.IsNullOrEmpty(roomDTO.roomNumber))
            {
                throw new ServiceException("Thông tin phòng không hợp lệ");
            }

            if (roomDTO.maxOccupancy <= 0)
            {
                throw new ServiceException("Sức chứa phòng phải lớn hơn 0");
            }

            // Validate current occupancy
            if (roomDTO.studentIds != null && roomDTO.studentIds.Count > roomDTO.maxOccupancy)
            {
                throw new ServiceException("Số lượng sinh viên vượt quá sức chứa của phòng");
            }

            // Create room
            var room = new Rooms
            {
                floorNumber = roomDTO.floorNumber,
                roomNumber = roomDTO.roomNumber,
                maxOccupancy = roomDTO.maxOccupancy,
                status = roomDTO.status,
                currentOccupancy = roomDTO.studentIds?.Count ?? 0
            };

            var createdRoom = _roomRepository.CreateRoom(room);
            if (createdRoom == null)
            {
                throw new ServiceException("Tạo phòng thất bại");
            }

            // Add students to room
            if (roomDTO.studentIds != null && roomDTO.studentIds.Any())
            {
                bool success = _roomRepository.AddStudentsToRoom(createdRoom.id, roomDTO.studentIds);
                if (!success)
                {
                    throw new ServiceException("Thêm sinh viên vào phòng thất bại");
                }
            }

            return createdRoom;
        }

        public List<RoomResponseDTO> GetAllRoomsWithStudents()
        {
            // Check permissions if needed
            string role = _userContext.Role;
            if ("STAFF".Equals(role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            // Get all rooms with students
            var rooms = _roomRepository.GetAllRoomsWithStudents();
            return rooms;
        }

        public Rooms UpdateRoom(int id, RoomDTO roomDTO)
        {
            // Check if room exists
            var existingRoom = _roomRepository.GetByID(id);
            if (existingRoom == null)
            {
                throw new ServiceException("Phòng không tồn tại");
            }

            // Validate input
            if (string.IsNullOrEmpty(roomDTO.floorNumber) || string.IsNullOrEmpty(roomDTO.roomNumber))
            {
                throw new ServiceException("Thông tin phòng không hợp lệ");
            }

            if (roomDTO.maxOccupancy <= 0)
            {
                throw new ServiceException("Sức chứa phòng phải lớn hơn 0");
            }

            // Validate current occupancy
            if (roomDTO.studentIds != null && roomDTO.studentIds.Count > roomDTO.maxOccupancy)
            {
                throw new ServiceException("Số lượng sinh viên vượt quá sức chứa của phòng");
            }

            // Create room object to update
            var roomToUpdate = new Rooms
            {
                floorNumber = roomDTO.floorNumber,
                roomNumber = roomDTO.roomNumber,
                maxOccupancy = roomDTO.maxOccupancy,
                status = roomDTO.status,
                currentOccupancy = roomDTO.studentIds?.Count ?? 0
            };

            // Remove all students from the room
            bool removeSuccess = _roomRepository.RemoveAllStudentsFromRoom(id);
            if (!removeSuccess)
            {
                throw new ServiceException("Xóa sinh viên khỏi phòng thất bại");
            }

            // Update room
            var updatedRoom = _roomRepository.UpdateRoom(id, roomToUpdate);
            if (updatedRoom == null)
            {
                throw new ServiceException("Cập nhật phòng thất bại");
            }

            // Add students to room
            if (roomDTO.studentIds != null && roomDTO.studentIds.Any())
            {
                bool addSuccess = _roomRepository.AddStudentsToRoom(id, roomDTO.studentIds);
                if (!addSuccess)
                {
                    throw new ServiceException("Thêm sinh viên vào phòng thất bại");
                }
            }

            return updatedRoom;
        }

        public bool DeleteRoom(int id)
        {
            // Validate user permissions (if needed)
            string role = _userContext.Role;
            if ("STAFF".Equals(role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            // Check if room exists
            var existingRoom = _roomRepository.GetByID(id);
            if (existingRoom == null)
            {
                throw new ServiceException("Phòng không tồn tại");
            }

            // First remove all students from room
            bool removeSuccess = _roomRepository.RemoveAllStudentsFromRoom(id);
            if (!removeSuccess)
            {
                throw new ServiceException("Xóa sinh viên khỏi phòng thất bại");
            }

            // Then delete the room
            int result = _roomRepository.Delete(id);
            if (result == 0)
            {
                throw new ServiceException("Xóa phòng thất bại");
            }

            return true;
        }
    }
} 