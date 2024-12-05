using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class BlockService : IBlockService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> BlockUserAsync(int blockedById, int blockedUserId)
        {
            // Check if the user is already blocked
            var existingBlock = await _unitOfWork.Repository<BlockUser>()
                .FindAsync(b => b.BlockedById == blockedById && b.BlockedUserId == blockedUserId);

            if (existingBlock != null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "User is already blocked."
                };
            }

            /// Create a new BlockUser entity
            var blockUser = new BlockUser
            {
                BlockedById = blockedById,
                BlockedUserId = blockedUserId
            };

            await _unitOfWork.Repository<BlockUser>().AddAsync(blockUser);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "User blocked successfully."
            };
        }

        public async Task<ResponseDto> UnblockUserAsync(int blockedById, int blockedUserId)
        {
            // Check if the block exists
            var block = await _unitOfWork.Repository<BlockUser>()
                .FindAsync(b => b.BlockedById == blockedById && b.BlockedUserId == blockedUserId);

            if (block == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "This user wasn't blocked"
                };
            }

            _unitOfWork.Repository<BlockUser>().Delete(block);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "User unblocked successfully."
            };
        }

        public async Task<IEnumerable<int>> GetBlockedUserIdsAsync(int userId)
        {
            var blockedUserIds = await _unitOfWork.Repository<BlockUser>()
                .FindAllAsync(b => b.BlockedById == userId, b => b.BlockedUserId);

            return blockedUserIds;
        }

        public async Task<bool> IsBlocked(int userId)
        {
            var blocked = await _unitOfWork.Repository<BlockUser>().FindAsync(u => u.BlockedUserId == userId);
            return blocked == null;
        }

    }
}
