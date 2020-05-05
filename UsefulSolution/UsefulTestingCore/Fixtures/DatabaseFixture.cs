using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsefulDatabase.Model;

namespace UsefulTestingCore.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<UsefulContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            Context = new UsefulContext(options);
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public UsefulContext Context { get; private set; }
    }
}
