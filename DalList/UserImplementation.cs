namespace Dal;
using DO;

internal class UserImplementation : DalApi.IUser
{
    //create a new user
    public string Create(User item)
    {
        DataSource.Users.Add(item);//add the task to the list
        return item.Email;//return the email
    }

    //read a task and return it. if not found return null
    public User? Read(string email)
    {
        return DataSource.Users.FirstOrDefault(user => user.Email == email);//find the user with the email and if not found return null
    }

    //read a user that matches the condition and return it. if not found return null
    public User? Read(Func<User, bool> filter)
    {
        // find the user that matches the condition and if not found return null
        return DataSource.Users.FirstOrDefault(filter);
    }

    //return a copy of the list of users
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter != null)
            return DataSource.Users.Where(filter);//return a copy of the list of users

        return DataSource.Users.Select(user => user);//return a copy of the list of users
    }

    //update a task by removing the old one and adding the new one
    public void Update(User item)
    {
        //find the index of the user with the same email
        User? user = DataSource.Users.FirstOrDefault(user => user.Email == item.Email);
        if (user == null)//if not found
            throw new DalDoesNotExistException($"User with Email={item.Email} does not exist");//throw exception

        //remove the user
        DataSource.Users.RemoveAll(u => u.Email == user.Email);
        DataSource.Users.Add(item);//add the new user
    }

    //we dont need to check if there is no users with the user email because we will check it in the logic layer
    public void Delete(string email)
    {
        User? user = DataSource.Users.FirstOrDefault(user => user.Email == email);//find the index of the user with the same email
        if (user == null)//if not found
            throw new DalDoesNotExistException($"User with Email={email} does not exist");//throw exception

        DataSource.Users.RemoveAll(user => user.Email == email);//remove the user
    }

    //delete all users and add the admin user
    public void DeleteAll() { 
        DataSource.Users.Clear();
        DataSource.Users.Add(new User{
            Type = UserType.manager,
            Name = "admin",
            Password = "Admin123",
            Email = "admin@gmail.com"
        });
    }
}

