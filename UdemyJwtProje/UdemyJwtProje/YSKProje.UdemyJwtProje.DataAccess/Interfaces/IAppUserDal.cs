using System.Collections.Generic;
using System.Threading.Tasks;
using YSKProje.UdemyJwtProje.Entities.Concrete;

namespace YSKProje.UdemyJwtProje.DataAccess.Interfaces
{
    public interface IAppUserDal : IGenericDal<AppUser>
    {
        Task<List<AppRole>> GetRolesByUserName(string userName);
    }
}
