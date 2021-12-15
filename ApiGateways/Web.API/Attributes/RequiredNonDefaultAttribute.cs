namespace Web.API.Attributes;

using Attribute = System.ComponentModel.DataAnnotations.ValidationAttribute;

public class RequiredNonDefaultAttribute : Attribute
{
    public RequiredNonDefaultAttribute()
    {
        ErrorMessage = "Field must contain non default value";
    }

    public override bool IsValid(object? value)
    {
        if (value is null) return false;

        if (!value.GetType().IsValueType) return true;

        return !value.Equals(Activator.CreateInstance(value.GetType()));
    }
}
