using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class MuteUserService : IMuteUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MuteUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> MuteUserAsync(int mutedById, int mutedUserId)
        {
            if (mutedById == mutedUserId)
                throw new InvalidOperationException("You cannot mute yourself.");

            /// Check if already muted
            var existingMute = await _unitOfWork.Repository<MuteUser>().FindAsync(
                mu => mu.MutedById == mutedById && mu.MutedUserId == mutedUserId);

            if (existingMute != null)
                return false;

            var mute = new MuteUser { MutedById = mutedById, MutedUserId = mutedUserId };
            await _unitOfWork.Repository<MuteUser>().AddAsync(mute);

            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> UnmuteUserAsync(int mutedById, int mutedUserId)
        {
            var mute = await _unitOfWork.Repository<MuteUser>().FindAsync(
                mu => mu.MutedById == mutedById && mu.MutedUserId == mutedUserId);

            if (mute == null)
                return false;

            _unitOfWork.Repository<MuteUser>().Delete(mute);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> IsUserMutedAsync(int mutedById, int mutedUserId)
        {
            return await _unitOfWork.Repository<MuteUser>().AnyAsync(
                mu => mu.MutedById == mutedById && mu.MutedUserId == mutedUserId);
        }
    }

}
