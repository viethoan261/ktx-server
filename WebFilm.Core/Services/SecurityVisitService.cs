using System;
using System.Collections.Generic;
using WebFilm.Core.Enitites.Security;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class SecurityVisitService : BaseService<int, SecurityVisit>, ISecurityVisitService
    {
        private readonly ISecurityVisitRepository _securityVisitRepository;
        private readonly IUserRepository _userRepository;

        public SecurityVisitService(ISecurityVisitRepository securityVisitRepository, IUserRepository userRepository)
            : base(securityVisitRepository)
        {
            _securityVisitRepository = securityVisitRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<SecurityVisitResponseDTO> GetAllVisits()
        {
            var visits = _securityVisitRepository.GetAllVisits();
            return MapToResponseDTOs(visits);
        }

        public IEnumerable<SecurityVisitResponseDTO> GetActiveVisits()
        {
            var visits = _securityVisitRepository.GetActiveVisits();
            return MapToResponseDTOs(visits);
        }

        public IEnumerable<SecurityVisitResponseDTO> GetVisitsByDate(DateTime date)
        {
            var visits = _securityVisitRepository.GetVisitsByDate(date);
            return MapToResponseDTOs(visits);
        }

        public IEnumerable<SecurityVisitResponseDTO> GetVisitsByStudentId(int studentId)
        {
            var visits = _securityVisitRepository.GetVisitsByStudentId(studentId);
            return MapToResponseDTOs(visits);
        }

        public SecurityVisitResponseDTO CheckIn(SecurityVisitDTO visitDTO)
        {
            var visit = new SecurityVisit
            {
                visitorName = visitDTO.visitorName,
                phoneNumber = visitDTO.phoneNumber,
                studentId = visitDTO.studentId,
                purpose = visitDTO.purpose,
                notes = visitDTO.notes,
                entryTime = DateTime.Now,
                status = "CHECKED_IN",
                createdDate = DateTime.Now,
                modifiedDate = DateTime.Now
            };

            _securityVisitRepository.CreateVisit(visit);
            return MapToResponseDTO(visit);
        }

        public SecurityVisitResponseDTO CheckOut(int id)
        {
            var exitTime = DateTime.Now;
            var result = _securityVisitRepository.CheckOut(id, exitTime);
            
            if (result == 0)
            {
                return null; // Visit not found or already checked out
            }
            
            var visit = _securityVisitRepository.GetVisitById(id);
            return MapToResponseDTO(visit);
        }

        public SecurityVisitResponseDTO GetVisitById(int id)
        {
            var visit = _securityVisitRepository.GetVisitById(id);
            if (visit == null)
            {
                return null;
            }
            return MapToResponseDTO(visit);
        }

        public int DeleteVisit(int id)
        {
            return _securityVisitRepository.DeleteVisit(id);
        }

        private IEnumerable<SecurityVisitResponseDTO> MapToResponseDTOs(IEnumerable<SecurityVisit> visits)
        {
            var responseDTOs = new List<SecurityVisitResponseDTO>();
            
            foreach (var visit in visits)
            {
                responseDTOs.Add(MapToResponseDTO(visit));
            }
            
            return responseDTOs;
        }

        private SecurityVisitResponseDTO MapToResponseDTO(SecurityVisit visit)
        {
            string studentName = null;
            if (visit.studentId.HasValue)
            {
                var student = _userRepository.GetByID(visit.studentId.Value);
                studentName = student?.fullname;
            }
            
            return new SecurityVisitResponseDTO
            {
                id = visit.id,
                visitorName = visit.visitorName,
                phoneNumber = visit.phoneNumber,
                studentId = visit.studentId,
                studentName = studentName,
                entryTime = visit.entryTime,
                exitTime = visit.exitTime,
                status = visit.status,
                purpose = visit.purpose,
                notes = visit.notes,
                createdDate = visit.createdDate,
                modifiedDate = visit.modifiedDate
            };
        }
    }
} 