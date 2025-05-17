using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.Post;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Application.Handlers.Post
{
    public class GetSinglePostByIdQueryHandler : IRequestHandler<GetSinglePostByIdQuery, PostResponseDto>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public GetSinglePostByIdQueryHandler(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<PostResponseDto> Handle(GetSinglePostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);

            if (post is null)
                throw new CustomException("Post bulunamadı.", 404);

            return _mapper.Map<PostResponseDto>(post);
        }
    }
}
