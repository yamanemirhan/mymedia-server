namespace MyMedia.Application.Common
{
    public interface IAuthorizationHelper
    {
        Task EnsureUserIsOwnerOrAdminAsync(Guid resourceOwnerId);
    }
}
