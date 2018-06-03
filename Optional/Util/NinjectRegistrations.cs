using Ninject.Modules;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Util
{
    public class NinjectRegistrations: NinjectModule
    {
        public override void Load()
        {
            Bind<IStudentRepository>().To<StudentRepository>();
            Bind<ICourseRepository>().To<CourseRepository>();
            Bind<IRegisterRepository>().To<RegisterRepository>();
            Bind(typeof(IRepository<LecturerRepository>)).To(typeof(LecturerRepository));
        }
    }
}