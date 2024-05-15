namespace AskApplicant.Infrastructure.Repository
{

    public interface IAskApplicantRepository<T> where T : class
    {
        T GetById(int id);
    }

    public class AskApplicantRepository<T> : IAskApplicantRepository<T> where T : class
    {

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
