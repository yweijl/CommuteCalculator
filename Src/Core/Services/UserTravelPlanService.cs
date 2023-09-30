using Core.Interfaces;
using Core.Models.Csv;
using Core.Models.Travelplans;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Core.Services
{
    public class UserTravelplanService : IUserTravelplanService
    {
        private readonly IUserTravelplansRepository _userTravelplansRepository;
        private readonly ICsvService _csvService;

        public UserTravelplanService(IUserTravelplansRepository userTravelplansRepository, ICsvService csvService)
        {
            _userTravelplansRepository = userTravelplansRepository;
            _csvService = csvService;
        }

        public Task<bool> DeleteTravelplanByIdAsync(Guid id, Guid userId)
        {
            return _userTravelplansRepository.DeleteTravelplanByIdAsync(id, userId);
        }

        public async Task<(string, byte[])> DownloadTravelplanAsync(int monthNumber, Guid userId)
        {
            var travelplans = await _userTravelplansRepository.GetTravelplansByMonthAsync(monthNumber, userId);
            return _csvService.CreateTravelplanRegistration(travelplans);
        }

        public Task<UserTravelplan> GetPersistedTravelplansAsync(Guid userId)
        {
            return _userTravelplansRepository.GetByUserIdAsync(userId);
        }

        public Task<bool> SaveAsync(Guid userId, Travelplan travelPlan)
        {

            return _userTravelplansRepository.SaveAsync(userId, travelPlan);
        }
    }
}
