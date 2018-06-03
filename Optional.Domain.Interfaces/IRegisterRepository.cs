using Optional.Domain.Core;

namespace Optional.Domain.Interfaces
{
    public interface IRegisterRepository
    {
        Register Get(int id);
        int GetMarkOfStudent(int courseId, string userName);
        void Create(Register item, int courseId, string studentName);
        void Update(Register item);
        void Dispose();
    }
}