using TwitterClone.Service.DTOs;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IBlockService
    {
        Task<ResponseDto> BlockUserAsync(int blockedById, int blockedUserId);
        Task<ResponseDto> UnblockUserAsync(int blockedById, int blockedUserId);
        Task<IEnumerable<int>> GetBlockedUserIdsAsync(int userId);
        Task<bool> IsBlocked(int userId);
    }
}
