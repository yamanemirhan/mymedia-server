using MyMedia.Domain.Entities;

namespace MyMedia.Application.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(Post post);
        Task<List<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(Guid postId);
        Task SaveChangesAsync();
        Task<List<Post>> GetPostsByUserIdAsync(Guid userId);
        Task<List<Post>> GetSavedPostsByUserIdAsync(Guid userId);
        Task DeletePostByIdAsync(Guid postId);
    }
}
