namespace StudyTodocups.DAL
{
    public interface ICurrentUser
    {
        bool IsLoggedIn();
        int GetUserId();
        bool IsAdmin();
    }
}
