namespace MyMedia.Application.Interfaces
{
    public interface IPostLikeService
    {
        Task RemoveLikeAsync(int postId, int userId, CancellationToken cancellationToken = default);
    }

}
