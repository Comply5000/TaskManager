using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TaskManager.Core.Common.DTOs;

namespace TaskManager.Core.Common.Extensions;

public static class EnumExtensions
{       
    public static string? GetDisplayName(this Enum enumValue)
    {
        try
        {
            if (enumValue is not null)
            {
                var ev = enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .FirstOrDefault();

                if (ev != null && ev.CustomAttributes.Any())
                    return ev.GetCustomAttribute<DisplayAttribute>()?.GetName();
            }

            return enumValue?.ToString();
        }
        catch
        {
            return enumValue?.ToString();
        }
    }
    
    public static BaseEnum ToBaseEnum(this Enum enumValue)
    {
        var dto = new BaseEnum();

        try
        {
            dto.Name = enumValue.GetDisplayName();
            dto.Id = Convert.ToInt32(enumValue);
        }
        catch { /* ignored */ }
        
        return dto;
    }
    
    public static List<BaseEnum> GetValues<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Where(e => !e.Equals(default(TEnum))) 
            .Select(e => new BaseEnum { Id = Convert.ToInt32(e), Name = e.GetDisplayName()??e.ToString() })
            .ToList();
    }

    public static List<BaseEnum> GetObjectsFromFlag<TEnum>(this TEnum flagsEnum) where TEnum : Enum
    {
        var result = new List<BaseEnum>();

        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            if (value.Equals(Enum.ToObject(typeof(TEnum), 0)))
                continue;
            
            if (flagsEnum.HasFlag(value))
            {
                result.Add(value);
            }
        }
        
        return result;
    }
    
    public static List<TEnum> GetValuesFromFlag<TEnum>(this TEnum flagsEnum) where TEnum : Enum
    {
        var result = new List<TEnum>();

        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            if (value.Equals(Enum.ToObject(typeof(TEnum), 0)))
                continue;
            
            if (flagsEnum.HasFlag(value))
            {
                result.Add(value);
            }
        }
        
        return result;
    }

    public static TEnum AggregateFlags<TEnum>(this List<TEnum>? enumValues) where TEnum : Enum
    {
        if (enumValues == null)
            return (TEnum)Enum.ToObject(typeof(TEnum), 0);

        var resultValue = 0;

        foreach (var enumValue in enumValues)
        {
            resultValue |= Convert.ToInt32(enumValue);
        }

        var result = (TEnum)Enum.ToObject(typeof(TEnum), resultValue);
        return result;
    }

}
