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
            Bind(typeof(IRepository<CourseRepository>)).To(typeof(CourseRepository));
            Bind(typeof(IRepository<RegisterRepository>)).To(typeof(RegisterRepository));
            Bind(typeof(IRepository<LecturerRepository>)).To(typeof(LecturerRepository));
        }
    }
}