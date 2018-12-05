namespace AJG.TravelApp.DataAccess.Interfaces
{
    public interface IGeneratedCertificateDataAccess
    {
        Models.GeneratedCertificate GetById(int id);

        void Insert(Models.GeneratedCertificate generatedCertificate);
    }
}