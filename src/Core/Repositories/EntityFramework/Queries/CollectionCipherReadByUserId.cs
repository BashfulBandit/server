using System.Linq;
using Bit.Core.Models.Table;
using System;
using Bit.Core.Enums;

namespace Bit.Core.Repositories.EntityFramework.Queries
{
    public class CollectionCipherReadByUserId : IQuery<CollectionCipher>
    {
        private Guid UserId { get; set; }

        public CollectionCipherReadByUserId(Guid userId)
        {
            UserId = userId;
        }

        public virtual IQueryable<CollectionCipher> Run(DatabaseContext dbContext)
        {
            var query = from cc in dbContext.CollectionCiphers
                join c in dbContext.Collections 
                    on cc.CollectionId equals c.Id
                join ou in dbContext.OrganizationUsers
                    on c.OrganizationId equals ou.OrganizationId
                where ou.UserId == UserId
                join cu in dbContext.CollectionUsers
                    on c.Id equals cu.CollectionId into cu_g
                from cu in cu_g
                where ou.AccessAll && cu.OrganizationUserId == ou.Id
                join gu in dbContext.GroupUsers
                    on ou.Id equals gu.OrganizationUserId into gu_g
                from gu in gu_g
                where cu.CollectionId == null && !ou.AccessAll
                join g in dbContext.Groups
                    on gu.GroupId equals g.Id into g_g
                from g in g_g
                join cg in dbContext.CollectionGroups
                    on cc.CollectionId equals cg.CollectionId into cg_g
                from cg in cg_g
                where g.AccessAll && cg.GroupId == gu.GroupId  &&
                    ou.Status == OrganizationUserStatusType.Confirmed &&
                    (ou.AccessAll || cu.CollectionId != null || g.AccessAll || cg.CollectionId != null)
                select new { cc, c, ou, cu, gu, g, cg };
                return query.Select(x => x.cc);
        }
    }
}
