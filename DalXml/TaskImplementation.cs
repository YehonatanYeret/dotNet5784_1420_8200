using DalApi;
using DO;

namespace Dal
{
    internal class TaskImplementation : ITask
    {
        readonly string s_task_xml = "tasks";
        public int Create(DO.Task item)
        {
            List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>("tasks");

            if (tasksList.Exists(e => e.Id == item.Id))
                throw new DalAlreadyExistsException($"Task with ID={item.Id} already exists");

            int nextId =Config.NextTaskId;
            tasksList.Add(item with { Id = nextId, CreatedAtDate =DateTime.Now});

            XMLTools.SaveListToXMLSerializer(tasksList, "tasks");

            return nextId;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public DO.Task? Read(int id)
        {
            throw new NotImplementedException();
        }

        public DO.Task? Read(Func<DO.Task, bool> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.Task item)

        {
            List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>("tasks");

            bool found = tasksList.Any(e => e.Id == item.Id);

            if (!found)
                throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

            tasksList.RemoveAll(x => x.Id == item.Id);
            tasksList.Add(item);


            XMLTools.SaveListToXMLSerializer(tasksList, "tasks");

        }
    }
}