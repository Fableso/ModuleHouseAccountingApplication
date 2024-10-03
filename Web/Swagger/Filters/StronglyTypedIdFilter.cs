using Domain.StronglyTypedIds.Interface;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Swagger.Filters;

public class StronglyTypedIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var stronglyTypedIdInterface = Array.Find(context.Type.GetInterfaces(),
            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStronglyTypedId<>));

        if (stronglyTypedIdInterface == null) return;
        var valueType = stronglyTypedIdInterface.GetGenericArguments()[0];
            
        if (valueType == typeof(Guid))
        {
            schema.Type = "string";
            schema.Format = "uuid";
            schema.Example = new OpenApiString(Guid.NewGuid().ToString());
        }
        else if (valueType == typeof(int) || valueType == typeof(long))
        {
            schema.Type = "integer";
            schema.Format = valueType == typeof(int) ? "int32" : "int64";
            schema.Example = new OpenApiInteger(1);
        }
        else if (valueType == typeof(float) || valueType == typeof(double) || valueType == typeof(decimal))
        {
            schema.Type = "number";
            schema.Format = valueType == typeof(float) ? "float" : "double";
        }
        else
        {
            schema.Type = "string";
            schema.Format = null;
            schema.Example = new OpenApiString("TestHouseModel");
        }

        schema.Properties.Clear();
        schema.Reference = null;
        schema.AllOf.Clear();
    }
}
