using AspectCore.DynamicProxy;
using Dev.Npgsql.UnitOfWork;

namespace Dev.Npgsql.AOP
{
    /// 
    ///Provide transactional consistency for units of work
    /// 
    public class TransactionalAttribute : AbstractInterceptorAttribute
    {
        IUnitOfWork? UnitOfWork { get; set; }
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                UnitOfWork = context.ServiceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                if (UnitOfWork != null)
                {
                    await UnitOfWork.BeginNewTransactionAsync();
                    await next(context);
                    UnitOfWork.Commit();
                }
                else
                {
                    await next(context);
                }

            }
            catch (Exception)
            {
                if (UnitOfWork != null)
                {
                    await UnitOfWork.RollBackTransactionAsync();
                }
                throw;
            }
        }

    }
}
