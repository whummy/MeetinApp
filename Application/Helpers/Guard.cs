﻿using System.Net;

namespace Application.Helpers
{
    public static class Guard
    {
        public static void AgainstNull<T>(T value, HttpStatusCode code = HttpStatusCode.NotFound, string message = null)
        {
            if (value == null)
                throw new RestException(code, $"{typeof(T).Name} not found", message);
        }
        public static void AgainstDuplicate<T>(T value, string message = null, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            if (value != null)
                throw new RestException(code, $"Duplicate {typeof(T).Name}", message);
        }
        public static void AgainstNotInRole(bool value, string message = null, HttpStatusCode code = HttpStatusCode.NotFound)
        {
            if (value == false)
                throw new RestException(code, $"User does not have the required role", message);
        }
        public static void AgainstUserNotExist(bool value, string message = null, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            if (value == false)
                throw new RestException(code, $"User not found", message);
        }
        public static void AgainstUserExist(bool value, string message = null, HttpStatusCode code = HttpStatusCode.NotFound)
        {
            if (value == true)
                throw new RestException(code, "Duplicate User found", message);
        }
        public static void AgainstFailedTransaction(bool value, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            if (value == false)
                throw new RestException(code, "Internal server error.");
        }
        public static void AgainstStringNullOrEmpty(string value, HttpStatusCode code = HttpStatusCode.NotFound, string message = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new RestException(code, $"{value} is Null or Empty", message);
        }
        public static int ValidateIsNullOrWhiteSpace(string field, Dictionary<string, string> errors, int i, int count, string value)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                errors.Add($"{value}[{i}]", $"The {value} is required");
                count++;
            }
            return count;
        }
    }
}
