using MyMedia.Application.Interfaces;
using MyMedia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyMedia.Domain.Entities;
using MyMedia.Shared.GlobalErrorHandling;
using System.Net;

namespace MyMedia.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Avatar)
                .Include(u => u.Posts)
                .Include(u => u.LikedPosts)
                .Include(u => u.SavedPosts)
                .Include(u => u.Comments)
                .Include(u => u.CommentLikes)
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .Include(u => u.OutgoingFollowRequests)
                .Include(u => u.IncomingFollowRequests)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.Avatar)
                .Include(u => u.Posts)
                .Include(u => u.LikedPosts)
                .Include(u => u.SavedPosts)
                .Include(u => u.Comments)
                .Include(u => u.CommentLikes)
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .Include(u => u.OutgoingFollowRequests)
                .Include(u => u.IncomingFollowRequests)
                .FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }

        public async Task FollowUserAsync(Guid currentUserId, Guid targetUserId, CancellationToken cancellationToken)
        {
            var currentUser = await GetUserWithFollowDataAsync(currentUserId, cancellationToken);
            var targetUser = await _context.Users
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .Include(u => u.IncomingFollowRequests)
                .Include(u => u.OutgoingFollowRequests)
                .FirstOrDefaultAsync(u => u.Id == targetUserId, cancellationToken);

            if (currentUser == null || targetUser == null)
                throw new CustomException("User not found.", 404);

            if (currentUserId == targetUserId)
                throw new CustomException("You cannot follow yourself.", 400);

            if (targetUser.Followers.Any(f => f.Id == currentUserId))
            {
                throw new CustomException("You are already following this user.", 400);
            }

            if (targetUser.IsPrivate)
            {
                await SendFollowRequestAsync(currentUserId, targetUserId, currentUser, targetUser, cancellationToken);
                return;
            }

            await FollowUserDirectlyAsync(currentUser, targetUser, cancellationToken);
        }

        public async Task<User> GetUserWithFollowDataAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Followings)
                .Include(u => u.Followers)
                .Include(u => u.IncomingFollowRequests)
                .Include(u => u.OutgoingFollowRequests)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        private async Task SendFollowRequestAsync(Guid currentUserId, Guid targetUserId, User currentUser, User targetUser, CancellationToken cancellationToken)
        {
            var existingRequest = await _context.FollowRequests
                .FirstOrDefaultAsync(req =>
                    req.RequesterId == currentUserId &&
                    req.ReceiverId == targetUserId,
                    cancellationToken);

            if (existingRequest != null)
                throw new CustomException("Follow request already sent.", 400);

            var followRequest = new FollowRequest
            {
                RequesterId = currentUserId,
                ReceiverId = targetUserId
            };

            await _context.FollowRequests.AddAsync(followRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task FollowUserDirectlyAsync(User currentUser, User targetUser, CancellationToken cancellationToken)
        {
            if (!targetUser.Followers.Any(f => f.Id == currentUser.Id))
            {
                targetUser.Followers.Add(currentUser);
                currentUser.Followings.Add(targetUser);

                _context.Users.Update(currentUser);
                _context.Users.Update(targetUser);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UnfollowUserAsync(Guid currentUserId, Guid targetUserId, CancellationToken cancellationToken)
        {
            var currentUser = await GetUserWithFollowDataAsync(currentUserId, cancellationToken);
            var targetUser = await _context.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == targetUserId, cancellationToken);

            if (currentUser == null || targetUser == null)
                throw new CustomException("User not found.", 404);

            if (currentUserId == targetUserId)
                throw new CustomException("You cannot unfollow yourself.", 400);

            var isFollowing = targetUser.Followers.Any(f => f.Id == currentUserId);
            if (!isFollowing)
                throw new CustomException("You are not following this user.", 400);

            targetUser.Followers.Remove(currentUser);
            currentUser.Followings.Remove(targetUser);

            _context.Users.Update(currentUser);
            _context.Users.Update(targetUser);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ApproveFollowRequestAsync(Guid receiverId, Guid requesterId, CancellationToken cancellationToken)
        {
            var receiver = await _context.Users
                .Include(u => u.Followers)
                .Include(u => u.IncomingFollowRequests)
                .FirstOrDefaultAsync(u => u.Id == receiverId, cancellationToken);

            var requester = await _context.Users
                .Include(u => u.Followings)
                .FirstOrDefaultAsync(u => u.Id == requesterId, cancellationToken);

            if (receiver == null || requester == null)
                throw new CustomException("User not found.", (int)HttpStatusCode.NotFound);

            var followRequest = receiver.IncomingFollowRequests
                .FirstOrDefault(fr => fr.RequesterId == requesterId && fr.ReceiverId == receiverId);

            if (followRequest == null)
                throw new CustomException("Follow request not found.", (int)HttpStatusCode.NotFound);

            receiver.Followers.Add(requester);
            requester.Followings.Add(receiver);

            _context.FollowRequests.Remove(followRequest);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RejectFollowRequestAsync(Guid receiverId, Guid requesterId, CancellationToken cancellationToken)
        {
            var receiver = await _context.Users
                .Include(u => u.IncomingFollowRequests)
                .FirstOrDefaultAsync(u => u.Id == receiverId, cancellationToken);

            if (receiver == null)
                throw new CustomException("Receiver not found.", (int)HttpStatusCode.NotFound);

            var followRequest = receiver.IncomingFollowRequests
                .FirstOrDefault(fr => fr.RequesterId == requesterId && fr.ReceiverId == receiverId);

            if (followRequest == null)
                throw new CustomException("Follow request not found.", (int)HttpStatusCode.NotFound);

            _context.FollowRequests.Remove(followRequest);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CancelFollowRequestAsync(Guid requesterId, Guid receiverId, CancellationToken cancellationToken)
        {
            var requester = await _context.Users
                .Include(u => u.OutgoingFollowRequests)
                .FirstOrDefaultAsync(u => u.Id == requesterId, cancellationToken);

            if (requester == null)
                throw new CustomException("Requester not found.", (int)HttpStatusCode.NotFound);

            var followRequest = requester.OutgoingFollowRequests
                .FirstOrDefault(fr => fr.RequesterId == requesterId && fr.ReceiverId == receiverId);

            if (followRequest == null)
                throw new CustomException("Follow request not found.", (int)HttpStatusCode.NotFound);

            _context.FollowRequests.Remove(followRequest);

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Avatar)
                .Include(u => u.Posts)
                .Include(u => u.LikedPosts)
                .Include(u => u.SavedPosts)
                .Include(u => u.Comments)
                .Include(u => u.CommentLikes)
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .Include(u => u.IncomingFollowRequests)
                .Include(u => u.OutgoingFollowRequests)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FollowRequest>> GetIncomingFollowRequestsAsync(Guid userId)
        {
            return await _context.FollowRequests
                .Where(r => r.ReceiverId == userId)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Avatar)
                .ToListAsync();
        }

        public async Task<List<FollowRequest>> GetOutgoingFollowRequestsAsync(Guid userId)
        {
            return await _context.FollowRequests
                .Where(r => r.RequesterId == userId)
                .Include(r => r.Receiver)
                    .ThenInclude(u => u.Avatar)
                .ToListAsync();
        }
    }
}
