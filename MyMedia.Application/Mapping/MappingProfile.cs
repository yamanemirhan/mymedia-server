using AutoMapper;
using MyMedia.Application.Dtos;
using MyMedia.Application.Dtos.Request;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Mapping.Resolvers;
using MyMedia.Domain.Entities;
using MyMedia.Domain.Enums;

namespace MyMedia.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Kullanıcı ve DTO eşlemeleri
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.PostCount, opt => opt.MapFrom(src => src.Posts.Count))
                .ForMember(dest => dest.LikedPostCount, opt => opt.MapFrom(src => src.LikedPosts.Count))
                .ForMember(dest => dest.SavedPostCount, opt => opt.MapFrom(src => src.SavedPosts.Count))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.CommentLikeCount, opt => opt.MapFrom(src => src.CommentLikes.Count))
                .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
                .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Followings.Count))
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate));

            // CreatePostRequestDto -> Post
            CreateMap<CreatePostRequestDto, Post>();

            // PostMediaDto -> PostMedia
            CreateMap<PostMediaDto, PostMedia>()
                .ForMember(dest => dest.MediaUrl, opt => opt.MapFrom(src => src.Url)) 
                .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => Enum.Parse<MediaTypeEnum>(src.Type, true)))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ReverseMap();

            // Post -> PostResponseDto 
            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.MediaItems, opt => opt.MapFrom(src => src.MediaItems))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.LikedByUserIds, opt => opt.MapFrom(src => src.LikedByUsers.Select(u => u.Id).ToList()))
                .ForMember(dest => dest.SavedByUserIds, opt => opt.MapFrom(src => src.SavedByUsers.Select(u => u.Id).ToList()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User))
                .ReverseMap();

            // PostMedia -> PostMediaResponseDto
            CreateMap<PostMedia, PostMediaDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.MediaUrl))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.MediaType.ToString()))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ReverseMap();

            // Comment -> CommentResponseDto
            CreateMap<Comment, CommentResponseDto>().ReverseMap();

            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies)).ReverseMap();

            CreateMap<CommentLike, CommentLikeResponseDto>();

            CreateMap<User, CreatedByUserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar));

            CreateMap<UserAvatarMedia, UserAvatarMediaDto>();

            CreateMap<User, FollowerResponseDto>()
                    .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar != null ? src.Avatar : new UserAvatarMedia { AvatarUrl = "https://bu4gkqk43wetzryg.public.blob.vercel-storage.com/def-avatar-S4NnHLL63VypWynP8rLED2sUCNNoBi.jpg" }));

            CreateMap<User, FollowingResponseDto>()
                    .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar != null ? src.Avatar : new UserAvatarMedia { AvatarUrl = "https://bu4gkqk43wetzryg.public.blob.vercel-storage.com/def-avatar-S4NnHLL63VypWynP8rLED2sUCNNoBi.jpg" }));

            CreateMap<User, UserMiniResponseDto>();

            CreateMap<FollowRequest, FollowRequestResponseDto>()
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FromUserId, opt => opt.MapFrom(src => src.RequesterId))
                .ForMember(dest => dest.ToUserId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(dest => dest.User, opt => opt.MapFrom<FollowRequestUserResolver>());
        }
    }
}
