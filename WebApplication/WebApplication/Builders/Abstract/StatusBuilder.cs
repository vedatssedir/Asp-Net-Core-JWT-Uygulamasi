using WebApplication.Builders.Concrete;
using WebApplication.Models;

namespace WebApplication.Builders.Abstract{
    public abstract class StatusBuilder{
        public abstract Status GenerateStatus(AppUser activeUser, string roles); 
    }
}