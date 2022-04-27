using System;
using NUnit.Framework;

namespace PixelPass.Tests.Helpers;

internal class ParseExceptionHelper : BaseHelper
{
    static readonly string TypeName = "PixelPass.ParseException";

    public static readonly Type ParseExceptionType = Type.GetType($"{TypeName}, {AssemblyName}");

    public static bool ClassExistsAndInheritsFromParseException()
    {
        if (ParseExceptionType is not null)
        {
            return ParseExceptionType.IsAssignableTo(typeof(Exception));
        }
        return false;
    }
}