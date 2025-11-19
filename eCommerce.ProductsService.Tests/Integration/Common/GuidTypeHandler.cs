using Dapper;
using System.Data;

namespace eCommerce.ProductsService.Tests.Integration.Common;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        var str = value.ToString();

        if (str is not null)
            return Guid.Parse(str);
        else
            return Guid.Empty;
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
        => parameter.Value = value.ToString();
}
