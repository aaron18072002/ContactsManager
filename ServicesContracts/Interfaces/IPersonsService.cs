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
        /// Returns all people from datasource
        /// </summary>
        /// <returns>Returns list of person response object</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns a matching person from datasource based on the given id
        /// </summary>
        /// <param name="personId">PersonId to search</param>
        /// <returns>Return a PersonResponse type</returns>
        PersonResponse? GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// Returns a list of matching persons from datasource given searchBy and searchString
        /// </summary>
        /// <param name="searchBy">SearchField to search</param>
        /// <param name="searchString">SearchValue to search</param>
        /// <returns>Returns all matching persons as a list of PersonResponse type</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
    }
}
