using ServicesContracts.DTOs;
using ServicesContracts.Enums;
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
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all people from datasource
        /// </summary>
        /// <returns>Returns list of person response object</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns a matching person from datasource based on the given id
        /// </summary>
        /// <param name="personId">PersonId to search</param>
        /// <returns>Return a PersonResponse type</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// Returns a list of matching persons from datasource given searchBy and searchString
        /// </summary>
        /// <param name="searchBy">SearchField to search</param>
        /// <param name="searchString">SearchValue to search</param>
        /// <returns>Returns all matching persons as a list of PersonResponse type</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns a list of sorted persons by given arguments
        /// </summary>
        /// <param name="allPersons">A list of person to sort</param>
        /// <param name="sortBy">Name of a property (key) to sort</param>
        /// <param name="sortOrderOptions">ASC and DESC</param>
        /// <returns>Returns a list of PersonResponse after sorted</returns>
        List<PersonResponse> GetSortedPersons
            (List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOption);

        /// <summary>
        /// Update an exist person in datasource
        /// </summary>
        /// <param name="personUpdateRequest">Details of person to update, includes PersonId</param>
        /// <returns>Return an PersonResponse after updated</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Delete a person in datasource by given personId
        /// </summary>
        /// <param name="personId">A id of person who must be delete</param>
        /// <returns>Return true or false</returns>
        Task<bool> DeletePerson(Guid? personId);
    }
}
