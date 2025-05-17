using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.Post;

namespace MyMedia.Application.Handlers.Post
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<PostResponseDto>>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<List<PostResponseDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postService.GetAllPostsAsync();

            var postDtos = _mapper.Map<List<PostResponseDto>>(posts);

            return postDtos;
        }

    }
}
