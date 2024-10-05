namespace LylinkBackend_API.Renderers
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewPath, TModel model);
    }

}