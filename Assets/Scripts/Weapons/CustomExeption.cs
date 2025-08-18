using System;

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }

    public CustomException() : base("Custom Exception") { }

}