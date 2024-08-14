using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesContracts
{
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);

        Task<List<Person>> GetAllPersons();

        Task<Person?> GetPersonById(Guid personId);

        Task<List<Person>?> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        Task<bool> DeletePerson(Guid personId);

        Task<Person> UpdatePerson(Person person);
    }
}
