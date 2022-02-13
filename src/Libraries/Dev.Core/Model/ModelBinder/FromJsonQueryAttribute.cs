using Microsoft.AspNetCore.Mvc;

namespace Dev.Core.Model.ModelBinder
{
    public class FromJsonQueryAttribute : ModelBinderAttribute
    {
        public FromJsonQueryAttribute()
        {
            BinderType = typeof(JsonQueryBinder);
        }
    }
}
