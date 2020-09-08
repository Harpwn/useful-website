using UsefulDatabase.Model;

namespace UsefulServices.Services
{
    public abstract class Service
    {
        protected UsefulContext Context;

        public Service(UsefulContext context)
        {
            Context = context;
        }
    }
}
