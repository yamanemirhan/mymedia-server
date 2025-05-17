using AutoMapper;
using MediatR;
using MyMedia.Application.Dtos.Response;
using MyMedia.Application.Interfaces;
using MyMedia.Application.Queries.Comment;
using MyMedia.Application.Common;

namespace MyMedia.Application.Handlers.Comment
{
    public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, List<CommentResponseDto>>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationHelper _authorizationHelper;

        public GetAllCommentsQueryHandler(ICommentService commentService, IMapper mapper, IAuthorizationHelper authorizationHelper)
        {
            _commentService = commentService;
            _mapper = mapper;
            _authorizationHelper = authorizationHelper;
        }

        public async Task<List<CommentResponseDto>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            await _authorizationHelper.EnsureUserIsOwnerOrAdminAsync(request.UserId);

            var comments = await _commentService.GetAllCommentsAsync(cancellationToken);
            return _mapper.Map<List<CommentResponseDto>>(comments);
        }
    }
}
