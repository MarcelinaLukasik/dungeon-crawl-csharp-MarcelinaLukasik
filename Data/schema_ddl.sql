using Microsoft.Data.SqlClient;
using System.Configuration;
using Codecool.BookDb.Model;
using Codecool.BookDb.View;

namespace Codecool.BookDb.Manager
{
    public class BookDbManager : BaseManager
    {
        private UserInterface _ui;
        private AuthorDao _authorDao;
        private BookDao _bookDao;
        private IAuthorDao iAuthorDao;
        string ConnectionString => ConfigurationManager.AppSettings["connectionString"];

        public BookDbManager(UserInterface ui) : base(ui)
        {
            iAuthorDao = new AuthorDao(ConnectionString);
            this._authorDao = new AuthorDao(ConnectionString);
            _bookDao = new BookDao(ConnectionString, iAuthorDao);
            _ui = ui;
        }

        public bool TestConnection()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                     connection.Open();
                     return true;
                }
                catch (Exception)
                {
                     return false;
                }

            }
        }