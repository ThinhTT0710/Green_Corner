using AutoMapper;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;

namespace GreenCorner.EventAPI.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IMapper _mapper;
        public VolunteerService(IVolunteerRepository volunteerRepository, IMapper mapper)
        {
            _volunteerRepository = volunteerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VolunteerDTO>> GetAllVolunteer()
        {
            var volunteers = await _volunteerRepository.GetAllVolunteers();
            return _mapper.Map<List<VolunteerDTO>>(volunteers);
        }
        public async Task<VolunteerDTO?> GetVolunteerDetailsAsync(int eventId, string userId)
        {
            var volunteer = await _volunteerRepository.GetVolunteerByEventAndUserAsync(eventId, userId);

            if (volunteer == null)
            {
                return null;
            }

            return _mapper.Map<VolunteerDTO>(volunteer);
        }
        public async Task ApproveTeamLeaderRegistration(int id)
        {
            await _volunteerRepository.ApproveTeamLeaderRegistration(id);
        }

        public async Task ApproveVolunteerRegistration(int id)
        {
            await _volunteerRepository.ApproveVolunteerRegistration(id);
        }

        public async Task<bool> CheckRegisteredAsync(int eventId, string userId, string applicationType)
        {
            return await _volunteerRepository.CheckRegister(eventId, userId, applicationType);
        }

        public async Task<IEnumerable<VolunteerDTO>> GetAllTeamLeaderRegistrations()
        {
            var leaders = await _volunteerRepository.GetAllTeamLeaderRegistrations();
            return _mapper.Map<List<VolunteerDTO>>(leaders);
        }

        public async Task<IEnumerable<VolunteerDTO>> GetAllVolunteerRegistrations()
        {
            var volunteers = await _volunteerRepository.GetAllVolunteerRegistrations();
            return _mapper.Map<List<VolunteerDTO>>(volunteers);
        }

        public async Task<string> GetApprovedRoleAsync(int eventId, string userId)
        {
            return await _volunteerRepository.GetApprovedRoleAsync(eventId, userId);
        }

        public async Task<List<VolunteerDTO>> GetApprovedVolunteersByUserIdAsync(string userId)
        {
            var volunteers = await _volunteerRepository.GetApprovedVolunteersByUserIdAsync(userId);
            return _mapper.Map<List<VolunteerDTO>>(volunteers);
        }

        public async Task<IEnumerable<VolunteerDTO>> GetParticipatedActivitiesByUserId(string userId)
        {
            var volunteers = await _volunteerRepository.GetParticipatedActivitiesByUserId(userId);
            return _mapper.Map<List<VolunteerDTO>>(volunteers);
        }

        public async Task<VolunteerDTO> GetTeamLeaderRegistrationById(int id)
        {
            var leader = await _volunteerRepository.GetTeamLeaderRegistrationById(id);
            return _mapper.Map<VolunteerDTO>(leader);
        }

        public　async Task<IEnumerable<string>> GetUserWithParticipation()
        {
            return await _volunteerRepository.GetUserWithParticipation();
        }

        public async Task<VolunteerDTO> GetVolunteerRegistrationById(int id)
        {
            var volunteer = await _volunteerRepository.GetVolunteerRegistrationById(id);
            return _mapper.Map<VolunteerDTO>(volunteer);
        }

        public async Task<bool> HasApprovedTeamLeaderAsync(int eventId)
        {
            return await _volunteerRepository.HasApprovedTeamLeaderAsync(eventId);
        }

        public async Task<bool> IsTeamLeader(int eventId, string userId)
        {
            return await _volunteerRepository.IsTeamLeader(eventId, userId);
        }

        public async Task<bool> IsVolunteer(int eventId, string userId)
        {
            return await _volunteerRepository.IsVolunteer(eventId, userId);
        }

        public async Task<bool> IsConfirmVolunteer(int eventId, string userId)
        {
            return await _volunteerRepository.IsConfirmVolunteer(eventId, userId);
        }

        public async Task<string> RegisterVolunteer(VolunteerDTO volunteerDto)
        {
            bool isVolunteer = await _volunteerRepository.IsVolunteer(volunteerDto.CleanEventId, volunteerDto.UserId);
            bool isTeamLeader = await _volunteerRepository.IsTeamLeader(volunteerDto.CleanEventId, volunteerDto.UserId);

            if (isVolunteer)
            {
                return "Bạn đã đăng ký làm tình nguyện viên. Vui lòng chờ phê duyệt.";
            }
            else if (isTeamLeader)
            {
                return "Bạn đã đăng ký làm đội trưởng. Vui lòng chờ phê duyệt.";
            }

            // Vẫn cho phép đăng ký thêm vai trò khác nếu chưa trùng
            Volunteer volunteer = _mapper.Map<Volunteer>(volunteerDto);
            await _volunteerRepository.RegisteredVolunteer(volunteer);
            return "Đăng ký thành công!";
        }

        public async Task RejectTeamLeaderRegistration(int id)
        {
            await _volunteerRepository.RejectTeamLeaderRegistration(id);
        }

        public async Task RejectVolunteerRegistration(int id)
        {
            await _volunteerRepository.RejectVolunteerRegistration(id);
        }

        public async Task<string> UnregisterAsync(int eventId, string userId, string role)
        {
            bool isVolunteer = await _volunteerRepository.IsVolunteer(eventId, userId);
            bool isTeamLeader = await _volunteerRepository.IsTeamLeader(eventId, userId);

            if (role == "Volunteer")
            {
                if (!isVolunteer)
                    return "Bạn chưa đăng ký làm tình nguyện viên nên không thể hủy.";

                await _volunteerRepository.UnRegisteVolunteer(eventId, userId, role);
                return "Hủy đăng ký tình nguyện viên thành công.";
            }
            else if (role == "TeamLeader")
            {
                if (!isTeamLeader)
                    return "Bạn chưa đăng ký làm đội trưởng nên không thể hủy.";

                await _volunteerRepository.UnRegisteVolunteer(eventId, userId, role);
                return "Hủy đăng ký đội trưởng thành công.";
            }

            return "Vai trò không hợp lệ.";
        }

        public async Task UpdateRegister(VolunteerDTO volunteerDto)
        {
            Volunteer volunteer = _mapper.Map<Volunteer>(volunteerDto);
            await _volunteerRepository.UpdateRegister(volunteer);
        }
        public async Task<string> GetTeamLeaderByEventId(int eventId)
        {
            return await _volunteerRepository.GetTeamLeaderByEventId(eventId);
        }
    }
}
