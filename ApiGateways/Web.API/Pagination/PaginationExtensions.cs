using System.Web;

namespace Web.API.Pagination;

public static class PaginationExtensions
{
    public static string ToQueryString(this PagedRequest request)
    {
        string[] parmas = request.GetType()
            .GetProperties()
            .Select(x => $"{x.Name}={HttpUtility.UrlEncode(x.GetValue(request).ToString())}")
            .ToArray();

        return "?" + string.Join("&", parmas);
    }
}
