
public class UserService
{
    public User? LoggedInUser;

    public void AddNewUser(User user)
    {
        using var db = new AppContext();
        db.Users.Add(user);
        db.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        using var db = new AppContext();
        db.Users.Update(user);
        db.SaveChanges();
    }

    public void RemoveUser(User user)
    {
        using var db = new AppContext();
        db.Users.Remove(user);
        db.SaveChanges();
    }

    public void Logout()
    {
        LoggedInUser = null;
    }

    public bool TryLogin(string username, string password)
    {
        using var db = new AppContext();

        if(TryFindUser(username, out var user))
        {
            var passMatch = user!.Password.Equals(password);

            if(passMatch)
            {
                LoggedInUser = user;
                return true;
            }
        }

        return false;
    }

    public bool TryFindUser(string username, out User? user)
    {
        using var db = new AppContext();
        user = db.Users.FirstOrDefault(u => u.Username.Equals(username));
        return user != null;
    }
}