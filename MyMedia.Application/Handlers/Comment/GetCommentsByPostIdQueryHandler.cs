using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.Comment;

namespace MyMedia.Application.Handlers.Comment
{
    public class GetCommentsByPostIdQueryHandler : IRequestHandler<GetCommentsByPostIdQuery, List<CommentResponseDto>>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public GetCommentsByPostIdQueryHandler(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        public async Task<List<CommentResponseDto>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentService.GetCommentsByPostIdAsync(request.PostId, cancellationToken);

            return _mapper.Map<List<CommentResponseDto>>(comments);
        }
    }

}
