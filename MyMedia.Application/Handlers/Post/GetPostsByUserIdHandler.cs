using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.Post;

namespace MyMedia.Application.Handlers.Post
{
    public class GetPostsByUserIdHandler : IRequestHandler<GetPostsByUserIdQuery, List<PostResponseDto>>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public GetPostsByUserIdHandler(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<List<PostResponseDto>> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postService.GetPostsByUserIdAsync(request.UserId);
            return _mapper.Map<List<PostResponseDto>>(posts);
        }
    }

}
