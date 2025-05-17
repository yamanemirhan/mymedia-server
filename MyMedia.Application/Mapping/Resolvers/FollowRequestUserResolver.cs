using AutoMapper;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Dtos;
using MyMedia.Domain.Entities;

namespace MyMedia.Application.Mapping.Resolvers
{
    public class FollowRequestUserResolver : IValueResolver<FollowRequest, FollowRequestResponseDto, UserMiniResponseDto>
    {
        public UserMiniResponseDto Resolve(FollowRequest source, FollowRequestResponseDto destination, UserMiniResponseDto destMember, ResolutionContext context)
        {
            var currentUserId = context.Items["CurrentUserId"] as Guid?;
            var isOutgoing = currentUserId == source.RequesterId;

            var user = isOutgoing ? source.Receiver : source.Requester;

            return new UserMiniResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Avatar = user.Avatar != null
                    ? new UserAvatarMediaDto { AvatarUrl = user.Avatar.AvatarUrl }
                    : null
            };
        }
    }
}
