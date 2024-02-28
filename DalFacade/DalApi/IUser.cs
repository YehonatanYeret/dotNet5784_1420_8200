namespace DalApi;
using DO;

/// <summary>
/// creates an interface that derived from ICrud with T=User
/// </summary>
public interface IUser : ICrud<User, string> { }
