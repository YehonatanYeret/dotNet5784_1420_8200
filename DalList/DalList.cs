using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    /// <summary>
    /// Represents a Data Access Layer (DAL) list providing access to different entities.
    /// </summary>
    public sealed class DalList : IDal
    {
        /// <summary>
        /// Gets an instance of the <see cref="IEngineer"/> interface for accessing engineer-related data.
        /// </summary>
        public IEngineer Engineer => new EngineerImplementation();

        /// <summary>
        /// Gets an instance of the <see cref="ITask"/> interface for accessing task-related data.
        /// </summary>
        public ITask Task => new TaskImplementation();

        /// <summary>
        /// Gets an instance of the <see cref="IDependency"/> interface for accessing dependency-related data.
        /// </summary>
        public IDependency Dependency => new DependencyImplementation();
    }
}
