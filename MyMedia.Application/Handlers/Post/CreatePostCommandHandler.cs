using AutoMapper;
using MediatR;
using MyMedia.Application.Commands.Post;
using MyMedia.Application.Interfaces;
using MyMedia.Domain.Entities;

namespace MyMedia.Application.Handlers.Post
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<MyMedia.Domain.Entities.Post>(request.PostDto);
            post.UserId = request.UserId;

            post.MediaItems = request.PostDto.MediaItems
                .Select(m => _mapper.Map<PostMedia>(m))
                .ToList();

            await _postService.CreatePostAsync(post);
            return post.Id;
        }

    }
}


