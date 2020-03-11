using System;
using System.Collections.Generic;
using System.Linq;
using Cheeseria.Data;
using Cheeseria.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

/* [Implementation Nodes]:
 * In a more complex application I would split the business logic out into a service layer. As this api is
 * just a CRUD layer I reduced it to just a data and api layer.
 *
 * This application has no security on it. In a more complete application I would lock down the PUT, POST, DELETE to require some authentication and authorization.
 * Probably some combination JWT and integration with whatever existing directory the client had (i.e. Azure Active Directory). If they had none OAuth2 would be a
 * possibility.
 *
 * Persistence in this example is done into a in memory database with Entity Framework. I used this because it was the easiest way to implement a simple CRUD example.
 * In a more complete solution I would probably use Postgres or MS SQL. I would also store images outside the database either in a CDN or some kind
 * (S3, Azure Data blob) or in a folder on the web server that is stored outside the container by a volume. One consideration of storing files uploaded by users though
 * is some work would need to be done to make sure they are images and not allow the user to upload executable content that could be used to breach the security of the host.
 *
 * There are a lot of comments on asp.net standard classes which might seem excessive, this is because I am using xml docs for swagger documentation and not documenting methods
 * generates lots of compiler warnings.
 */


namespace Cheeseria.API.Controllers
{
    /// <summary>
    /// Cheese API End point handles all cheese related calls.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CheeseController : ControllerBase
    {
        private readonly CheeseContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CheeseController(CheeseContext context)
        {
            _context = context;

            //Force the context to create the database with test data if it hasn't already.
            _context.Database.EnsureCreated();
        }

        /// <summary>
        /// Get all cheeses that are published
        /// </summary>
        /// <param name="unpublished">If set to true will also return unpublished cheeses.</param>
        /// <returns>All cheese entries in the database.</returns>
        [HttpGet]
        //public IEnumerable<Cheese> Get(bool unpublished = false)
        public ActionResult<IEnumerable<Cheese>> Get(bool unpublished = false)
        {
            return Ok(_context.Cheeses.Where(c => unpublished || c.Published));
        }

        /// <summary>
        /// Get a single cheese item by its Id.
        /// </summary>
        /// <param name="id">Id of cheese to retrieve.</param>
        /// <returns>Cheese object.</returns>
        [HttpGet("{id}", Name = "GetCheese")]
        [ProducesResponseType(typeof(Cheese), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cheese> Get(int id)
        {
           var result = _context.Cheeses.FirstOrDefault(c => c.Id == id);

           if (result == default(Cheese))
           {
               return NotFound("Can't find cheese with that id!");
           }

           return Ok(result);

        }

        /// <summary>
        /// Creates a new cheese
        /// </summary>
        /// <param name="value">Cheese details to store. Leave Id as 0 to create a new entry with a new Id. If it is set to a value it must not already exist.</param>
        /// <returns>Id of newly created cheese. -1 if there was an error.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> Post([FromBody] Cheese value)
        {
            try
            {
                var cheese = _context.Cheeses.Add(value);
                _context.SaveChanges();

                return Ok(cheese.Entity.Id);
            }
            catch (Exception e)
            {

                if (e is DbUpdateException || e is InvalidOperationException)
                {
                    //Give user generic bad request error and would normally write error to log.
                    return BadRequest(
                        "Unable to create new cheese. Please check data is correct and if you set an ID that it doesn't already exist!");
                }

                //Not handling other exceptions to allow it to go to the global exception handler. In debug mode we get the exception and in prod we just get a error 500.
                //In a more complete app I would instead write this to a log file.
                throw;
            }
        }

        /// <summary>
        /// Updates an existing cheese.
        /// </summary>
        /// <param name="value">Cheese object to update. Set ID to id of cheese to update. Leave fields blank you don't want to update and they will be ignored
        /// (with the exception of published, if left blank it will be set to false).</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Put([FromBody] Cheese value)
        {
            var dbCheese = _context.Cheeses.FirstOrDefault(c => c.Id == value.Id);

            if (dbCheese == null)
            {
                return NotFound("Unable to find cheese with that Id!");
            }

            try
            {
                if (!string.IsNullOrEmpty(value.Colour))
                    dbCheese.Colour = value.Colour;

                if (!string.IsNullOrEmpty(value.Name))
                    dbCheese.Name = value.Name;

                if (value.Image != null && value.Image.Length > 0)
                    dbCheese.Image = value.Image;

                dbCheese.Published = value.Published;

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                if (e is DbUpdateException || e is InvalidOperationException)
                {
                    return BadRequest(
                        "Failed to update target cheese. Please make sure the data passed in was in correct format.");
                }
                    
                //Not handling other exceptions to allow it to go to the global exception handler. In debug mode we get the exception and in prod we just get a error 500.
                //In a more complete app I would instead write this to a log file.
                throw;

            }

            return Ok();
        }

        /// <summary>
        /// Deletes a cheese
        /// </summary>
        /// <param name="id">Id of cheese to remove.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            var cheese = _context.Cheeses.FirstOrDefault(c => c.Id == id);

            if (cheese == null)
            {
                return NotFound("Unable to find cheese with that ID!");
            }

            _context.Remove(cheese);
            _context.SaveChanges();

            return Ok();

            //Not handling other exceptions to allow it to go to the global exception handler. In debug mode we get the exception and in prod we just get a error 500.
            //In a more complete app I would instead write this to a log file.

        }
    }
}