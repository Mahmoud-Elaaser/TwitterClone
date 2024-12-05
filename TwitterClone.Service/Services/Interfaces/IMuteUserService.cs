namespace TwitterClone.Service.Services.Interfaces
{
    public interface IMuteUserService
    {
        Task<bool> MuteUserAsync(int mutedById, int mutedUserId);
        Task<bool> UnmuteUserAsync(int mutedById, int mutedUserId);
        Task<bool> IsUserMutedAsync(int mutedById, int mutedUserId);

    }
}
