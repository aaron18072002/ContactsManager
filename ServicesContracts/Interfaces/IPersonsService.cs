using ServicesContracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.Interfaces
{
    public interface IPersonsService
    {
        /// <summary>
        /// Add a person object to list of persons
        /// </summary>
        /// <param name="personAddRequest">A person object to add</param>
        /// <returns>Returns the PersonResponse after adding it (including newly generated person id)</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Return all people from datasource
        /// </summary>
        /// <returns>Returns list of person response object</returns>
        List<PersonResponse> GetAllPersons();
    }
}
