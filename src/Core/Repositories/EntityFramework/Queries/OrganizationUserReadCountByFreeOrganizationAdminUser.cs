using System.Collections.Generic;
using System.Linq;
using Bit.Core.Enums;
using Bit.Core.Models.Table;
using System;

namespace Bit.Core.Repositories.EntityFramework.Queries
{
    public class OrganizationUserReadCountByFreeOrganizationAdminUser : IQuery<OrganizationUser>
    {
        private Guid UserId { get; set; }

        public OrganizationUserReadCountByFreeOrganizationAdminUser(Guid userId)
        {
            UserId = userId;
        }

        public IQueryable<OrganizationUser> Run(DatabaseContext dbContext)
        {
            var query = from ou in dbContext.OrganizationUsers
                        join o in dbContext.Organizations
                            on ou.OrganizationId equals o.Id
                        where ou.UserId == UserId &&
                            (ou.Type == OrganizationUserType.Owner || ou.Type == OrganizationUserType.Admin) &&
                            o.PlanType == PlanType.Free &&
                            ou.Status == OrganizationUserStatusType.Confirmed
                        select new { ou, o };
                                
            return query.Select(x => x.ou);
        }
    }
}
