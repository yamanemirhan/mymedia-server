using MyMedia.Application.Commands.User;
using MyMedia.Application.Dtos.Response;
using MyMedia.Domain.Entities;

namespace MyMedia.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task SaveChangesAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task FollowUserAsync(Guid currentUserId, Guid targetUserId, CancellationToken cancellationToken);
        Task<User> GetUserWithFollowDataAsync(Guid userId, CancellationToken cancellationToken);
        Task UnfollowUserAsync(Guid currentUserId, Guid targetUserId, CancellationToken cancellationToken);
        Task ApproveFollowRequestAsync(Guid receiverId, Guid requesterId, CancellationToken cancellationToken);
        Task RejectFollowRequestAsync(Guid receiverId, Guid requesterId, CancellationToken cancellationToken);
        Task CancelFollowRequestAsync(Guid requesterId, Guid receiverId, CancellationToken cancellationToken);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<List<FollowRequest>> GetIncomingFollowRequestsAsync(Guid userId);
        Task<List<FollowRequest>> GetOutgoingFollowRequestsAsync(Guid userId);
    }
}
