using AutoMapper;
using MediatR;
using MyMedia.Application.Commands.Comment;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;

namespace MyMedia.Application.Handlers.Comment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;
        public CreateCommentCommandHandler(ICommentService commentService, IMapper mapper)
        {
            _mapper = mapper;
            _commentService = commentService;
        }

        public async Task<CommentResponseDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentService.CreateCommentAsync(
                request.UserId,
                request.Text,
                request.PostId,
                request.ParentCommentId,
                cancellationToken
            );

            return _mapper.Map<CommentResponseDto>(comment);
        }
    }
}
