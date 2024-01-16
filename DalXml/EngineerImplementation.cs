using DalApi;
using DO;

namespace Dal;

internal class EngineerImplementation : IEngineer
{

    readonly string s_engineer_xml = "engineers";
    public int Create(Engineer item)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>("engineers");
        
        if( engineersList.Exists(e => e.Id == item.Id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

        engineersList.Add(item);
        XMLTools.SaveListToXMLSerializer(engineersList, "engineers");

        return item.Id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {
        throw new NotImplementedException();
    }
}