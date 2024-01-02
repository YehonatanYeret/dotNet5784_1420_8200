using System.Security.Cryptography.X509Certificates;

namespace DO;

public record Engineer
(
    int ID,
    double Cost,
    string Name,
    string Email,
    DO.EngineerExperience Level

 


     
)
{ 
    public Engineer() : this(0, "", "", 0) { }  
}
