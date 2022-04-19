using ECXBookApp.Business.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using ECXBookApp.Extensions;
using ECXBookApp.Entities.Models;

namespace ECXBookApp.Filter
{
    public class UserSessionFilter : IActionFilter
    {
        private readonly IDataStore _store;
        public UserSessionFilter(IDataStore store)
        {
            _store = store;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var defaultUser = context.HttpContext.Session.Get<User>("UserDetail");
            if (defaultUser == null)
            {
                defaultUser = _store.GetDefaultUser();
                context.HttpContext.Session.Set("UserDetail", defaultUser);
            }
        }
    }
}
