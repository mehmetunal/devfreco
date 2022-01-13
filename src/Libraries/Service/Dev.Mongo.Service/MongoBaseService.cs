using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dev.Core.Infrastructure;
using Dev.Core.Model;
using Dev.Core.Repository;
using Dev.Data.Mongo;
using Dev.Mongo.Model.Pagination;
using Dev.Mongo.Repository;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Dev.Services
{
    public partial class MongoBaseService<TTable, TResultDto, TEditDto, TAddDto> where TTable : BaseEntity,
        new()
        where TResultDto : BaseDevModel,
        new()
        where TEditDto : BaseDevModel,
        new()
        where TAddDto : BaseDevModel
    {
        #region Properties

        protected readonly IMapper Mapper;
        protected readonly IMongoRepository<TTable> Repository;

        #endregion

        #region Ctor

        public MongoBaseService()
        {
            Mapper = EngineContext.Current.Resolve<IMapper>()
                     ?? throw new ArgumentNullException($"{nameof(IMapper)} is null");
            Repository = EngineContext.Current.Resolve<IMongoRepository<TTable>>()
                         ?? throw new ArgumentNullException($"{nameof(IMongoRepository<TTable>)} is null");
        }

        #endregion

        #region Method

        public virtual async Task<int> CountAsync()
        {
            return await Repository.Table.CountAsync();
        }

        public virtual async Task<PagedList<TResultDto>> GetAsync(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = Repository.Table;

            if (!showHidden)
                query = query.Where(p => p.IsPublish);

            query = query.Where(p => !p.IsDeleted);

            query = query.OrderBy(v => v.DisplayOrder);

            var result = await PagedList<TTable>.Create(query, pageIndex, pageSize);

            return Mapper.Map<PagedList<TResultDto>>(result);
        }

        public virtual async Task<TResultDto> GetByIdAsync(object id)
        {
            var result = await Repository.FindByIdAsync(id);
            return Mapper.Map<TResultDto>(result);
        }

        public virtual async Task<TResultDto> AddAsync(TAddDto companyAddDto)
        {
            if (companyAddDto == null)
                throw new ArgumentNullException($"{nameof(companyAddDto)}");

            var domainEntity = Mapper.Map<TTable>(companyAddDto);

            domainEntity.CreatedDate = DateTime.UtcNow;
            domainEntity.CreatorIP = RemoteIp;

            var result = await Repository.AddAsync(domainEntity);

            // event notification
            // await _mediator.EntityInserted(vendor);

            return Mapper.Map<TResultDto>(domainEntity);
        }

        public virtual async Task<TResultDto> UpdateAsync(TEditDto companyEditDto)
        {
            if (companyEditDto == null)
                throw new ArgumentNullException($"{nameof(companyEditDto)}");

            var domainEntity = Mapper.Map<TTable>(companyEditDto);

            domainEntity.ModifiedDate = DateTime.UtcNow;
            domainEntity.ModifierIP = RemoteIp;

            // event notification
            // await _mediator.EntityUpdated(vendor);

            var result = await Repository.UpdateAsync(domainEntity);

            return Mapper.Map<TResultDto>(result);
        }

        public virtual async Task<TResultDto> DeleteAsync(object id)
        {
            // var filter = Builders<Company>.Filter.Eq("Id", id);
            var result = await Repository.DeleteAsync(id);
            // event notification
            // await _mediator.EntityDeleted(vendorNote);
            return Mapper.Map<TResultDto>(result);
        }

        #endregion

        #region Prop

        protected virtual string RemoteIp => EngineContext.Current.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.ToString();

        #endregion
    }
}